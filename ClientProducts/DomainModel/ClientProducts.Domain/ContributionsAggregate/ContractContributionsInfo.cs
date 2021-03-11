using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.Contributions
{
    public class ContractContributionsInfo : Entity<string>
    {
        private readonly String _contractId;
        private readonly decimal _commitedAmount;
        private readonly decimal _minumumAmountAllowed;
        private readonly decimal _maximumAmountAllowed;
        private readonly decimal _percentajeCommision;
        private readonly bool _advancecontributionsAllowed;
        private readonly decimal _amountDue;
        private readonly bool _coverContributions;
        private readonly bool _thereDirectDomiciliation;
        private readonly string _lastDomiciliationDate;
        private readonly List<ContractContributionSubaccount> _subaccounts;
        private readonly List<ContractBankAccount> _bankAccounts;

        private ContractContributionsInfo(string contractId, decimal committedAmount, decimal minumumAmount, decimal amountDue, List<ContractContributionSubaccount> subaccounts, List<ContractBankAccount> bankAccounts)
        {
            Id = contractId;
            _amountDue = amountDue;
            _commitedAmount = committedAmount;
            _minumumAmountAllowed = minumumAmount;
            _subaccounts = subaccounts;
            _bankAccounts = bankAccounts;
        }

        public decimal CommittedAmount => _commitedAmount;
        public decimal MinumumAmountAllowed => _minumumAmountAllowed;
        public decimal MaximunAmountAllowed => _maximumAmountAllowed;
        public decimal CommissionPercentaje => _percentajeCommision;
        public bool AdvancecontributionsAllowed => _advancecontributionsAllowed;
        public decimal AmountContributionsDue => _amountDue;
        public bool CoverContributions => _coverContributions;
        public bool IsTheDirectDomiciliation => _thereDirectDomiciliation;
        public string LastDomiciliationDate => _lastDomiciliationDate;
        public List<ContractContributionSubaccount> Subaccounts => _subaccounts;
        public List<ContractBankAccount> BankAccounts => _bankAccounts;

        public static ContractContributionsInfo Create(string contractId, decimal committedAmount, decimal minumumAmount, decimal amountDue, List<ContractContributionSubaccount> subaccounts, List<ContractBankAccount> bankAccounts)
        {
            return new ContractContributionsInfo(contractId, committedAmount, minumumAmount, amountDue, subaccounts, bankAccounts);
        }
    }
}
