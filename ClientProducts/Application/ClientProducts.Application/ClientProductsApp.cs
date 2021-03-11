using System;
using System.Collections.Generic;
using ClientProducts.Domain.ProductsOfClienteAggregate;
using ClientProducts.Domain.ProductAggregate;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Repository;
using ClientProducts.Domain.Contributions;
using ClientProducts.Domain.ClientSolvenciaDataAggregate;
using ClientProducts.Logs;
using Common.Domain;
using System.Configuration;
using SkCo.SecurityPolicyLevel.OTP.OTP;
using prxyNotsTypes = ClientProducts.Repository.NotificationsProxyTypes;

namespace ClientProducts.Application
{
    public class ClientProductsApp : IClientProductsApp
    {
        private readonly ISeriLogHelper _log;
        private readonly IRAWRAPSIIFDa _repositoryContract;
        private readonly IRADa _repositoryPlan;
        private readonly IDOMICILIACIONDa _repositoryDomiciliacion;
        private readonly INotificationsProxy _notificationsProxy;
        private readonly ISecurityGlobalAppDa _repositorySecurityGlobalAppDa;

        public ClientProductsApp(ISeriLogHelper log, IRAWRAPSIIFDa iRawRapSiifDa, IRADa iRaDa,
            INotificationsProxy iNotificationsProxy, ISecurityGlobalAppDa iSecurityGlobalAppDa, IDOMICILIACIONDa iDomiciliacion)
        {
            _log = log;
            _repositoryContract = iRawRapSiifDa;
            _repositoryPlan = iRaDa;
            _notificationsProxy = iNotificationsProxy;
            _repositorySecurityGlobalAppDa = iSecurityGlobalAppDa;
            _repositoryDomiciliacion = iDomiciliacion;
        }

        public ProductsOfClient GetClientProducts(string clientIdentifier, out OperationResult result)
        {
            result = new OperationResult();
            if (string.IsNullOrEmpty(clientIdentifier)) { throw new ArgumentException("El clientIdentifier no puede ir vacío o null"); }
            try
            {
                var clientContracts = _repositoryContract.GetClientContracts(clientIdentifier);
                var clientProductsSummaryBalance = GetContractsSummaryBalance(clientContracts);
                var clientProductsList = FillProductsList(clientIdentifier, clientProductsSummaryBalance);
                result.Successful = true;
                return clientProductsList;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error al obtener los productos del cliente");
                result.Successful = false;
                result.SystemMessages.Add(new SystemMessage { Message = "Error al obtener los productos del cliente - " + ex.Message + " - " + ex.StackTrace, MessageType = SystemMessageTypes.Error });
                return null;
            }
        }

        public Contract GetContractDetail(string contractId, out OperationResult result)
        {
            result = new OperationResult { Successful = true };

            if (string.IsNullOrEmpty(contractId)) { throw new ArgumentException("contractId no puedes ser nulo o vacío."); }
            if (contractId.Length < 6) { throw new ArgumentException("contractId no puedes ser nulo o vacío."); }

            try
            {
                contractId = contractId.Substring(0, 6);
                Product productInfo = _repositoryPlan.GetContractProductInfo(contractId);
                Plan planInfo = _repositoryPlan.GetPlanInfo(contractId);
                ContractSOCData contractSOCInfo = _repositoryPlan.GetContractSOCInfo(contractId, planInfo.PlanComercialName);
                Contract contractDetail = _repositoryContract.GetContractDetailInfo(contractId, ref contractSOCInfo, out SavingInformation savingInformation);
                SavingAdvance savingAdvanceInfo = _repositoryContract.GetContractSavingAdvanceInfo(contractDetail, ref contractSOCInfo);
                var subContracts = _repositoryContract.GetSubContracts(contractId);
                CapitalBalance capitalBalance = GetCapitalBalance(contractId, contractDetail.ContractBasicInfo, contractSOCInfo, subContracts);
                savingAdvanceInfo.FillYieldAmount(capitalBalance.YieldAmount);
                var contractBalance = _repositoryContract.GetContractBalances(contractId, subContracts);
                var investments = new Investments(contractBalance);
                Domiciliation domiciliation = _repositoryDomiciliacion.GetContractDirectDebitBasicInfo(contractId);
                domiciliation = domiciliation.AccountNumber.Length == 0 ? _repositoryContract.GetRecurringPayments(contractId, contractDetail.ContractBasicInfo.Plataform) : domiciliation;
                if (domiciliation.AccountNumber.Length > 0)
                {
                    domiciliation.FillBank(_repositoryContract.GetBankName(contractId, contractDetail.ContractBasicInfo.Plataform, domiciliation));
                }

                contractDetail
                    .FillProduct(productInfo)
                    .FillInvestments(investments)
                    .FillSavingAdvance(savingAdvanceInfo)
                    .FillCapitalBalance(capitalBalance)
                    .FillDomiciliation(domiciliation);

                return contractDetail;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error al obtener el detalle del contrato");
                result.SystemMessages.Add(new SystemMessage
                {
                    Message = "Ha ocurrido un error al obtener el detalle del contrato. " + ex.Message,
                    MessageType = SystemMessageTypes.Error
                });
                return null;
            }
        }

        public ContractContributionsInfo GetContractContributionsInfo(string contractId, out OperationResult result)
        {
            result = new OperationResult { Successful = true };
            try
            {
                var subaccountsContracts = _repositoryContract.GetOffspringContract(contractId, 0);
                var subaccountsText = GetContractContributionSubaccounts(_repositoryPlan.GetPlanInfo(contractId).Id, subaccountsContracts, out result);
                var amountDue = 0M;
                if (subaccountsContracts != null)
                {
                    amountDue = _repositoryContract.GetContributionsAmountDue(subaccountsContracts.ContributionContractId);
                }
                var minimumAmount = _repositoryContract.GetContributionMinimumAmount(Convert.ToInt32(ConfigurationManager.AppSettings["TipoDomiciliacion"].ToString()));
                var bankAccounts = GetContractBankAccounts(contractId, out result);
                var socData = _repositoryPlan.GetContractSOCInfo(contractId, "Contribucion");
                var contractContributionsInfo = ContractContributionsInfo.Create(contractId, socData.CommitedAmount, minimumAmount, amountDue, subaccountsText, bankAccounts);
                return contractContributionsInfo;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error al obtener información para la aportación");
                result.SystemMessages.Add(new SystemMessage
                {
                    Message = "Ha ocurrido un error al obtener información para la aportación. " + ex.Message,
                    MessageType = SystemMessageTypes.Error
                });
                result.Successful = false;
                return null;
            }

        }

        public List<ContractContributionSubaccount> GetContractContributionSubaccounts(string productId, OffspringGrouperContract subaccountsContracts, out OperationResult result)
        {
            result = new OperationResult { Successful = true };
            try
            {
                if (string.IsNullOrEmpty(productId)) { throw new ArgumentException("El productId no puede ir vacío o null"); }
                return _repositoryContract.GetContributionsSubaccounts(productId, subaccountsContracts);

            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error al obtener los textos de las subcuentas");
                result.Successful = false;
                result.SystemMessages.Add(new SystemMessage { Message = "Ocurrio un error al obtener los textos de las subcuentas - " + ex.Message + "-" });
                return null;
            }
        }

        public List<ContractContributionsSubaccountText> GetContributionsSubaccountsTexts(string productId, out OperationResult result)
        {
            result = new OperationResult { Successful = true };
            try
            {
                if (string.IsNullOrEmpty(productId)) { throw new ArgumentException("El productId no puede ir vacío o null"); }

                return _repositoryPlan.GetContributionsSubaccountsTexts(productId);

            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error al obtener los textos de las subcuentas");
                result.Successful = false;
                result.SystemMessages.Add(new SystemMessage { Message = "Ocurrio un error al obtener los textos de las subcuentas - " + ex.Message + "-" });
                return null;
            }
        }

        public List<ContractBankAccount> GetContractBankAccounts(string contractId, out OperationResult result)
        {
            result = new OperationResult { Successful = true };

            try
            {
                if (string.IsNullOrEmpty(contractId)) { throw new ArgumentException("El productId no puede ir vacío o null"); }

                return _repositoryContract.GetContractBankAccounts(contractId);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error al obtener las cuentas bancarias del contrato");
                result.Successful = false;
                result.SystemMessages.Add(new SystemMessage { Message = "Ocurrio un error al obtener las cuentas bancarias del contrato - " + ex.Message + "-" });
                return null;
            }
        }

        public ClientOTP GenerateNewOTP(string userId, out OperationResult result)
        {
            result = new OperationResult { Successful = true };
            ClientOTP otpData = null;
            try
            {
                if (string.IsNullOrEmpty(userId)) { throw new ArgumentException("El userId no puede ir vacío o null"); }

                var portalId = Convert.ToInt32(ConfigurationManager.AppSettings["PortalId"].ToString());
                var minutos = Convert.ToInt32(ConfigurationManager.AppSettings["OTP:PinMinutesDuration"].ToString());
                var operation = ConfigurationManager.AppSettings["Notifications:OTP"].ToString();

                var solvData = _repositorySecurityGlobalAppDa.GetClientDatosSolvencia(userId, portalId);

                if (solvData == null)
                {
                    result.Successful = false;
                    result.SystemMessages.Add(new SystemMessage { Message = "Error al obtener los datos de solvencia." });
                    return null;
                }

                var strPIN = _repositorySecurityGlobalAppDa.GetActivePIN(userId, portalId, minutos);
                EncryptDecrypt ed = new EncryptDecrypt(_repositorySecurityGlobalAppDa.GetParameterValue("pwdHash", portalId).ToString(), _repositorySecurityGlobalAppDa.GetParameterValue("saltKey", portalId).ToString(), _repositorySecurityGlobalAppDa.GetParameterValue("ivKey", portalId).ToString());

                if (strPIN == string.Empty || strPIN == "000000")
                {
                    TOTP newOTP = new TOTP("password", 0.0001);
                    var pinvalid = false;
                    while (!pinvalid)
                    {
                        int pin = newOTP.now();
                        strPIN = pin.ToString().Length < 6 ? strPIN.PadLeft(6, '0') : pin.ToString();
                        if (strPIN != "000000")
                        {
                            pinvalid = true;
                        }
                    }

                    var encPIN = ed.Encrypt(strPIN);
                    _repositorySecurityGlobalAppDa.SetPinUser(encPIN, userId, portalId, minutos);

                }
                else if (strPIN.Contains("ERROR"))
                {
                    result.Successful = false;
                    result.SystemMessages.Add(new SystemMessage { Message = strPIN, MessageType = SystemMessageTypes.Error });
                    return null;
                }
                else
                {
                    strPIN = ed.Decrypt(strPIN);
                }

                result = _notificationsProxy.NotificationsAppSync(FillTemplateOTP(operation, solvData, strPIN));

                if (result.Successful)
                {
                    otpData = ClientOTP.Create(userId, strPIN, solvData.Email, solvData.CellPhone);
                }

                return otpData;
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.SystemMessages.Add(new SystemMessage { Message = "Error al generar el OTP - " + ex.Message });
                return null;
            }
        }

        public OperationResult GetOTPValidation(string userId, string pin, out bool valid, out bool expired)
        {
            OperationResult result = new OperationResult();
            valid = false;
            expired = false;
            var portalId = Convert.ToInt32(ConfigurationManager.AppSettings["PortalId"].ToString());
            if (string.IsNullOrEmpty(userId)) { throw new ArgumentException("El userId no puede ir vacío o null"); }
            if (string.IsNullOrEmpty(pin)) { throw new ArgumentException("El pin no puede ir vacío o null"); }

            try
            {
                var strPIN = _repositorySecurityGlobalAppDa.GetActivePIN(userId, portalId, 0);
                if (strPIN.Equals("000000"))
                {
                    result.SystemMessages.Add(new SystemMessage { MessageType = SystemMessageTypes.Warning, Message = "El PIN ha caducado." });
                    result.Successful = true;
                    expired = true;
                }
                else
                {
                    EncryptDecrypt ed = new EncryptDecrypt(_repositorySecurityGlobalAppDa.GetParameterValue("pwdHash", portalId).ToString(), _repositorySecurityGlobalAppDa.GetParameterValue("saltKey", portalId).ToString(), _repositorySecurityGlobalAppDa.GetParameterValue("ivKey", portalId).ToString());
                    var encPIN = ed.Encrypt(pin);
                    if (encPIN.Equals(strPIN))
                    {
                        result.Successful = true;
                        valid = true;
                    }
                    else
                    {
                        result.SystemMessages.Add(new SystemMessage { MessageType = SystemMessageTypes.Warning, Message = "El PIN es inválido." });
                        result.Successful = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.SystemMessages.Add(new SystemMessage { MessageType = SystemMessageTypes.Warning, Message = "Error al validar el PIN." });
                _log.Error(ex, "Error al validar el PIN");
            }
            return result;
        }

        #region"OTP Private Methods"
        public prxyNotsTypes.SendNotificationDistListRequest FillTemplateOTP(string template, SolvenciaData solvenciaData, string newPIN)
        {
            var email = solvenciaData.Email;
            var phone = solvenciaData.CellPhone.Replace(" ", string.Empty);
            var userName = solvenciaData.FullName;

            /** PARAMETROS DEL HTML **/
            Dictionary<string, string> dsMensajes = new Dictionary<string, string>
            {
                { "Email", email },
                { "Poliza", "000123" },
                { "Valor", newPIN }
            };

            prxyNotsTypes.SendNotificationDistListRequest request = new prxyNotsTypes.SendNotificationDistListRequest
            {
                ContextBusiness = new prxyNotsTypes.ContextBusinessN
                {
                    Application = "PortalCliente",
                    Process = "OTP",
                    ClientEmail = email,
                    ClientMobile = phone,
                    ClientIdentifier = userName,
                    Contract = ""
                },
                SendParameters = new prxyNotsTypes.SendParametersN
                {
                    Amount = 0,
                    ClientIdentifier = userName,
                    ClientTypeId = "C",
                    Contract = newPIN,
                    EmailFP = "",
                    MobileClient = phone,
                    MobileFP = "",
                    EmailClient = email,
                    AdditionalMessages = dsMensajes
                },
                Plantilla = (NotificationsProxyTypes.NotificationTemplateN)Enum.Parse(typeof(NotificationsProxyTypes.NotificationTemplateN), template),
                LstEmail = new string[] {}
            };

            return request;
        }
        #endregion

        #region "Contract Detail Methods"
        private CapitalBalance GetCapitalBalance(string contractId, ContractInfo contractInfo, ContractSOCData socData, List<string> subContracts)
        {
            var queryDates = new [] {
                        Convert.ToDateTime(contractInfo.AfiliationDate),
                        DateTime.Now,
                        Convert.ToDateTime(contractInfo.AfiliationDate).AddDays(-1)
                    };
            var capitalBalance = _repositoryContract.GetContractBalancesHistory(contractId, queryDates, true, false, contractInfo.Plataform, subContracts);
            var charges = _repositoryContract.GetContractCharges(contractId, subContracts);
            capitalBalance.FillData(socData.TotalContributions, socData.CurrentLifeInsuranceAmount, socData.TotalWithDrawals, charges);
            return capitalBalance;
        }
        #endregion

        #region "Summary Balance Private Methods"
        private Dictionary<string, double> GetContractsSummaryBalance(Dictionary<string, string> clientContracts)
        {
            Dictionary<string, double> dictContranctsBalance = new Dictionary<string, double>();
            var summaryBalance = 0D;

            foreach (var contract in clientContracts)
            {
                var contractId = contract.Key;
                var balance = _repositoryContract.GetContractSummaryBalance(contractId);
                var contractGrouped = string.IsNullOrEmpty(contract.Value) ? contract.Key : contract.Value;
                if (dictContranctsBalance.TryGetValue(contractGrouped, out summaryBalance))
                {
                    dictContranctsBalance[contractGrouped] = summaryBalance + balance;
                }
                else
                {
                    dictContranctsBalance.Add(contractGrouped, balance);
                }
            }

            return dictContranctsBalance;
        }

        private ProductsOfClient FillProductsList(string clientIdentifier, Dictionary<string, double> clientContracts)
        {

            ProductsOfClient productsOfClient = new ProductsOfClient(clientIdentifier);
            foreach (var contract in clientContracts)
            {
                var plan = _repositoryPlan.GetPlanInfo(contract.Key);
                var product = ProductSummary
                                .Create(contract.Key, contract.Value)
                                .FillPlan(plan);
                productsOfClient.AddProduct(product);
            }

            return productsOfClient;
        }
        #endregion
    }
}
