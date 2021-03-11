using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;
using NumericValues.Helper;

namespace ClientProducts.Mapping
{
    public static class ContractSOCInfoMapp
    {
        public static ContractSOCData SOCDataMapping(DataRow drGeneralInfo, string productName)
        {
            if (drGeneralInfo != null)
            {
                var savingGoal = Convert.ToDecimal(Null.SetNull(drGeneralInfo["Contrato_Objetivo_Ahorro"], new decimal()));
                var savingTerm = Convert.ToInt32(Null.SetNull(drGeneralInfo["Contrato_Plazo_Aportaciones"], new int()));
                var commitedAmount = Convert.ToDecimal(Null.SetNull(drGeneralInfo["Contrato_Aportacion_Mensual"], new decimal()));
                var maxAmountInsuranced = Convert.ToDecimal(Null.SetNull(drGeneralInfo["Contrato_Monto_Maximio_Asegurado"], new decimal()));
                var currentLifeInsuranceAmount =  Convert.ToDecimal(Null.SetNull(drGeneralInfo["Suma_Asegurada_Actual"], new decimal()));
                currentLifeInsuranceAmount = currentLifeInsuranceAmount == 0 ? maxAmountInsuranced : currentLifeInsuranceAmount;
                var terminationDate = Null.SetNull(drGeneralInfo["Contrato_Vigencia_Hasta"], string.Empty).ToString();
                terminationDate = terminationDate != string.Empty ? Convert.ToDateTime(terminationDate).ToString("yyyy-MM-dd") : string.Empty;
                
                var contractSOCInfo = ContractSOCData
                    .Create(productName)
                    .FillSavingInfo(
                        savingGoal,
                        savingTerm,
                        commitedAmount,
                        currentLifeInsuranceAmount,
                        terminationDate);

                return contractSOCInfo;
            }
            else
            {
                return null;
            }
        }
    }
}
