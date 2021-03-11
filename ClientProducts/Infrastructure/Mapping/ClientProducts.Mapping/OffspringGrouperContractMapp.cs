using System;
using System.Data;
using ClientProducts.Domain.ContractDetailAggregate;
using NumericValues.Helper;
namespace ClientProducts.Mapping
{
    public static class OffspringGrouperContractMapp
    {
        public static OffspringGrouperContract OffspringGrouperMapping(DataSet dsContract)
        {
            if (dsContract != null && dsContract.Tables.Count > 0 && dsContract.Tables[0].Rows.Count > 0)
            {
                var productId = int.Parse(Null.SetNull(dsContract.Tables[0].Rows[0]["producto_id"].ToString(), 0).ToString());
                var grouperContractIdentifier = (String)Null.SetNull(dsContract.Tables[0].Rows[0]["contrato_agrupador"].ToString(), string.Empty);
                var grouperPlanId = int.Parse(Null.SetNull(dsContract.Tables[0].Rows[0]["plan_agrupador"], 0).ToString());
                var contributionContractId = (String)Null.SetNull(dsContract.Tables[0].Rows[0]["contrato_aportaciones"].ToString(), string.Empty);
                var additionalContractId = (String)Null.SetNull(dsContract.Tables[0].Rows[0]["contrato_adiciones"].ToString(), string.Empty);
                var warningMessage = (String)Null.SetNull(dsContract.Tables[0].Rows[0]["mensaje_wrn"].ToString(), string.Empty);

                return new OffspringGrouperContract(productId, grouperContractIdentifier, grouperPlanId, contributionContractId, additionalContractId, warningMessage);
            }
            else
            {
                return null;
            }

            
        }
    }
}
