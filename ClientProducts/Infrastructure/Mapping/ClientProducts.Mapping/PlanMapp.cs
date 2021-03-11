using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using ClientProducts.Domain.ProductAggregate;

namespace ClientProducts.Mapping
{
    public static class PlanMapp
    {
        public static Plan PlanMapping(DataSet dsPlan)
        {
            if (dsPlan.Tables.Count>0)
            {
                var drPlan = dsPlan.Tables[0].Rows[0];
                Plan plan = new Plan(drPlan["PlanId"].ToString(), drPlan["Plan_Descripcion_Comercial"].ToString(), int.Parse(drPlan["Plan_Tipo"].ToString()));
                return plan;
            }
            else
            {
                return null;
            }
        }
    }
}
