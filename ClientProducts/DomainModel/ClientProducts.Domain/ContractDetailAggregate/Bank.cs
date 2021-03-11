using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class Bank  : Entity<string>
    {
        private string _bankName;

        private Bank(string bankId, string bankName)
        {
            Id = bankId;
            _bankName = bankName;
        }

        public string BankName => _bankName;
        public static Bank Create(string bankId, string bankName)
        {
           if (bankId == null) { throw new ArgumentException("bankId no puede ser nulo ni vacío"); }
           if (bankName == null) { throw new ArgumentException("bankName no puede ser nulo ni vacío"); }

            return new Bank(bankId, bankName);
        }
    }
}
