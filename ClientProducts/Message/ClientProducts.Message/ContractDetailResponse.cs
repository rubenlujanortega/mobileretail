using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;
using Common.Domain;

namespace ClientProducts.Message
{
    public class ContractDetailResponse
    {
        public Contract ClientContractDetail { get; set; }
        public OperationResult Result { get; set; }

        public ContractDetailResponse()
        {
            Result = new OperationResult();
        }
    }
}
