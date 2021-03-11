using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ClientProducts.Domain.ProductAggregate;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Mapping;
using SqlClient.Helper;
using Unity;

namespace ClientProducts.Repository
{
    public class DOMICILIACIONDa : IDOMICILIACIONDa
    {
        private readonly SqlConnection _dbConn;
        private readonly ISqlClientHelper _sqlClientHelper;
        public DOMICILIACIONDa([Dependency("DOMICILIACION")] SqlConnection dbConn, ISqlClientHelper sqlClientHelper)
        {
            this._dbConn = dbConn;
            _sqlClientHelper = sqlClientHelper;
        }

        public Domiciliation GetContractDirectDebitBasicInfo(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_DCML_GetContratoDomiciliado", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            if(ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                return DomiciliationMapp.DomiciliationMapping(ds);
            }
            else
            {
                return GetContractHSBCDebitBasicInfo(contractId);
            }
        }

        public Domiciliation GetContractHSBCDebitBasicInfo(string contractId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_TC_DCML_GetContratosHSBC", _dbConn);
            cmd.AddParameterWithValue("@pstrContratoId", contractId);
            DataSet ds = _sqlClientHelper.ExecuteDataSet(cmd);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return DomiciliationMapp.DomiciliationMapping(ds);
            }
            else
            {
                return DomiciliationMapp.DomiciliationMappingEmpty();
            }
        }
    }
}
