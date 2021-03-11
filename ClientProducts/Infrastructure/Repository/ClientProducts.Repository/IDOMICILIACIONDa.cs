using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Repository
{
    public interface IDOMICILIACIONDa
    {
        Domiciliation GetContractDirectDebitBasicInfo(string contractId);
    }
}
