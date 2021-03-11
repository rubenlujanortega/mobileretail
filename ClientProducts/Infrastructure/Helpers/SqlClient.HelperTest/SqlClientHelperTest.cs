using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using SqlClient.Helper;
using System.Configuration;
using System.Data;

namespace SqlClient.HelperTest
{
    [TestClass]
    public class SqlClientHelperTest
    {
        private readonly SqlConnection _sqlConnection;
        private readonly ISqlClientHelper _sqlClientHelper;
        public SqlClientHelperTest()
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["RA"].ConnectionString);
            _sqlClientHelper = new SqlClientHelper();
        }

        [TestMethod, TestCategory("ProcessSuccessFul")]
        public void ExecuteDataSetSuccess()
        {
            var iDbcommand = _sqlClientHelper.CreateCmdSP("usp_Test_ExecuteDataSet", _sqlConnection);
            Assert.IsInstanceOfType(iDbcommand, typeof(IDbCommand));

            Assert.IsInstanceOfType(iDbcommand.AddParameterWithValue("@parameter", "ExecuteDataSet"), typeof(IDbDataParameter));

            var ds = _sqlClientHelper.ExecuteDataSet(iDbcommand);
            Assert.IsInstanceOfType(ds, typeof(DataSet));
        }


        [TestMethod, TestCategory("ProcessSuccessFul")]
        public void ExecuteNonQuerySuccess()
        {
            var iDbcommand = _sqlClientHelper.CreateCmdSP("usp_Test_ExecuteNonQuery", _sqlConnection);
            Assert.IsInstanceOfType(iDbcommand, typeof(IDbCommand));

            Assert.IsInstanceOfType(iDbcommand.AddParameterWithValue("@parameter", "ExecuteDataSet"), typeof(IDbDataParameter));

            var affectedRows = _sqlClientHelper.ExecuteNonQuery(iDbcommand);
            Assert.IsTrue(affectedRows > 0);
        }

        [TestMethod, TestCategory("ProcessSuccessFul")]
        public void ExecuteScalarSuccess()
        {
            var iDbcommand = _sqlClientHelper.CreateCmdSP("usp_Test_ExecuteNonQuery", _sqlConnection);
            Assert.IsInstanceOfType(iDbcommand, typeof(IDbCommand));

            Assert.IsInstanceOfType(iDbcommand.AddParameterWithValue("@parameter", "ExecuteDataSet"), typeof(IDbDataParameter));

            _sqlClientHelper.ExecuteScalar(iDbcommand);
        }
    }
}
