using System.Configuration;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientProducts.Domain.ProductsOfClienteAggregate;
using ClientProducts.Domain.ProductAggregate;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Domain.Contributions;
using ClientProducts.Domain.ClientSolvenciaDataAggregate;
using ClientProducts.Repository;
using ClientProducts.Logs;
using ClientProducts.Application;
using prxyNotsTypes = ClientProducts.Repository.NotificationsProxyTypes;
using Common.Domain;
using Moq;
using System;

namespace ClientProducts.ApplicationTest
{
    [TestClass]
    public class ClientProductsAppTest
    {
        private readonly Mock<IRAWRAPSIIFDa> _repositoryContract;
        private readonly Mock<IRADa> _repositoryPlan;
        private readonly Mock<ISeriLogHelper> _SeriloHelper;
        private readonly ClientProductsApp _clientProductsAppBL;
        private readonly Mock<IDOMICILIACIONDa> _repositoryDomiciliacion;
        private readonly Mock<INotificationsProxy> _notificationsProxy;
        private readonly Mock<ISecurityGlobalAppDa> _repositorySecurityGlobalAppDa;
        private Product productInfo;
        private Plan planInfo;
        private ContractSOCData contractSOC;
        private Contract contractDetail;
        private ContractInfo contractInfo;
        private CapitalBalance capitalBalance;
        private List<InvestmentContractBalance> contractsBalance;
        private SavingAdvance savingAdvance;
        private Domiciliation domiciliation;
        OperationResult result;

        public ClientProductsAppTest()
        {
            _repositoryContract = new Mock<IRAWRAPSIIFDa>();
            _repositoryPlan = new Mock<IRADa>();
            _SeriloHelper = new Mock<ISeriLogHelper>();
            _repositoryDomiciliacion = new Mock<IDOMICILIACIONDa>();
            _notificationsProxy = new Mock<INotificationsProxy>();
            _repositorySecurityGlobalAppDa = new Mock<ISecurityGlobalAppDa>();
            _clientProductsAppBL = new ClientProductsApp(_SeriloHelper.Object, _repositoryContract.Object, _repositoryPlan.Object,
               _notificationsProxy.Object, _repositorySecurityGlobalAppDa.Object, _repositoryDomiciliacion.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            _repositoryPlan
                .Setup(x => x.GetContributionsSubaccountsTexts("542"))
                .Returns(
                    new List<ContractContributionsSubaccountText>() { ContractContributionsSubaccountText.Create(1, 1, "Encabezado", "Detalle") }
                );

            _notificationsProxy
               .Setup(x => x.NotificationsAppSync(new prxyNotsTypes.SendNotificationDistListRequest()))
               .Returns(new OperationResult() { Successful = true });

            productInfo = Product.Create("599500", "540", "CREA");
            planInfo = new Plan("540", "CREA", 1);
            contractSOC = ContractSOCData.Create("CREA");
            contractInfo = new ContractInfo("12346", "12/12/2010", false, "Activo", 1, 1, contractSOC);
            contractDetail = Contract.Create("599500", contractSOC, contractInfo);
            capitalBalance = new CapitalBalance(1245M);
            savingAdvance = new SavingAdvance(contractSOC, 10, 5);
            InvestmentContractBalance contractBalance = InvestmentContractBalance.Create("599500");
            contractBalance.FillLists(new List<InvestmentFundValue>(), new List<InvestmentFundValue>(), new List<InvestmentFundAllocation>() );
            contractsBalance = new List<InvestmentContractBalance>() { contractBalance };
            domiciliation = Domiciliation.Create("", 0, 0M);

        }

        [TestMethod, TestCategory("ParamError")]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow("")]
        [DataRow("1234")]
        public void GetContractDetailParamError(string contractId)
        {
            result = new OperationResult();
            _clientProductsAppBL.GetContractDetail(contractId, out result);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractDetailSuccess()
        {
            result = new OperationResult();
            SavingInformation savingInformation;
            List<string> subcontracts = new List<string>();
            _repositoryPlan.Setup(c => c.GetContractProductInfo(It.IsAny<string>())).Returns(productInfo);
            _repositoryPlan.Setup(c => c.GetPlanInfo(It.IsAny<string>())).Returns(planInfo);
            _repositoryPlan.Setup(c => c.GetContractSOCInfo(It.IsAny<string>(), It.IsAny<string>())).Returns(contractSOC);
            _repositoryContract.Setup(c => c.GetSubContracts(It.IsAny<string>())).Returns(subcontracts);
            _repositoryContract.Setup(c => c.GetContractBalances(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(contractsBalance);
            _repositoryContract.Setup(c => c.GetContractSavingAdvanceInfo(contractDetail, ref contractSOC)).Returns(savingAdvance);
            _repositoryContract.Setup(c => 
                c.GetContractBalancesHistory(It.IsAny<string>(), It.IsAny<DateTime[]>(), It.IsAny<Boolean>(), It.IsAny<Boolean>(), It.IsAny<int>(), It.IsAny<List<string>>())).Returns(capitalBalance);
            _repositoryContract.Setup(c => c.GetContractCharges(It.IsAny<string>(), It.IsAny<List<string>>())).Returns(100M);
            _repositoryContract.Setup(c => c.GetContractDetailInfo(It.IsAny<string>(), ref contractSOC, out savingInformation)).Returns(contractDetail);
            _repositoryDomiciliacion.Setup(c => c.GetContractDirectDebitBasicInfo(It.IsAny<string>())).Returns(domiciliation);
            _clientProductsAppBL.GetContractDetail("MEAJ72080855", out result);
            Assert.IsTrue(result.Successful);
        }

        [TestMethod, TestCategory("ParamError")]
        [ExpectedException(typeof(ArgumentException))]
        public void GetClientProductsParamError()
        {
            result = new OperationResult();
            _clientProductsAppBL.GetClientProducts("", out result);
        }

        [TestMethod, TestCategory("GeneralError")]
        public void GetClientProductsGeneralError()
        {
            result = new OperationResult();
            _clientProductsAppBL.GetClientProducts("MEAJ72080855", out result);
            Assert.IsFalse(result.Successful);
        }

        [TestMethod, TestCategory("ParamError")]
        public void GetClientProductsSuccess()
        {
            result = new OperationResult();
            var clientContracts = new Dictionary<string, string>() { { "599500", "599500" }, { "585485", "" } };
            Plan plan = new Plan("549", "Test", 1);
            
            _repositoryContract.Setup(c => c.GetClientContracts(It.IsAny<string>())).Returns(clientContracts);
            _repositoryPlan.Setup(c => c.GetPlanInfo(It.IsAny<string>())).Returns(plan);
            _clientProductsAppBL.GetClientProducts("MEAJ72080855", out result);
            Assert.IsTrue(result.Successful);
        }
        

        [TestMethod, TestCategory("ParamError")]
        public void GetContributionsSubaccountsTextsParamError()
        {
            OperationResult result = new OperationResult();
            var clientContract = _clientProductsAppBL.GetContributionsSubaccountsTexts("", out result);

            Assert.IsFalse(result.Successful);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContributionsSubaccountsTextsSuccess()
        {
            OperationResult result = new OperationResult();
            var sendNotification = _clientProductsAppBL.GetContributionsSubaccountsTexts("549", out result);

            Assert.IsTrue(result.Successful && result.SystemMessages.Count == 0);
        }

        [TestMethod, TestCategory("ParamError")]
        public void GetContractBankAccountsParamError()
        {
            OperationResult result = new OperationResult();
            var clientContract = _clientProductsAppBL.GetContractBankAccounts("", out result);

            Assert.IsFalse(result.Successful);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractBankAccountsSuccess()
        {
            OperationResult result = new OperationResult();
            var sendNotification = _clientProductsAppBL.GetContractBankAccounts("599500", out result);

            Assert.IsTrue(result.Successful && result.SystemMessages.Count == 0);
        }

        [TestMethod, TestCategory("ParamError")]
        public void GenerateNewOTPParamError()
        {
            OperationResult result = new OperationResult();
            var clientContract = _clientProductsAppBL.GenerateNewOTP("", out result);

            Assert.IsFalse(result.Successful);
        }

        [TestMethod, TestCategory("SolvenciaError")]
        //[ExpectedException(typeof(Exception))]
        public void GenerateNewOTPSolvenciaError()
        {
            OperationResult result = new OperationResult();
            ConfigurationManager.AppSettings["PortalId"] = "2";
            ConfigurationManager.AppSettings["OTP:PinMinutesDuration"] = "4";
            ConfigurationManager.AppSettings["Notifications:OTP"] = "1";

            SolvenciaData solvData = null;
            _repositorySecurityGlobalAppDa.Setup(c => c.GetClientDatosSolvencia(It.IsAny<string>(), It.IsAny<int>())).Returns(solvData);
            var clientContract = _clientProductsAppBL.GenerateNewOTP("MEAJ720808J25", out result);
            Assert.IsFalse(result.Successful && result.SystemMessages[0].Message.Equals("Error al obtener los datos de solvencia."));
        }

        [TestMethod, TestCategory("PINError")]
        public void GenerateNewOTPPINError()
        {
            OperationResult result = new OperationResult();
            ConfigurationManager.AppSettings["PortalId"] = "2";
            ConfigurationManager.AppSettings["OTP:PinMinutesDuration"] = "4";
            ConfigurationManager.AppSettings["Notifications:OTP"] = "1";

            SolvenciaData solvData = SolvenciaData.Create("MEAJ720808J25", "email", "Nombre completo", "5540681788");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetClientDatosSolvencia(It.IsAny<string>(), It.IsAny<int>())).Returns(solvData);
            _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("ERROR");
            _notificationsProxy.Setup(x => x.NotificationsAppSync(new prxyNotsTypes.SendNotificationDistListRequest())).Returns(new OperationResult() { Successful = true });
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("P@SsW0rD");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("sR|mItDT|KTNiUwiLlfBk9=Vn5cE2H91Q34ri%u=iKMHs~f60Uld*rd9Lugk");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("Ajbd7sozZcwz/FR0+jMUNw==");
            var clientContract = _clientProductsAppBL.GenerateNewOTP("MEAJ720808J25", out result);
            Assert.IsTrue(!result.Successful && result.SystemMessages[0].Message.Contains("ERROR"));
        }

        //[TestMethod, TestCategory("Success")]
        //public void GenerateNewOTPValidPIN()
        //{
        //    OperationResult result = new OperationResult();
        //    ConfigurationManager.AppSettings["PortalId"] = "2";
        //    ConfigurationManager.AppSettings["OTP:PinMinutesDuration"] = "4";
        //    ConfigurationManager.AppSettings["Notifications:OTP"] = "1";

        //    SolvenciaData solvData = SolvenciaData.Create("MEAJ720808J25", "email", "Nombre completo", "5540681788");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetClientDatosSolvencia(It.IsAny<string>(), It.IsAny<int>())).Returns(solvData);
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("SmuwVXFibs/JC9wg2LYQGw==");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("P@SsW0rD");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("sR|mItDT|KTNiUwiLlfBk9=Vn5cE2H91Q34ri%u=iKMHs~f60Uld*rd9Lugk");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("Ajbd7sozZcwz/FR0+jMUNw==");
        //    var otpData = _clientProductsAppBL.GenerateNewOTP("MEAJ720808J25", out result);
        //    Assert.IsTrue(result.Successful && otpData.NewOTP.Length>0);
        //}

        //[TestMethod, TestCategory("Success")]
        //public void GenerateNewOTPValidCreatePIN()
        //{
        //    OperationResult result = new OperationResult();
        //    ConfigurationManager.AppSettings["PortalId"] = "2";
        //    ConfigurationManager.AppSettings["OTP:PinMinutesDuration"] = "4";
        //    ConfigurationManager.AppSettings["Notifications:OTP"] = "1";

        //    SolvenciaData solvData = SolvenciaData.Create("MEAJ720808J25", "email", "Nombre completo", "5540681788");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetClientDatosSolvencia(It.IsAny<string>(), It.IsAny<int>())).Returns(solvData);
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("000000");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.SetPinUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("P@SsW0rD");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("sR|mItDT|KTNiUwiLlfBk9=Vn5cE2H91Q34ri%u=iKMHs~f60Uld*rd9Lugk");
        //    _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("Ajbd7sozZcwz/FR0+jMUNw==");
        //    var otpData = _clientProductsAppBL.GenerateNewOTP("MEAJ720808J25", out result);
        //    Assert.IsTrue(result.Successful && otpData.NewOTP.Length > 0);
        //}

        [TestMethod, TestCategory("ParamError")]
        [DataRow("", "123456")]
        [DataRow("MEAJ720808J25", "")]
        public void GetOTPValidationParamError(string userdId, string pin)
        {
            var output1 = false;
            var output2 = false;
            ConfigurationManager.AppSettings["PortalId"] = "2";
            Assert.ThrowsException<ArgumentException>(() => _clientProductsAppBL.GetOTPValidation(userdId, pin, out output1, out output2));
        }

        [TestMethod, TestCategory("ProcessGeneralException")]
        public void GetOTPValidationGeneralError()
        {
            var output1 = false;
            var output2 = false;
            ConfigurationManager.AppSettings["PortalId"] = "2";
            _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("123456");
            var result = _clientProductsAppBL.GetOTPValidation("userid", "123456", out output1, out output2);

            Assert.IsFalse(result.Successful);
        }

        [TestMethod, TestCategory("ProcessNotSuccessful")]
        public void GetOTPValidationPINTimeout()
        {
            var output1 = false;
            var output2 = false;
            ConfigurationManager.AppSettings["PortalId"] = "2";
            _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("000000");
            var result = _clientProductsAppBL.GetOTPValidation("userid", "123456", out output1, out output2);

            Assert.IsTrue(result.Successful && result.SystemMessages[0].Message.Equals("El PIN ha caducado."));
        }

        [TestMethod, TestCategory("ProcessNotSuccessful")]
        public void GetOTPValidationPINInvalid()
        {
            var output1 = false;
            var output2 = false;
            ConfigurationManager.AppSettings["PortalId"] = "2";
            _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("SmuwVXFibs/JC9wg2LYQGw==");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("P@SsW0rD");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("sR|mItDT|KTNiUwiLlfBk9=Vn5cE2H91Q34ri%u=iKMHs~f60Uld*rd9Lugk");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("Ajbd7sozZcwz/FR0+jMUNw==");
            var result = _clientProductsAppBL.GetOTPValidation("userid", "654321", out output1, out output2);

            Assert.IsTrue(result.Successful && result.SystemMessages[0].Message.Equals("El PIN es inválido."));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetOTPValidationSuccess()
        {
            var output1 = false;
            var output2 = false;
            ConfigurationManager.AppSettings["PortalId"] = "2";
            _repositorySecurityGlobalAppDa.Setup(c => c.GetActivePIN(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("SmuwVXFibs/JC9wg2LYQGw==");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("P@SsW0rD");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("sR|mItDT|KTNiUwiLlfBk9=Vn5cE2H91Q34ri%u=iKMHs~f60Uld*rd9Lugk");
            _repositorySecurityGlobalAppDa.Setup(c => c.GetParameterValue(It.IsAny<string>(), It.IsAny<int>())).Returns("Ajbd7sozZcwz/FR0+jMUNw==");
            var result = _clientProductsAppBL.GetOTPValidation("userid", "123456", out output1, out output2);

            Assert.IsTrue(result.Successful && result.SystemMessages.Count == 0);
        }
    }
}
