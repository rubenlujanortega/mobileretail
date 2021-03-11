using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class Domiciliation : ValueObject<Domiciliation>
    {
        private Bank _bank;
        private readonly string _accountNumber;
        private readonly int _payDay;
        private readonly decimal _debitAmount;
        private readonly bool _isUnique;

        private Domiciliation(string accountNumber, int payDay, decimal amount)
        {
            _accountNumber = accountNumber;
            _payDay = payDay;
            _debitAmount = amount;
            _isUnique = true;
        }

        public Bank Bank => _bank;
        public string AccountNumber => _accountNumber;
        public int PayDay => _payDay;
        public decimal DebitAmount => _debitAmount;
        public bool IsUnique => _isUnique;

        public static Domiciliation Create(string accountNumber, int payDay, decimal amount)
        {
            return new Domiciliation(accountNumber, payDay, amount);
        }

        public Domiciliation FillBank(Bank bank)
        {
            this._bank = bank;
            return this;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                Bank,
                AccountNumber,
                PayDay,
                DebitAmount,
                IsUnique
            };
        }
    }
}
