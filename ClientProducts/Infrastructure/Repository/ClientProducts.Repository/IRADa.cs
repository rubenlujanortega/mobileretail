using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ProductAggregate;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Domain.Contributions;

namespace ClientProducts.Repository
{
    public interface IRADa
    {
        Plan GetPlanInfo(string contractId);
        Product GetContractProductInfo(string contractId);
        ContractSOCData GetContractSOCInfo(string contractId, string planComercialName);
        List<ContractContributionsSubaccountText> GetContributionsSubaccountsTexts(string planId);

    }
}
