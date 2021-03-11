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
    public class DomiciliationDATest
    {
        private readonly SqlConnection _dbConn;
        private readonly DOMICILIACIONDa _domiciliacionrepository;
        private readonly Mock<ISqlClientHelper> mockSqlClientHelper;

        public DomiciliationDATest()
        {
            _dbConn = new SqlConnection();
            mockSqlClientHelper = new Mock<ISqlClientHelper>();
            _domiciliacionrepository = new DOMICILIACIONDa(_dbConn, mockSqlClientHelper.Object);
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
        public void GetContractDirectDebitBasicInfoNullError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _domiciliacionrepository.GetContractDirectDebitBasicInfo("595500"));
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContractDirectDebitBasicInfoFailCorruptDB()
        {
            DataSet ds = new DataSet();
            string[] columns = { "Contrato_Cta_Num", "Cobro_Monto", "OtherField" };
            string[] rows = { "12313132131", "1200", "12" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));

            Assert.ThrowsException<ArgumentException>(() => _domiciliacionrepository.GetContractDirectDebitBasicInfo("595500"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractDirectDebitBasicInfoNoDataSuccess()
        {
            DataSet ds = new DataSet();
            string[] columns = { "Contrato_Cta_Num", "Cobro_Monto", "Cobro_Dia_Mes" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns)));
            var data = _domiciliacionrepository.GetContractDirectDebitBasicInfo("595500");
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractDirectDebitBasicInfoSuccess()
        {
            DataSet ds = new DataSet();
            string[] columns = { "Contrato_Cta_Num", "Cobro_Monto", "Cobro_Dia_Mes" };
            string[] rows = { "12313132131", "1200", "12" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var data = _domiciliacionrepository.GetContractDirectDebitBasicInfo("595500");
            Assert.IsNotNull(data);
        }
    }
}
