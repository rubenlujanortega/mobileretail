using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class InvestmentFundAllocation : ValueObject<InvestmentFundAllocation>
    {
        private readonly InvestmentFundValue _investmentFund;
        private readonly decimal _percentage;

        public InvestmentFundAllocation(InvestmentFundValue investmentFund, decimal percentage)
        {
            if(investmentFund == null) { throw new ArgumentException("investmentFund no puede ser nulo."); }
            _investmentFund = investmentFund;
            _percentage = percentage;
        }

        public InvestmentFundValue InvestmentFund => _investmentFund;
        public decimal Percentage => _percentage;

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                InvestmentFund,
                Percentage
            };
        }
    }
}
