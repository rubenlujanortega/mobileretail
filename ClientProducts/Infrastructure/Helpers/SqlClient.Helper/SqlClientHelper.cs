using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SqlClient.Helper
{
    public class SqlClientHelper : ISqlClientHelper
    {
        private SqlConnection _dbConn;
        private IDbConnection _idbConn;
        public virtual IDbConnection GetConnection(SqlConnection dbConn)
        {
            _dbConn = dbConn;
            _idbConn = _dbConn;
            return _dbConn;
        }


        public virtual IDbCommand GetCommand(IDbConnection cn)
        {
            return cn.CreateCommand();
        }

        public virtual IDataAdapter GetDataAdapter(IDbCommand cmd)
        {
            StringBuilder strParameters = new StringBuilder();
            var da = new SqlDataAdapter(cmd.CommandText, _dbConn);
            foreach (SqlParameter p in cmd.Parameters)
            {
                da.SelectCommand.Parameters.Add(new SqlParameter(p.ParameterName, p.Value));
                strParameters.Append(string.Concat(" ", p.ParameterName, ","));
            }
            da.SelectCommand.CommandText += strParameters.ToString().Substring(0, strParameters.Length - 1);
            return da;
        }


        public virtual DataSet FillDataset(IDataAdapter dAdapter)
        {
            DataSet ds = new DataSet();
            dAdapter.Fill(ds);

            return ds;
        }

        public virtual IDbCommand CreateCmdSP(string storeProcName, SqlConnection dbConn)
        {
            IDbConnection cn = GetConnection(dbConn);
            IDbCommand cmd = GetCommand(cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = storeProcName;

            return cmd;
        }

        public virtual DataSet ExecuteDataSet(IDbCommand cmd)
        {
            if (_idbConn.State == ConnectionState.Closed) { _idbConn.Open(); }
            IDataAdapter da = GetDataAdapter(cmd);
            return FillDataset(da);
        }

        public int ExecuteNonQuery(IDbCommand cmd)
        {
            if (_idbConn.State == ConnectionState.Closed) { _idbConn.Open(); }
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(IDbCommand cmd)
        {
            if (_idbConn.State == ConnectionState.Closed) { _idbConn.Open(); }
            return cmd.ExecuteScalar();
        }
    }

    public static class ExtensionsDB
    {
        public static IDbDataParameter AddParameterWithValue(this IDbCommand cmd, string paramName, object paramValue)
        {
            var dbParam = cmd.CreateParameter();
            if (dbParam != null)
            {
                dbParam.ParameterName = paramName;
                dbParam.Value = paramValue;
                cmd.Parameters.Add(dbParam);
            }
            return dbParam;
        }
    }
}
