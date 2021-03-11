using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlClient.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using ClientProducts.Repository;
using ClientProducts.Domain.ProductAggregate;

namespace ClientProducts.RepositoryTest
{
    [TestClass]
    public class RADaTest
    {
        private readonly SqlConnection _dbConn;
        private readonly RADa _planRepository;
        private readonly Mock<ISqlClientHelper> mockSqlClientHelper;

        public RADaTest()
        {
            _dbConn = new SqlConnection();
            mockSqlClientHelper = new Mock<ISqlClientHelper>();
            _planRepository = new RADa(_dbConn, mockSqlClientHelper.Object);
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
        public void GetClientIdByRFCError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _planRepository.GetClientIdByRFC("MEAJ720808J25"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetClientIdByRFCSuccess()
        {
            DataSet ds = new DataSet();

            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns("clt_Id").AddRow("1234")));

            var plan = _planRepository.GetClientIdByRFC("MEAJ720808J25");
            Assert.IsNotNull(plan);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetPlanInfoError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _planRepository.GetPlanInfo("123456"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetPlanInfoSuccess()
        {
            DataSet ds = new DataSet();

            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet()
              .AddTable(
                  () => new DataTable()
                      .AddColumns("PlanId", "Contrato_Id", "Plan_Descripcion_Comercial", "Plan_Tipo")
                      .AddRow("540", "599500", "CREA", 1)));

            var plan = _planRepository.GetPlanInfo("599500");
            Assert.IsNotNull(plan);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContractProductInfoError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _planRepository.GetContractProductInfo("123456"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractProductInfoSuccess()
        {
            DataSet ds = new DataSet();

            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet()
              .AddTable(
                  () => new DataTable()
                      .AddColumns("PlanId",  "Plan_Descripcion_Comercial")
                      .AddRow("540", "CREA")));

            var plan = _planRepository.GetContractProductInfo("123456");
            Assert.IsNotNull(plan);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContractSOCInfoError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _planRepository.GetContractSOCInfo("123456","CREA"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractSOCInfoSuccess()
        {
            DataSet ds = new DataSet();
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>()))
                 .Returns(ds
                        .AddTable(
                         () => new DataTable()
                             .AddColumns("Contrato_Objetivo_Ahorro", "Contrato_Plazo_Aportaciones", "Contrato_Aportacion_Mensual", "Contrato_Monto_Maximio_Asegurado", "Suma_Asegurada_Actual", "Contrato_Vigencia_Hasta")
                             .AddRow(1000000, 54, 1500, 0, 0, "12/12/2029")));

            var data = _planRepository.GetContractSOCInfo("123456", "CREA");
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContributionsSubaccountsTextsError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _planRepository.GetContributionsSubaccountsTexts("540"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContributionsSubaccountsTextsSuccess()
        {
            DataSet ds = new DataSet();
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>()))
                 .Returns(ds
                        .AddTable(
                         () => new DataTable()
                             .AddColumns("Id", "SubcuentaOrden", "TextoEncabezado", "TextoDetalle")
                             .AddRow(1, 1, "header", "detail")));

            var data = _planRepository.GetContributionsSubaccountsTexts("540");
            Assert.IsNotNull(data);
        }
    }
}
