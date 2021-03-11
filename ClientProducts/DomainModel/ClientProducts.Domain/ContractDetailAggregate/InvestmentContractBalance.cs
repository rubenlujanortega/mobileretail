using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class InvestmentContractBalance : Entity<string>
    {
        List<InvestmentFundValue> _investmentFundsBalance;
        List<InvestmentFundValue> _cashBalance;
        List<InvestmentFundAllocation> _investmentFundAllocation;

        private InvestmentContractBalance(string contractId)
        {
            Id = contractId;
        }

        public static InvestmentContractBalance Create(string contractId)
        {
            if (string.IsNullOrEmpty(contractId)) { throw new ArgumentException("contractId no puede ser nulo o vacío."); }

            return new InvestmentContractBalance(contractId);
        }
        public InvestmentContractBalance FillLists(List<InvestmentFundValue> fundsBalance, List<InvestmentFundValue> cashBalance, List<InvestmentFundAllocation> source)
        {
            this._investmentFundsBalance = fundsBalance;
            this._cashBalance = cashBalance;
            this._investmentFundAllocation = source;
            
            return this;
        }

        public List<InvestmentFundValue> FundsBalance => _investmentFundsBalance;
        public List<InvestmentFundValue> CashBalance => _cashBalance;
        public List<InvestmentFundAllocation> FundAllocation => _investmentFundAllocation;

    }
}
