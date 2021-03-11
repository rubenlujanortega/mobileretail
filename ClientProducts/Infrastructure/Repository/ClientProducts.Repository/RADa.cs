using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClientProducts.Domain.ProductAggregate;
using ClientProducts.Domain.Contributions;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Mapping;
using SqlClient.Helper;
using Unity;

namespace ClientProducts.Repository
{
    public class RADa : IRADa
    {
        private readonly SqlConnection _dbConn;
        private readonly ISqlClientHelper _sqlClientHelper;

        public RADa([Dependency("RA")] SqlConnection dbConn, ISqlClientHelper sqlClientHelper)
        {
            this._dbConn = dbConn;
            _sqlClientHelper = sqlClientHelper;
        }
    

        public int GetClientIdByRFC(string clientIdentifier)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_RA_GetClientMedianteRFC", _dbConn);
            cmd.AddParameterWithValue("@pstrclienteRFC", clientIdentifier);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            return int.Parse(ds.Tables[0].Rows[0]["clt_Id"].ToString());
        }

        public Plan GetPlanInfo(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("sp_RA_GetPlanInfo", _dbConn);
            cmd.AddParameterWithValue("@ContratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            return PlanMapp.PlanMapping(ds);
        }

        public Product GetContractProductInfo(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("sp_RA_GetPlanInfo", _dbConn);
            cmd.AddParameterWithValue("@ContratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            return ProductMapp.ProductMapping(ds, contractId);
        }

        public ContractSOCData GetContractSOCInfo(string contractId, string planComercialName)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_RA_GetContrato", _dbConn);
            cmd.AddParameterWithValue("@ContratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            return ContractSOCInfoMapp.SOCDataMapping(ds.Tables[0].Rows[0], planComercialName);
        }

        public List<ContractContributionsSubaccountText> GetContributionsSubaccountsTexts(string planId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_SOC_RA_Mobile_GetTextosSubcuentasAportaciones", _dbConn);
            cmd.AddParameterWithValue("@productoId", planId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            return ContractContributionsSubaccountTextMapp.GetContractShareSubaccountsTextMapping(ds);
        }


    }
}
