using System.Data;
using System.Data.SqlClient;

namespace SqlClient.Helper
{
    public interface ISqlClientHelper
    {
        IDbConnection GetConnection(SqlConnection dbConn);
        IDbCommand GetCommand(IDbConnection cn);
        IDataAdapter GetDataAdapter(IDbCommand cmd);
        DataSet FillDataset(IDataAdapter dAdapter);
        IDbCommand CreateCmdSP(string storeProcName, SqlConnection dbConn);
        DataSet ExecuteDataSet(IDbCommand cmd);
        int ExecuteNonQuery(IDbCommand cmd);
        object ExecuteScalar(IDbCommand cmd);
    }
}
