using System;
using System.Data;
using System.Data.SqlClient;
using ClientProducts.Domain.ClientSolvenciaDataAggregate;

namespace ClientProducts.Mapping
{
    public static class SolvenciaDataMapp
    {
        public static SolvenciaData SolvenciaDataMapping(DataSet dsSolvencia)
        {
            if(dsSolvencia.Tables.Count>0 && dsSolvencia.Tables[0].Rows.Count>0)
            {
                var user = dsSolvencia.Tables[0].Rows[0]["UserName"] != DBNull.Value ? dsSolvencia.Tables[0].Rows[0]["UserName"].ToString() : String.Empty;
                var email = dsSolvencia.Tables[0].Rows[0]["Email"] != DBNull.Value ? dsSolvencia.Tables[0].Rows[0]["Email"].ToString() : String.Empty;
                var cellPhone = dsSolvencia.Tables[0].Rows[0]["Telefono"] != DBNull.Value ? dsSolvencia.Tables[0].Rows[0]["Telefono"].ToString() : String.Empty;
                var fullName = dsSolvencia.Tables[0].Rows[0]["NombreCompleto"] != DBNull.Value ? dsSolvencia.Tables[0].Rows[0]["NombreCompleto"].ToString() : String.Empty;
                return SolvenciaData.Create(user, email, fullName, cellPhone);
            }
            else
            {
                return null;
            }
        }
    }
}
