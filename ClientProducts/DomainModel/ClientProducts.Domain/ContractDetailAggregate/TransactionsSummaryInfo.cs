using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class TransactionsSummaryInfo : ValueObject<TransactionsSummaryInfo>
    {

        private readonly decimal _totalContributionsAmount;
        private readonly decimal _totalWithdrawalsAmount;
        private readonly decimal _netContributionsAmount;
        private readonly int _totalNumOfContributions;
        private readonly int _totalNumOfWithdrawals;
        
        public TransactionsSummaryInfo(decimal totalContributionsAmount, decimal totalWithdrawalsAmount, int totalNumOfContributions, int totalNumOfWithdrawals)
        {
            _totalContributionsAmount = Math.Round(totalContributionsAmount, 2);
            _totalWithdrawalsAmount = Math.Round(totalWithdrawalsAmount, 2);
            _totalNumOfContributions = totalNumOfContributions;
            _totalNumOfWithdrawals = totalNumOfWithdrawals;
            _netContributionsAmount = _totalContributionsAmount - _totalWithdrawalsAmount;
        }

        public decimal TotalContributionsAmount => _totalContributionsAmount;
        public decimal TotalWithdrawalsAmount => _totalWithdrawalsAmount;
        public decimal NetContributionsAmount => _netContributionsAmount;
        public decimal TotalNumOfContributions => _totalNumOfContributions;
        public decimal TotalNumOfWithdrawals => _totalNumOfWithdrawals;

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                TotalContributionsAmount,
                TotalWithdrawalsAmount,
                NetContributionsAmount,
                TotalNumOfContributions,
                TotalNumOfWithdrawals
            };
        }
    }
}
