using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class ContractSOCData : ValueObject<ContractSOCData>
    {
        private string _reasonSaving;
        private decimal _savingGoal;
        private int _savingTerm;
        private decimal _commitedAmount;
        private decimal _currentLifeInsuranceAmount;
        private decimal _totalWithDrawals;
        private decimal _totalContributions;
        private decimal _additionalContributions;
        private string _terminationDate;
        private readonly string _productName;

        private ContractSOCData(string productName)
        {
            _productName = productName;
        }

        public static ContractSOCData Create(string productName)
        {
            if(string.IsNullOrEmpty(productName)) { throw new ArgumentException("productName no puede ser nulo o vacío."); }

            return new ContractSOCData(productName);
        }
        public ContractSOCData FillSavingInfo(decimal savingGoal, int savingTerm, decimal commitedAmount, decimal currentLifeInsuranceAmount, string terminationDate)
        {
            if (string.IsNullOrEmpty(terminationDate)) { throw new ArgumentException("terminationDate no puede ser nulo o vacío."); }

            this._savingGoal = savingGoal;
            this._savingTerm = savingTerm;
            this._commitedAmount = commitedAmount;
            this._currentLifeInsuranceAmount = currentLifeInsuranceAmount;
            this._terminationDate = terminationDate;
            return this;
        }

        public string ProductName => _productName;
        public string SavingReason => _reasonSaving;
        public decimal SavingGoal => _savingGoal;
        public int SavingTerm => _savingTerm;
        public decimal CommitedAmount => _commitedAmount;
        public decimal CurrentLifeInsuranceAmount => _currentLifeInsuranceAmount;
        public string TerminationDate => _terminationDate;
        public decimal TotalWithDrawals => _totalWithDrawals;
        public decimal AdditionalContributions => _additionalContributions;
        public decimal TotalContributions => _totalContributions;

        public ContractSOCData FillSavingReason(string reasonSaving)
        {
            if(reasonSaving == null) { throw new ArgumentException("reasonSaving no puede ser nulo."); }

            this._reasonSaving = reasonSaving;
            return this;
        }

        public ContractSOCData FillTotalWithDrawals(decimal totalWithDrawals)
        {
            this._totalWithDrawals = totalWithDrawals;
            return this;
        }
        public ContractSOCData FillTotalContributions(decimal totalContributions)
        {
            this._totalContributions = totalContributions;
            return this;
        }
        public ContractSOCData FillAdditionalContributions(decimal additionalContributions)
        {
            this._additionalContributions = additionalContributions;
            return this;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                ProductName,
                SavingReason,
                SavingGoal
            };
            
        }
    }
}
