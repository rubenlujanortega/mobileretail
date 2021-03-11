using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlClient.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using ClientProducts.Repository;

namespace ClientProducts.RepositoryTest
{
    [TestClass]
    public class SecurityGlobalAppDaTest
    {
        private readonly SqlConnection _dbConn;
        private readonly SecurityGlobalAppDa _securityGlobalrepository;
        private readonly Mock<ISqlClientHelper> mockSqlClientHelper;

        public SecurityGlobalAppDaTest()
        {
            _dbConn = new SqlConnection();
            mockSqlClientHelper = new Mock<ISqlClientHelper>();
            _securityGlobalrepository = new SecurityGlobalAppDa(_dbConn, mockSqlClientHelper.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            mockSqlClientHelper.Setup(x => x.GetConnection(_dbConn)).Returns(Mock.Of<IDbConnection>());
            mockSqlClientHelper.Setup(x => x.GetCommand(It.IsAny<IDbConnection>())).Returns(Mock.Of<IDbCommand>());
            mockSqlClientHelper.Setup(x => x.GetDataAdapter(It.IsAny<IDbCommand>())).Returns(Mock.Of<IDbDataAdapter>());
            mockSqlClientHelper.Setup(x => x.CreateCmdSP(It.IsAny<string>(), _dbConn)).Returns(Mock.Of<IDbCommand>());
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetClienteDatosSolvenciaNullError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _securityGlobalrepository.GetClientDatosSolvencia("XXXXXX000000XXX", 1));
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetClienteDatosSolvenciaFailCorruptDB()
        {
            DataSet ds = new DataSet();
            string[] columns = { "UserName", "Email", "Telefono", "NombreCompleto" };
            string[] rows = { null, "test Email", "test telefono", "test full name" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));

            Assert.ThrowsException<ArgumentException>(() => _securityGlobalrepository.GetClientDatosSolvencia("MEAJ720808J25", 1));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetClienteDatosSolvenciaSuccess()
        {
            DataSet ds = new DataSet();
            string[] columns = { "UserName", "Email", "Telefono", "NombreCompleto" };
            string[] rows = { "test name", "test Email", "test telefono", "test full name" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var solvenciaData = _securityGlobalrepository.GetClientDatosSolvencia("MEAJ720808J25", 1);
            Assert.IsNotNull(solvenciaData);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetActivePINEmptyData()
        {
            var data = _securityGlobalrepository.GetActivePIN(null, 2, 4);
            Assert.IsTrue(data == string.Empty);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetActivePINSuccess()
        {
            DataSet ds = new DataSet();
            string[] columns = { "newPin" };
            string[] rows = { "123456" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var data = _securityGlobalrepository.GetActivePIN("MEAJ720808J25", 2, 4);
            Assert.IsTrue(data.Length > 0);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void SetPinUserExecution()
        {
            mockSqlClientHelper.Setup(x => x.ExecuteScalar(It.IsAny<IDbCommand>()));
            _securityGlobalrepository.SetPinUser("123456", "MEAJ720808J25", 2, 4);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetParameterValueNullError()
        {
            mockSqlClientHelper.Setup(x => x.ExecuteScalar(It.IsAny<IDbCommand>()));
            Assert.ThrowsException<NullReferenceException>(() => _securityGlobalrepository.GetParameterValue("passwd", 2));
        }
    }
}
