using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class ContractInfo : ValueObject<ContractInfo>
    {
        private readonly int _productType;
        private readonly int _platform;
        private readonly string _productName;
        private readonly string _referenceNumber;
        private readonly string _afiliationDate;
        private readonly bool _applyDeductible;
        private readonly string _status;

        public ContractInfo(string referenceNumber, string afiliationDate, bool applyDeductible, string status, int platform, int type, ContractSOCData socData )
        {
            if(platform < 0) { throw new ArgumentException("platform debe ser mayor o igual a 0"); }
            if(socData == null) { throw new ArgumentException("socData no puede ser nulo"); }

            _referenceNumber = referenceNumber;
            _afiliationDate = afiliationDate;
            _applyDeductible = applyDeductible;
            _status = status;
            _productType = type;
            _platform = platform;
            _productName = socData.ProductName;
        }
        public string ProductName => _productName;
        public int ProductType => _productType;
        public int Plataform => _platform;
        public string ReferenceNumber => _referenceNumber;
        public string AfiliationDate => _afiliationDate;
        public bool Deductible => _applyDeductible;
        public string Status => _status;

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                ProductName,
                ProductType,
                Plataform,
                ReferenceNumber,
                AfiliationDate,
                Deductible,
                Status
            };
        }
    }
}
