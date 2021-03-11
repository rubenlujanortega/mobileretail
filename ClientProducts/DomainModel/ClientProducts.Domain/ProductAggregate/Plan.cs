using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ProductAggregate
{
    public class Plan : Entity<string>
    {
        private readonly int _planType;
        private readonly string _planComercialName;

        public Plan(string planId, string planComercialName, int planType)
        {
            if (string.IsNullOrEmpty(planId)) { throw new ArgumentException("planId no puede ser nulo o vacío."); }
            if (string.IsNullOrEmpty(planComercialName)) { throw new ArgumentException("planComercialName no puede ser nulo o vacío."); }

            Id = planId;
            _planComercialName = planComercialName;
            _planType = planType;
        }

        public int PlanType => _planType;
        public string PlanComercialName => _planComercialName;
    }
}
