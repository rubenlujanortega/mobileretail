using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Domain.Contributions;

namespace ClientProducts.Repository
{
    public interface IRAWRAPSIIFDa
    {
        Dictionary<string, string> GetClientContracts(string clientIdentifier);
        double GetContractSummaryBalance(string contractId);
        Contract GetContractDetailInfo(string contractId, ref ContractSOCData contractSOC, out SavingInformation savingInformation);
        List<InvestmentContractBalance> GetContractBalances(string contractId, List<string> subContracts);
        InvestmentContractBalance GetContractBalance(string contractId);
        SavingAdvance GetContractSavingAdvanceInfo(Contract contractInfo, ref ContractSOCData socData);
        CapitalBalance GetContractBalancesHistory(string contractId, DateTime[] queryDates, bool getLastBalance, bool IsBasedProcessPeriod, int platform, List<string> subContracts);
        decimal GetContractCharges(string contractId, List<string> subContracts);
        List<string> GetSubContracts(string contractId);
        OffspringGrouperContract GetOffspringContract(string contractId, int platform);
        List<ContractBankAccount> GetContractBankAccounts(string contractId);
        List<ContractContributionSubaccount> GetContributionsSubaccounts(string planId, OffspringGrouperContract subaccountsContracts);
        decimal GetContributionMinimumAmount(int typeId);
        decimal GetContributionsAmountDue(string contractId);
        Bank GetBankName(string contractId, int platform, Domiciliation domiciliation);
        Domiciliation GetRecurringPayments(string contractId, int platform);
    }
}
