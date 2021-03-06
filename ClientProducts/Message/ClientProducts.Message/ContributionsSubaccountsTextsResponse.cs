using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.Contributions;
using Common.Domain;

namespace ClientProducts.Message
{
    public class ContributionsSubaccountsTextsResponse
    {
        public OperationResult Result { get; set; }
        public List<ContractContributionsSubaccountText> SubaccountsTexts { get; set; }

        public ContributionsSubaccountsTextsResponse()
        {
            Result = new OperationResult();
        }
    }
}
