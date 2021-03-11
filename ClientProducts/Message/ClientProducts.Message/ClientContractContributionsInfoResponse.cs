using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.Contributions;
using Common.Domain;

namespace ClientProducts.Message
{
    public class ClientContractContributionsInfoResponse
    {
        public OperationResult Result { get; set; }
        public ContractContributionsInfo ContractContributionsInfo { get; set; }

    }
}
