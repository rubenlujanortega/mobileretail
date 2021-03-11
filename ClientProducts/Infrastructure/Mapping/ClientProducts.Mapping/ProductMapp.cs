using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Mapping
{
    public static class ProductMapp
    {
        public static Product ProductMapping(DataSet dsPlan, string contractId)
        {
            if (dsPlan.Tables.Count > 0)
            {
                var drPlan = dsPlan.Tables[0].Rows[0];
                Product product = Product.Create(contractId, drPlan["PlanId"].ToString(), drPlan["Plan_Descripcion_Comercial"].ToString());
                return product;
            }
            else
            {
                return null;
            }
        }
    }
}
