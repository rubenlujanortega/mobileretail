using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.Contributions;
using Common.Domain;

namespace ClientProducts.Message
{
    public class ContractBankAccountsResponse
    {
        public OperationResult Result { get; set; }

        public List<ContractBankAccount> ContractBankAccounts { get; set; }

        public ContractBankAccountsResponse()
        {
            Result = new OperationResult();
        }
    }
}
