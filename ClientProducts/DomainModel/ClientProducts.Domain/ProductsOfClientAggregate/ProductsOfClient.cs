using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;
using ClientProducts.Domain.ProductAggregate;

namespace ClientProducts.Domain.ProductsOfClienteAggregate
{
    public class ProductsOfClient : Entity<string>
    {
        private List<ProductSummary> _products;

        public ProductsOfClient(string clientIdentifier)
        {
            if (string.IsNullOrEmpty(clientIdentifier)) { throw new ArgumentException("clientIdentifier no puede ser nulo o vacío"); }

            Id = clientIdentifier;
            _products = new List<ProductSummary>();
        }

        public IEnumerable<ProductSummary> Products => _products.ToList().AsReadOnly();

        public ProductsOfClient AddProduct(ProductSummary product)
        {
            if(product is null) { throw new ArgumentException("product no puede ser nulo."); }
            _products.Add(product);
            return this;
        }
    }
}
