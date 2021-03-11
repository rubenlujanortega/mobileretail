using ClientProducts.Domain.ProductsOfClienteAggregate;
using Common.Domain;

namespace ClientProducts.Message
{
    public class ClientProductsResponse
    {
        public ProductsOfClient ClientProducts { get; set; }
        public OperationResult Result { get; set; }

        public ClientProductsResponse()
        {
            Result = new OperationResult();
        }

    }
}
