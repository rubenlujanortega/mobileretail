using System;
using System.Data;
using System.Data.SqlClient;
using SqlClient.Helper;
using ClientProducts.Mapping;
using Unity;
using ClientProducts.Domain.ClientSolvenciaDataAggregate;
using Common.Domain;

namespace ClientProducts.Repository
{
    public class SecurityGlobalAppDa : ISecurityGlobalAppDa
    {
        private readonly SqlConnection _dbConn;
        private readonly ISqlClientHelper _sqlClientHelper;

        public SecurityGlobalAppDa([Dependency("SecurityGlobalApp")] SqlConnection dbConn, ISqlClientHelper sqlClientHelper)
        {
            this._dbConn = dbConn;
            _sqlClientHelper = sqlClientHelper;
        }

        public SolvenciaData GetClientDatosSolvencia(string userID, int portalId)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_Solv_GetBasicDataOfUser", _dbConn);
            cmd.AddParameterWithValue("@userName", userID);
            cmd.AddParameterWithValue("@portalId", portalId);

            var ds = _sqlClientHelper.ExecuteDataSet(cmd);

            return SolvenciaDataMapp.SolvenciaDataMapping(ds);
        }

        public string GetActivePIN(string userName, int portalId, int minutes)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_OTP_GetPinActivo_Dinamico", _dbConn);
            cmd.AddParameterWithValue("@userName", userName);
            cmd.AddParameterWithValue("@portalId", portalId);
            cmd.AddParameterWithValue("@minutos", minutes);

            var ds = _sqlClientHelper.ExecuteDataSet(cmd);
            var pin = string.Empty;

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    pin = dr[0] != null ? dr[0].ToString() : "";
                }
            }
            return pin;
        }

        public void SetPinUser(string pin, string userName, int portalId, int minutes)
        {
            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_OTP_SetPinUsuarioVigenciaDinamica", _dbConn);
            cmd.AddParameterWithValue("@pin", pin);
            cmd.AddParameterWithValue("@userName", userName);
            cmd.AddParameterWithValue("@portalId", portalId);
            cmd.AddParameterWithValue("@vigenciaPin", minutes);
            cmd.ExecuteScalar();
        }
        

        public object GetParameterValue(string param, int portalId)
        {
            SqlParameter outValue = new SqlParameter("@valor", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };

            IDbCommand cmd = _sqlClientHelper.CreateCmdSP("usp_Solv_GetValorParametro", _dbConn);
            cmd.AddParameterWithValue("@param", param);
            cmd.AddParameterWithValue("@portal", 1);
            cmd.Parameters.Add(outValue);
            _sqlClientHelper.ExecuteScalar(cmd);
            return outValue.Value;
        }
    }
}
