using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class Product : Entity<string>
    {
        private readonly string _number;
        private readonly string _productCode;
        private readonly string _planCode;

        private Product(string productId, string number)
        {
            Id = productId;
            _number = number;
            _productCode = "S-" + productId;  
        }

        public string Number => _number;
        public string ProductCode => _productCode;
        public string PlanCode => _planCode;

        public static Product Create(string contractId, string planId, string planName)
        {
            if (string.IsNullOrEmpty(contractId)) { throw new ArgumentException("contractId no puede ser nulo ni vacío"); }
            if (string.IsNullOrEmpty(planId)) { throw new ArgumentException("planId no puede ser nulo ni vacío"); }
            if (string.IsNullOrEmpty(planName)) { throw new ArgumentException("planName no puede ser nulo ni vacío"); }

            return new Product(planId, contractId + " - " + planName);
        }
    }
}
