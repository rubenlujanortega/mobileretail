using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Mapping
{
    public static class ContractMapp
    {
        public static Contract MappingContractDetailInfo(DataSet dsContractDetail, ref ContractSOCData contractSOC)
        {
            var dr = dsContractDetail.Tables[0].Rows[0];
            string referenceNumber = dr["No_Referencia"].ToString();
            string afiliationDate = Convert.ToDateTime(dr["Contrato_Fecha"]).ToString("yyyy-MM-dd");
            bool applyDeductible = Convert.ToBoolean(dr["Contrato_Plan_Deducible"]);
            string status = dr["Sts_Contrato_Dsc"].ToString();
            var platform = 1;
            var type = 5; 
            if (dsContractDetail.Tables[0].Rows[0]["Contrato_Origen"].ToString() == "RS")
            {
                platform = 0; 
                type = 10;
            }

            var savingReason = dsContractDetail.Tables[0].Rows[0]["Llego_Actinver_Dsc"].ToString();
            contractSOC.FillSavingReason(savingReason);

            ContractInfo contractInfo = new ContractInfo(
                referenceNumber, 
                afiliationDate, 
                applyDeductible, 
                status, 
                platform,
                type, 
                contractSOC);


            var contractId = dsContractDetail.Tables[0].Rows[0]["Contrato_Id"].ToString();
            Contract contractDetail = Contract
                .Create(contractId, contractSOC, contractInfo);

            return contractDetail;
        }
    }
}
