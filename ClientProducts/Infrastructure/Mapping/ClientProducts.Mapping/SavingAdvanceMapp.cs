using System;
using System.Data;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Mapping
{
    public static class SavingAdvanceMapp
    {
        public static SavingAdvance SavingAdvanceMapping(DataSet dsCash, DataSet dsWithdrawals, Contract contractInfo, ref ContractSOCData socDATA, decimal onDemandShares, int currentNumContributions)
        {
            var afiliationDate = Convert.ToDateTime(contractInfo.ContractBasicInfo.AfiliationDate);
            var endDate = DateTime.Now.Date;
            var initialDate = DateTime.Now.Date;
            var currentMonths = 0;
            var tmpAux = 0M;
            TransactionsSummaryInfo transacSumInfo = null;
            if (dsCash.Tables.Count > 0)
            {
                initialDate = GetContractBeginningDate(dsCash.Tables[0], afiliationDate);
            }
            if (dsWithdrawals.Tables.Count > 0)
            {
                transacSumInfo = GetContributionsAndWithdrawals(dsCash.Tables[0], endDate);
            }
            if (socDATA.CommitedAmount > 0)
            {
                tmpAux = ((transacSumInfo.TotalContributionsAmount - transacSumInfo.TotalWithdrawalsAmount) / socDATA.CommitedAmount);
                currentMonths = ((DateTime.Now.Year - initialDate.Year) * 12) + (DateTime.Now.Month - initialDate.Month);
            }
            currentNumContributions = currentNumContributions == 0 ? Convert.ToInt32(tmpAux) : currentNumContributions;
            var contributionAmount = (transacSumInfo.TotalContributionsAmount - transacSumInfo.TotalWithdrawalsAmount);
            socDATA
                .FillTotalWithDrawals(transacSumInfo.TotalWithdrawalsAmount)
                .FillTotalContributions(contributionAmount + onDemandShares);
            return new SavingAdvance(socDATA, currentMonths, currentNumContributions);
        }

        private static DateTime GetContractBeginningDate(DataTable dtTransactions, DateTime afiliationDate)
        {
            var dr = dtTransactions.Select("Transaccion_Signo = '+'", "Transaccion_Fecha");
            var firstContributionDate = dr.Length > 0 ? Convert.ToDateTime(dr[0]["Transaccion_Fecha"]) : DateTime.MinValue;
            var openingDate = afiliationDate;
            var beginningDate = (firstContributionDate.Ticks >= openingDate.Ticks) ? firstContributionDate : openingDate;
            if (beginningDate.Ticks == new DateTime(1900, 1, 1).Ticks)
            {
                beginningDate = DateTime.Now;
            }

            return beginningDate;
        }

        private static TransactionsSummaryInfo GetContributionsAndWithdrawals(DataTable dtTransactions, DateTime finalDate)
        {
            Decimal totalContributions = 0;
            Decimal totalWithdrawals = 0;

            int contributionCounter = 0;
            int withdrawalCounter = 0;

            DateTime tmpDate = new DateTime(1900, 1, 1);

            if (dtTransactions.Rows.Count > 0)
            {
                foreach (DataRow drCash in dtTransactions.Rows)
                {
                    if (drCash["Transaccion_Signo"].ToString() == "+")
                    {
                        totalContributions += Convert.ToDecimal(drCash["Orden_Importe"]);

                        if (tmpDate != Convert.ToDateTime(drCash["Transaccion_Fecha"]))
                        {
                            contributionCounter++;
                        }
                    }
                    else if (drCash["Transaccion_Signo"].ToString() == "-")
                    {
                        totalWithdrawals += Convert.ToDecimal(drCash["Orden_Importe"]);

                        if (tmpDate != Convert.ToDateTime(drCash["Transaccion_Fecha"]))
                        {
                            withdrawalCounter++;
                        }
                            
                    }
                    tmpDate = Convert.ToDateTime(drCash["Transaccion_Fecha"]);
                }
            }
            var transactionsSummaryInfo = new TransactionsSummaryInfo(totalContributions, totalWithdrawals, contributionCounter, withdrawalCounter);
            return transactionsSummaryInfo;
        }
    }
}
