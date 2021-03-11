using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.Contributions
{
    public class ContractContributionsSubaccountText : Entity<int>
    {
        private int _order;
        private string _headerText;
        private string _detailText;
        private string _subaccountId;

        private ContractContributionsSubaccountText(int subaccountId, int order, string headerText, string detailText)
        {
            Id = subaccountId;
            _order = order;
            _headerText = headerText;
            _detailText = detailText;
        }

        public int SubaccountId => Id;
        public int Order => _order;
        public string HeaderText => _headerText;
        public string DetailText => _detailText;


        public static ContractContributionsSubaccountText Create(int subaccountId, int order, string headerText, string detailText)
        {
            if(subaccountId < 1) { throw new ArgumentException("subaccountId debe ser mayo a 0"); }
            if(order < 1) { throw new ArgumentException("order debe ser mayo a 0"); }

            return new ContractContributionsSubaccountText(subaccountId, order, headerText, detailText);
        }

    }
}
