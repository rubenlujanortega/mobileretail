using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.InvestmentContractAggregate
{
    public class InvestmentSource : ValueObject<InvestmentSource>
    {
        private readonly string _referenceNumber;
        private readonly int _investmentSourceType;

        public InvestmentSource(string referenceNumber, int investmentSourceType)
        {
            if(string.IsNullOrEmpty(referenceNumber)) { throw new ArgumentException("referenceNumber no puede ser nulo o vacío."); }

            _referenceNumber = referenceNumber;
            _investmentSourceType = investmentSourceType;
        }

        public string ReferenceNumber => _referenceNumber;
        public int InvestmentSourceType => _investmentSourceType;

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                ReferenceNumber,
                InvestmentSourceType
            };
        }
    }
}
