using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class OffspringGrouperContract : ValueObject<OffspringGrouperContract>
    {
        private readonly int _productId;
        private readonly string _grouperContractIdentifier;
        private readonly int _grouperPlanId;
        private readonly string _contributionContractId;
        private readonly string _additionalContractId;
        private readonly string _warningMessage;

        public OffspringGrouperContract(int productId, string grouperContractIdentifier, int grouperPlanId, string contributionContractId, string additionalContractId, string warningMessage)
        {
            _productId = productId;
            _grouperContractIdentifier = grouperContractIdentifier;
            _grouperPlanId = grouperPlanId;
            _contributionContractId = contributionContractId;
            _additionalContractId = additionalContractId;
            _warningMessage = warningMessage;
        }

        public int ProductId => _productId;
        public string GrouperContractIdentifier => _grouperContractIdentifier;
        public int GrouperPlanId => _grouperPlanId;
        public string ContributionContractId => _contributionContractId;
        public string AdditionalContractId => _additionalContractId;
        public string WarningMessage => _warningMessage;

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                ProductId,
                GrouperContractIdentifier,
                GrouperPlanId,
                ContributionContractId,
                AdditionalContractId,
                WarningMessage
            };
        }

    }
}
