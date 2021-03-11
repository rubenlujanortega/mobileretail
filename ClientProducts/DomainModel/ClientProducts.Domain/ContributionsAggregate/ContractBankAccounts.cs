using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.Contributions
{
    public class ContractBankAccount : Entity<string>
    {
        private readonly int _accountId;
        private readonly int _bankId;
        private readonly int _bankAccountTypeId;
        private readonly string _accountNumber;
        private readonly string _accountBankName;

        private ContractBankAccount(string contractId, int bankId, int accountId, string accountNumber, string accountBankName, int bankAccountTypeId)
        {
            Id = contractId;
            _bankId = bankId;
            _bankAccountTypeId = bankAccountTypeId;
            _accountId = accountId;
            _accountNumber = accountNumber;
            _accountBankName = accountBankName;
        }

        public int BankId => _bankId;
        public int BankAccountTypeId => _bankAccountTypeId;
        public int AccountId => _accountId;
        public string AccountNumber => _accountNumber;
        public string AccountBankName => _accountBankName;

        public static ContractBankAccount Create(string contractId, int bankId, int accountId, string accountNumber, string accountBankName, int bankAccountTypeId)
        {
            if(string.IsNullOrEmpty(contractId)) { throw new ArgumentException("contractId no puede ser nulo o vacío."); }

            return new ContractBankAccount(contractId, bankId, accountId, accountNumber, accountBankName, bankAccountTypeId);
        }

    }
}
