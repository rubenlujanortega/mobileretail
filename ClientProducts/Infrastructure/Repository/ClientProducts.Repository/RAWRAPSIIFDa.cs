using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Unity;
using SqlClient.Helper;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Domain.Contributions;
using ClientProducts.Mapping;
using NumericValues.Helper;

namespace ClientProducts.Repository
{
    public class RAWRAPSIIFDa : IRAWRAPSIIFDa
    {
        private readonly SqlConnection _dbConn;
        private readonly ISqlClientHelper _sqlClientHelper;
        public RAWRAPSIIFDa([Dependency("RAWRAPSIIF")] SqlConnection dbConn, ISqlClientHelper sqlClientHelper)
        {
            this._dbConn = dbConn;
            _sqlClientHelper = sqlClientHelper;
        }

        public Dictionary<string, string> GetClientContracts(string clientIdentifier)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GetContractsByRFC", _dbConn);
            cmd.AddParameterWithValue("@RFC", clientIdentifier);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);
            Dictionary<string, string> dictContracts = new Dictionary<string, string>();

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    dictContracts.Add(row[0].ToString(), row[1].ToString());
                }
            }

            return dictContracts;
        }

        public double GetContractSummaryBalance(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_RS_GetContratoPortafolio_Optimizado", _dbConn);
            cmd.AddParameterWithValue("@ContratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);
            cmd.CommandText = "usp_SOC_SIIF_GT_GetContratoPortafolio";
            DataSet dsSK = _sqlClientHelper.ExecuteDataSet(cmd);
            ds.Merge(dsSK);

            double summaryBalance = 0D;
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    summaryBalance += Convert.ToDouble(row["Valor_Fondo"]);
                }
            }

            return summaryBalance;
        }

        public Contract GetContractDetailInfo(string contractId, ref ContractSOCData contractSOC, out SavingInformation savingInformation)
        {

            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GT_GetContrato", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            cmd.AddParameterWithValue("@pbitContractOperative", false);
            DataSet dsContract = _sqlClientHelper.ExecuteDataSet(cmd);

            cmd.CommandText = "usp_SOC_SIIF_RS_GetContrato";
            DataSet dsRS = _sqlClientHelper.ExecuteDataSet(cmd);

            dsContract.Merge(dsRS);
            dsContract.Tables[0].TableName = "Parent";

            DataTable dtContract = dsContract.Tables[0].Copy();

            DataSet dsFinal = new DataSet();
            dsFinal.Tables.Add(dtContract);
            dsFinal.AcceptChanges();

            var contractDetailInfo = ContractMapp.MappingContractDetailInfo(dsFinal, ref contractSOC);
            savingInformation = SavingInfoMapp.SavingInfoMapping(contractSOC);
            return contractDetailInfo;
        }

        public List<InvestmentContractBalance> GetContractBalances(string contractId, List<string> subContracts)
        {
            List<InvestmentContractBalance> investmentContractBalance = new List<InvestmentContractBalance>();
            DataSet dsShares = new DataSet();
            var dtPortfolio = GetContractBalanceDetail(contractId);
            dsShares.Tables.Add(dtPortfolio);

            investmentContractBalance.Add(InvestmentContractBalanceMapp.InvestmentContractMapping(dsShares, dtPortfolio));

            if(subContracts.Count>0)
            {
                foreach(string contract in subContracts)
                {
                    investmentContractBalance.Add(GetContractBalance(contract));
                }
            }

            return investmentContractBalance;
        }

        public InvestmentContractBalance GetContractBalance(string contractId)
        {
            DataSet dsShares = new DataSet();
            var dtPortfolio = GetContractBalanceDetail(contractId);
            dsShares.Tables.Add(dtPortfolio);

            return InvestmentContractBalanceMapp.InvestmentContractMapping(dsShares, dtPortfolio);
        }

        public CapitalBalance GetContractBalancesHistory(string contractId, DateTime[] queryDates, bool getLastBalance, bool IsBasedProcessPeriod, int platform, List<string> subContracts)
        {
            List<DateTime> queryDatesTmp = new List<DateTime>();
         
            decimal totalBalance = 0;
            foreach (DateTime queryDate in queryDates)
            {
                DateTime queryDateTmp = queryDate.Date;
                if (queryDateTmp == DateTime.Now.Date)
                {
                    DataTable dtBalancePresent = GetContractPortfolioProduct(contractId, platform, subContracts);
                    foreach (DataRow dr in dtBalancePresent.Rows)
                    {
                        totalBalance += Convert.ToDecimal(Null.SetNull(dr["Valor_Fondo"], new Decimal()));
                    }
                }
                else
                {
                   
                    queryDatesTmp.Add(queryDateTmp);
                }
            }

            var ds = GetContractBalanceHistory(contractId, queryDates, subContracts, platform);
            foreach (DataRow dr in ds.Rows)
            {
                totalBalance += Convert.ToDecimal(Null.SetNull(dr["Valor_Fondo"], new Decimal()));
            }

            return CapitalBalanceMapp.CapitalBalanceMapping(totalBalance);
        }
        public decimal GetContractCharges(string contractId, List<string> subContracts)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GetContratoCargos_IDE_Up", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            var ds = _sqlClientHelper.ExecuteDataSet(cmd);
            decimal chargesAmount = 0M;

            if(ds.Tables.Count>0)
            {
                chargesAmount += Convert.ToDecimal(Null.SetNull(ds.Tables[0].Rows[0]["Importe"], new decimal()));
            }

            if (subContracts.Count> 0)
            {
                chargesAmount += GetChargesSubContract(subContracts);
            }

            return chargesAmount;
        }
        private decimal GetChargesSubContract(List<string> subContracts)
        {
            IDbCommand cmd;
            var chargesAmount = 0M;
            cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GetContratoCargos_IDE_Up", _dbConn);

            foreach (string contract in subContracts)
            {
                cmd.Parameters.Clear();
                cmd.AddParameterWithValue("@pstrContratoId", contract);
                var ds = _sqlClientHelper.ExecuteDataSet(cmd);
                if (ds.Tables.Count > 0)
                {
                    chargesAmount += Convert.ToDecimal(Null.SetNull(ds.Tables[0].Rows[0]["Importe"], new decimal()));
                }
            }

            return chargesAmount;
        }

        public SavingAdvance GetContractSavingAdvanceInfo(Contract contractInfo, ref ContractSOCData socData)
        {
            var startDate = new DateTime(1900, 1, 1);
            var endDate = DateTime.Now.Date;
            var dsCash = GetContractCashTransactions(contractInfo.Id, contractInfo.ContractBasicInfo.Plataform, startDate, endDate);
            var dsWithDrawals = GetContractSurrenders(contractInfo.Id, contractInfo.ContractBasicInfo.Plataform, startDate, endDate, true);
            var offspringGrouperContract = GetOffspringContract(contractInfo.Id, contractInfo.ContractBasicInfo.Plataform);
            var addtionalContributions = GetAdditionalContributions(offspringGrouperContract, contractInfo.ContractBasicInfo.Plataform);

            var currentNumContributions = 0;
            if (offspringGrouperContract != null)
            {
                currentNumContributions = GetCurrentNumContributions(offspringGrouperContract);
            }

            var onDemandShares = GetOnDemandCash(contractInfo.Id);

            socData.FillAdditionalContributions(addtionalContributions);

            return SavingAdvanceMapp.SavingAdvanceMapping(dsCash, dsWithDrawals, contractInfo, ref socData, onDemandShares, currentNumContributions);
        }

        public List<string> GetSubContracts(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_RS_GetSubcuentasContratos", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            var ds = _sqlClientHelper.ExecuteDataSet(cmd);

            List<string> subContractsList = new List<string>();
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    subContractsList.Add(row["cnt_id"].ToString());
                }
            }

            return subContractsList;
        }

        public List<ContractBankAccount> GetContractBankAccounts(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_RS_GetContratoCuentasBanco", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            var ds = _sqlClientHelper.ExecuteDataSet(cmd);
            cmd.CommandText = "usp_SOC_SIIF_GT_GetContratoCuentasBanco";
            var dsGT = _sqlClientHelper.ExecuteDataSet(cmd);
            ds.Merge(dsGT);

            return ContractBankAccountMapp.ContractBankAccountMapping(ds);
        }

        public Bank GetBankName(string contractId, int platform, Domiciliation domiciliation)
        {
            if (domiciliation.AccountNumber != string.Empty)
            {
                IDbCommand cmd;
                var storeName = platform == 0 ? "usp_SOC_SIIF_RS_GetContratoCuentasBanco" : "usp_SOC_SIIF_GT_GetContratoCuentasBanco";
                cmd = _sqlClientHelper.CreateCmdSP(storeName, _dbConn);
                cmd.AddParameterWithValue("@pstrContratoId", contractId);
                DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 )
                {
                    return BankMapp.BankMapping(ds.Tables[0].Rows[0]);
                }
                else
                {
                    return BankMapp.BankMappingEmpty();
                }
            }
            else
            {
                return BankMapp.BankMappingEmpty();
            }

        }

        public Domiciliation GetRecurringPayments(string contractId, int platform)
        {
            var storeName = platform == 0 ? "usp_SOC_SIIF_GetRecurringPaymentsBankAccountsRS" : "usp_SOC_SIIF_GetRecurringPaymentsBankAccountsGT";
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP(storeName, _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            cmd.AddParameterWithValue("@pstrFechaInicio", "19000101");
            cmd.AddParameterWithValue("@pstrFechaFin", "20991231");
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return DomiciliationMapp.DomiciliationMappingRecurring(ds);
            }
            else
            {
                return DomiciliationMapp.DomiciliationMappingEmpty();
            }
        }

        #region "Contributions"
        public List<ContractContributionSubaccount> GetContributionsSubaccounts(string planId, OffspringGrouperContract subaccountsContracts)
        {
            if (subaccountsContracts != null)
            {
                IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_GetTextosSubcuentasAportaciones", _dbConn);
                cmd.AddParameterWithValue("@productoId", planId);
                DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

                return ContractContributionSubaccountMapp.GetContractContributionsSubaccountsTextMapping(ds, subaccountsContracts);
            }
            else
            {
                return new List<ContractContributionSubaccount>();
            }
        }

        public decimal GetContributionsAmountDue(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_GetContratoAportacionesVencidas", _dbConn);
            cmd.AddParameterWithValue("@contratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            var amount = 0M;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                amount = Convert.ToDecimal(Null.SetNull(ds.Tables[0].Rows[0][0], 0M));
            }
            return amount;
        }

        public decimal GetContributionMinimumAmount(int typeId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_GetMontoAportacionMinima", _dbConn);
            cmd.AddParameterWithValue("@tipodomiciliacion", typeId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            var amount = 0M;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
            {
                amount = Convert.ToDecimal(Null.SetNull(ds.Tables[0].Rows[0][0], 0M)); 
            }
            return amount;
        }

        #endregion
        #region "Private Methods"

        #region "SavingAdvance Methods"
        private DataSet GetContractCashTransactions(string contractId, int productType, DateTime startDate, DateTime endDate)
        {
            DataSet dsCash = new DataSet();

            IDbCommand cmd;

            if (productType == 0)
            {
                cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_RS_GetContratoMovimientosEfectivo", _dbConn);
                cmd.AddParameterWithValue("@pstrContratoId", contractId);
                cmd.AddParameterWithValue("@pdteInicial", startDate);
                cmd.AddParameterWithValue("@pdteFinal", endDate);
                dsCash = _sqlClientHelper.ExecuteDataSet(cmd);

                cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_RSH_GetContratoMovimientosEfectivo", _dbConn);
                cmd.AddParameterWithValue("@pstrsiContratoId", contractId);
                cmd.AddParameterWithValue("@pdteInicial", startDate);
                cmd.AddParameterWithValue("@pdteFinal", endDate);
                DataSet dsRS = _sqlClientHelper.ExecuteDataSet(cmd);

                if (dsCash != null && dsCash.Tables.Count > 0 && dsCash.Tables[0].Rows.Count > 0)
                {
                    if (dsRS != null && dsRS.Tables.Count > 0 && dsRS.Tables[0].Rows.Count > 0)
                    {
                        dsCash.Merge(dsRS);
                    }
                }
                else if (dsRS != null && dsRS.Tables.Count > 0 && dsRS.Tables[0].Rows.Count > 0)
                {
                    dsCash = dsRS;
                }
            }
            else if (productType == 1)
            {
                cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GT_GetContratoMovimientosEfectivo", _dbConn);
                cmd.AddParameterWithValue("@pstrContratoId", contractId);
                cmd.AddParameterWithValue("@pdteInicial", startDate);
                cmd.AddParameterWithValue("@pdteFinal", endDate);
                dsCash = _sqlClientHelper.ExecuteDataSet(cmd);
            }

            return dsCash;
        }
        private decimal GetOnDemandCash(string contractId)
        {
            var onDemandFilter = System.Configuration.ConfigurationManager.AppSettings["AportacionesOnDemand"];
            var filters = onDemandFilter.Split('|');
            
            IDbCommand cmd;
            cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GETAportacionesOnDemand", _dbConn);
            cmd.AddParameterWithValue("@contratoId", contractId);
            cmd.AddParameterWithValue("@TipoMercado", filters[0]);
            cmd.AddParameterWithValue("@TipoOperacion", filters[1]);
            cmd.AddParameterWithValue("@TipoOrden", filters[2]);

            DataSet dsOnDemand = _sqlClientHelper.ExecuteDataSet(cmd);

            var importe = 0M;
            if (dsOnDemand.Tables.Count>0 && dsOnDemand.Tables[0].Rows.Count > 0)
            {
                importe = Convert.ToDecimal(Null.SetNull(dsOnDemand.Tables[0].Rows[0]["Importe"], new decimal())); 
            }

            return importe;
        }
        private DataSet GetContractSurrenders(string contractId, int productType, DateTime startDate, DateTime endDate, bool applied)
        {
            var storeName = productType == 0 ? "usp_SOC_SIIF_RS_GetContratoRetiros" : "usp_SOC_SIIF_GT_GetContratoRetiros";
            var flag = applied ? 1 : 0;

            IDbCommand cmd;
            cmd = _sqlClientHelper.CreateCmdSP(storeName, _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            cmd.AddParameterWithValue("@pbitAtendidas", flag);
            cmd.AddParameterWithValue("@pdteInicial", startDate);
            cmd.AddParameterWithValue("@pdteFinal", endDate);

            DataSet dsSurrender = _sqlClientHelper.ExecuteDataSet(cmd);
            return dsSurrender;
        }
        public OffspringGrouperContract GetOffspringContract(string contractId, int platform)
        {
            DataSet dsFinal = new DataSet();
            IDbCommand cmd;
            var storeName = platform == 0 ? "usp_SOC_SIIF_RS_GetContratoMaestroDescendencia" : "usp_SOC_SIIF_GT_GetContratoMaestroDescendencia";
            cmd = _sqlClientHelper.CreateCmdSP(storeName, _dbConn);
            cmd.AddParameterWithValue("@contractId", contractId);
            var ds = _sqlClientHelper.ExecuteDataSet(cmd);

            var dt = ds.Tables[0].Clone();
            var rows = ds.Tables[0].Select("producto_id='10'");

            foreach (DataRow item in rows)
            {
                dt.ImportRow(item);
            }
            dsFinal.Tables.Add(dt);

            return OffspringGrouperContractMapp.OffspringGrouperMapping(dsFinal);
        }
        private int GetCurrentNumContributions(OffspringGrouperContract contractInfo)
        {
            var currentNumContributions = 0;
            if (contractInfo != null)
            {
                IDbCommand cmd =_sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_RS_GetContratoAportaciones", _dbConn);
                cmd.AddParameterWithValue("@contractId", contractInfo.ContributionContractId);
                cmd.AddParameterWithValue("@StatusId", "10");
                var ds = _sqlClientHelper.ExecuteDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    currentNumContributions = Convert.ToInt32(Null.SetNull(ds.Tables[0].Rows[0]["Numero_Aportaciones"], 0));
                }
            }
            return currentNumContributions;
        }
        private decimal GetAdditionalContributions(OffspringGrouperContract contractInfo, int platform)
        {
            TransactionTypes filters = new TransactionTypes(20, 20, 640);
            var fechaInicio = new DateTime(1900, 01, 01);
            var fecha = DateTime.Now;
            var aportacionesAdicionales = 0M;
            if (contractInfo != null)
            {
                DataSet ds = GetTransactionOrdersContract(contractInfo.AdditionalContractId, filters, fechaInicio, fecha, platform);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        var importe = (String)Null.SetNull(item["Orden_Importe"].ToString(), "0");
                        importe = importe == "" ? "0" : (String)Null.SetNull(item["Orden_Importe"].ToString(), "0");
                        aportacionesAdicionales += decimal.Parse(importe);
                    }
                }

                var contractBalance = GetContractBalance(contractInfo.AdditionalContractId);
                var aditionalsContractBalanceAmount = 0M;
                aditionalsContractBalanceAmount = Convert.ToDecimal(contractBalance.FundsBalance.Sum(c => c.MarketSale));
                aditionalsContractBalanceAmount += Convert.ToDecimal(contractBalance.CashBalance.Sum(c => c.MarketSale));

                aportacionesAdicionales = aportacionesAdicionales != aditionalsContractBalanceAmount ? aditionalsContractBalanceAmount : aportacionesAdicionales;
            }
            return aportacionesAdicionales;
        }
        private DataSet GetTransactionOrdersContract(string contractId, TransactionTypes filters, DateTime startDate, DateTime endDate, int platform)
        {
            DataSet dsFinal = new DataSet();
            DataTable dtFinal = null;
            var firstStoredName = platform == 0 ? "usp_SOC_SIIF_RS_GetContratoOrdenes" : "usp_SOC_SIIF_GT_GetContratoOrdenes";
            var SecondStoredName = platform == 0 ? "usp_SOC_SIIF_RSH_GetContratoOrdenes" : "usp_SOC_SIIF_GTH_GetContratoOrdenes";
            IDbCommand cmd;

            cmd = _sqlClientHelper.CreateCmdSP(firstStoredName, _dbConn);
            cmd.AddParameterWithValue("@strContractId", contractId);
            cmd.AddParameterWithValue("@dteInitialDate", startDate);
            cmd.AddParameterWithValue("@dteFinalDate", endDate);
            var ds = _sqlClientHelper.ExecuteDataSet(cmd);

            cmd = _sqlClientHelper.CreateCmdSP(SecondStoredName, _dbConn);
            cmd.AddParameterWithValue("@strContractId", contractId);
            cmd.AddParameterWithValue("@dteInitialDate", startDate);
            cmd.AddParameterWithValue("@dteFinalDate", endDate);
            var dsH = _sqlClientHelper.ExecuteDataSet(cmd);

            if (ds.Tables.Count > 0 && dsH.Tables.Count >0 )
            {
                var dtR = ds.Tables[0];
                var dtH = dsH.Tables[0];
                dtR.Merge(dtH);
                dtFinal = dtR.Copy();
                var dt = dtR.Clone();

                foreach (DataRow item in dtFinal.Rows)
                {
                    var row = dt.NewRow();
                    if (item["Tipo_Mercado_Id"].ToString() == filters.TipoMercado.ToString() && item["Tipo_Operacion_Id"].ToString() == filters.TipoOperacion.ToString() && item["Tipo_Orden_Id"].ToString() == filters.TipoOrden.ToString())
                    {
                        row["Orden_Id"] = (String)Null.SetNull(item["Orden_Id"].ToString(), string.Empty).ToString();
                        row["Tipo_Mercado_Id"] = int.Parse(Null.SetNull(item["Tipo_Mercado_Id"].ToString(), 0).ToString());
                        row["Tipo_Operacion_Id"] = int.Parse(Null.SetNull(item["Tipo_Operacion_Id"].ToString(), 0).ToString());
                        row["Tipo_Orden_Id"] = int.Parse(Null.SetNull(item["Tipo_Orden_Id"].ToString(), 0).ToString());
                        row["Contrato_Id"] = (String)Null.SetNull(item["Contrato_Id"].ToString(), string.Empty);
                        row["Emision_Id"] = (String)Null.SetNull(item["Emision_Id"].ToString(), string.Empty);
                        row["Orden_Titulos"] = (String)Null.SetNull(item["Orden_Titulos"].ToString(), string.Empty);
                        row["Orden_Precio"] = Convert.ToDecimal(Null.SetNull(item["Orden_Precio"].ToString(), 0));
                        row["Orden_Importe"] = Convert.ToDecimal(Null.SetNull(item["Orden_Importe"].ToString(), 0));
                        row["Orden_Fecha_Operacion"] = Convert.ToDateTime(Null.SetNull(item["Orden_Fecha_Operacion"].ToString(), new DateTime()));
                        row["Orden_Fecha_Liquidacion"] = Convert.ToDateTime(Null.SetNull(item["Orden_Fecha_Liquidacion"].ToString(), new DateTime()));
                        row["Orden_Fecha_Proceso"] = Convert.ToDateTime(Null.SetNull(item["Orden_Fecha_Proceso"].ToString(), new DateTime()));
                        row["Orden_Comision"] = (String)Null.SetNull(item["Orden_Comision"].ToString(), string.Empty);
                        row["Orden_Iva"] = (String)Null.SetNull(item["Orden_Iva"].ToString(), string.Empty);
                        row["Orden_ISR"] = (String)Null.SetNull(item["Orden_ISR"].ToString(), string.Empty);
                        row["Orden_TitImp_Valor"] = (String)Null.SetNull(item["Orden_TitImp_Valor"].ToString(), string.Empty);
                        row["Banco_Id"] = (String)Null.SetNull(item["Banco_Id"].ToString(), string.Empty);
                        row["Banco_Descripcion"] = (String)Null.SetNull(item["Banco_Descripcion"].ToString(), string.Empty);
                        row["Cuenta_Id_Cliente"] = (String)Null.SetNull(item["Cuenta_Id_Cliente"].ToString(), string.Empty);

                        dt.Rows.Add(row);
                    }
                }
                dsFinal.Tables.Add(dt);
            }
            return dsFinal;
        }
        #endregion

        #region "BalanceHistory Methods"
        private DataTable GetContractBalanceHistory(string contractId, DateTime[] queryDates, List<string> subContracts, int platform)
        {
            DataTable dtMain = new DataTable();
            var year = string.Empty;
            var month = string.Empty;
            var day = string.Empty;
            string strQueryDates = string.Empty;

            foreach (DateTime queryDate in queryDates)
            {
                year = queryDate.Year.ToString();
                month = queryDate.Month.ToString().PadLeft(2, '0');
                day = queryDate.Day.ToString().PadLeft(2,'0');
                strQueryDates += year + month + day + "|";
            }

            var dtBalanceHist = GetBalanceHistory(contractId, strQueryDates, true, platform);
            if(dtBalanceHist != null)
            {
                dtMain.Merge(dtBalanceHist);
            }

            foreach (string contract in subContracts)
            {
                dtBalanceHist = GetBalanceHistory(contractId, strQueryDates, true, platform);
                dtMain.Merge(dtBalanceHist);
            }
            return dtMain;
        }

        private DataTable GetBalanceHistory(string contractId, string queryDates, bool getLastBalance, int platform)
        {
            var firstSPName = platform == 1 ? "usp_SOC_SIIF_RS_GetContratoBalancesHistorico" : "usp_SOC_SIIF_GT_GetContratoBalancesHistorico";
            var secondSPName = platform == 1 ? "usp_SOC_SIIF_RSH_GetContratoBalancesHistorico" : "usp_SOC_SIIF_GTH_GetContratoBalancesHistorico";

            IDbCommand cmd;
            cmd = _sqlClientHelper.CreateCmdSP(firstSPName, _dbConn);
            cmd.AddParameterWithValue("@strContractId", contractId);
            cmd.AddParameterWithValue("@strQueryDates", queryDates);
            cmd.AddParameterWithValue("@bitGetLastBalance", getLastBalance);
            var ds = _sqlClientHelper.ExecuteDataSet(cmd);
            cmd.CommandText = secondSPName;
            ds.Merge(_sqlClientHelper.ExecuteDataSet(cmd));

            if(ds.Tables.Count>0)
            {
                return ds.Tables[0].Copy();
            }
            else
            {
                return null;
            }
        }
        private DataTable GetContractPortfolioProduct(string contractId, int platform, List<string> subContracts)
        {
            DataTable dtMain = new DataTable();
            var dtPortfolio = GetContractPortafolio(contractId, platform);
            dtMain.Merge(dtPortfolio);
            foreach (string contract in subContracts)
            {
                dtPortfolio = GetContractPortafolio(contract, platform);
                dtMain.Merge(dtPortfolio);
            }
            dtMain.TableName = "Portafolio";
            return dtMain;
        }
        #endregion
        #endregion

        private DataTable GetContractBalanceDetail(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_SIIF_GT_GetContratoBalanceDetalle", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            DataSet dsBalance = _sqlClientHelper.ExecuteDataSet(cmd);

            cmd.CommandText = "usp_SOC_SIIF_RS_GetContratoBalanceDetalle";
            DataSet dsRS = _sqlClientHelper.ExecuteDataSet(cmd);

            dsBalance.Merge(dsRS);
            dsBalance.Tables[0].TableName = "Portafolio";
            return dsBalance.Tables[0].Copy();
        }

        public DataTable GetContractPortafolio(string contractId, int platform)
        {
            var storedProcName = platform == 0 ? "usp_SOC_SIIF_RS_GetContratoPortafolio" : "usp_SOC_SIIF_GT_GetContratoPortafolio";
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP(storedProcName, _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            DataSet dsPortafolio = _sqlClientHelper.ExecuteDataSet(cmd);
            DataTable dtPortolio = dsPortafolio.Tables[0].Copy();

            return dtPortolio;
        }
    }
}
