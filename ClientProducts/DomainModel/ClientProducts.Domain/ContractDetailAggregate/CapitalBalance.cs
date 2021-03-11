using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class CapitalBalance : ValueObject<CapitalBalance>
    {
        private readonly decimal _currentCapitalBalance;
        private decimal _totalContributions;
        private decimal _totalWithdrawals;
        private decimal _yieldAmount;
        private decimal _currentLifeInsuranceAmount;

        public CapitalBalance(decimal totalBalance)
        {
            _currentCapitalBalance = totalBalance - _yieldAmount;
        }

        public decimal CurrentCapitalBalance => _currentCapitalBalance;
        public decimal TotalContributions => _totalContributions;
        public decimal TotalWithdrawals => _totalWithdrawals;
        public decimal YieldAmount => _yieldAmount;
        public decimal CurrentLifeInsuranceAmount => _currentLifeInsuranceAmount;

        public CapitalBalance FillData(decimal totalContributions, decimal currentLifeInsuranceAmount, decimal totalWithdrawals, decimal charges)
        {
            this._currentLifeInsuranceAmount = currentLifeInsuranceAmount;
            this._totalWithdrawals = totalWithdrawals;
            this._totalContributions = totalContributions + totalWithdrawals;
            this._yieldAmount = this._currentCapitalBalance - (this._totalContributions - charges - this._totalWithdrawals);
            return this;
        }


        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                CurrentCapitalBalance,
                TotalContributions,
                TotalWithdrawals,
                YieldAmount,
                CurrentLifeInsuranceAmount
            };
        }
    }
}
