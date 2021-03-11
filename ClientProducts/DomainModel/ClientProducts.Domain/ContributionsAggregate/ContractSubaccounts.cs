using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientProducts.Domain._Ddd;
using ClientProducts.Domain.ContractDetailAggregate;
using System.Threading.Tasks;

namespace ClientProducts.Domain.Contributions
{
    public class ContractContributionSubaccount : ValueObject<ContractContributionSubaccount> {
        private readonly int _subaccountId;
        private string _subcontract;
        private readonly string _contract;
        private readonly int _displayOrder;
        private readonly string _headerText;
        private readonly string _detailText;
        private int _type;

        private ContractContributionSubaccount(int subaccountId, int order, string headerText, string detailText, OffspringGrouperContract subaccountsContracts)
        {
            if (subaccountsContracts != null)
            {
                _subaccountId = subaccountId;
                _headerText = headerText;
                _displayOrder = order;
                _detailText = detailText;
                _subcontract = subaccountsContracts.ContributionContractId;
                _type = 1;
                if (order == 2)
                {
                    _subcontract = subaccountsContracts.AdditionalContractId;
                    _type = 2;
                }
            }
        }

        public int SubaccountId => _subaccountId;
        public string SubcontractId => _subcontract;
        public int Order => _displayOrder;
        public string HeaderText => _headerText;
        public string DetailText => _detailText;
        public int AccountType => _type;

        public static ContractContributionSubaccount Create(int subaccountId, int order, string headerText, string detailText, OffspringGrouperContract subaccountsContracts)
        {
            if (subaccountsContracts == null ) { throw new ArgumentException("No hay subcuentas para aportaciones en este contrato"); }
            return new ContractContributionSubaccount(subaccountId, order, headerText, detailText, subaccountsContracts);
        }

        public ContractContributionSubaccount FillSubcontractData(string contractId, int type)
        {
            _subcontract = contractId;
            _type = type;

            return this;
        }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            throw new NotImplementedException();
        }
    }
}
