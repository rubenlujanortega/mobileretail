using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ProductAggregate
{
    public class ProductSummary : Entity<string>
    {
        private string _planId;
        private string _comercialName;
        private readonly double _totalBalance;
        private int _productType;

        private ProductSummary(string productId, double totalBalance)
        {
            Id = productId;
            _totalBalance = totalBalance;
        }

        public string ProductId => Id;
        public string PlanId => _planId;
        public string ProductComercialName => _comercialName;
        public double TotalBalance => _totalBalance;
        public int ProductType => _productType;

        public static ProductSummary Create(string productId, double totalBalance)
        {
            if (string.IsNullOrEmpty(productId)) { throw new ArgumentException("productId no puede ser nulo ni vacío"); }

            return new ProductSummary(productId, totalBalance);
        }

        public ProductSummary FillPlan(Plan plan)
        {
            if(plan == null) { throw new ArgumentException("Plan no puede ser nulo."); }

            this._planId = plan.Id;
            this._comercialName = plan.PlanComercialName;
            this._productType = plan.PlanType;
            return this;
        }
    }
}
