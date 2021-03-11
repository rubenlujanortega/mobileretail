using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlClient.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using ClientProducts.Repository;
using ClientProducts.Domain.ProductAggregate;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.RepositoryTest
{
    [TestClass]
    public class RAWRAPSIIFDaTest
    {
        private readonly SqlConnection _dbConn;
        private readonly RAWRAPSIIFDa _contractRepository;
        private readonly Mock<ISqlClientHelper> mockSqlClientHelper;
        private ContractSOCData socData;
        private SavingInformation savingInformation;
        public RAWRAPSIIFDaTest()
        {
            _dbConn = new SqlConnection();
            mockSqlClientHelper = new Mock<ISqlClientHelper>();
            _contractRepository = new RAWRAPSIIFDa(_dbConn, mockSqlClientHelper.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            mockSqlClientHelper.Setup(x => x.GetConnection(_dbConn)).Returns(Mock.Of<IDbConnection>());
            mockSqlClientHelper.Setup(x => x.GetCommand(It.IsAny<IDbConnection>())).Returns(Mock.Of<IDbCommand>());
            mockSqlClientHelper.Setup(x => x.GetDataAdapter(It.IsAny<IDbCommand>())).Returns(Mock.Of<IDbDataAdapter>());
            mockSqlClientHelper.Setup(x => x.CreateCmdSP(It.IsAny<string>(), _dbConn)).Returns(Mock.Of<IDbCommand>());

            socData = ContractSOCData.Create("CREA");
            savingInformation = new SavingInformation(socData);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetClientContractsNullReferenceError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _contractRepository.GetClientContracts("599500"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetClientContractsSuccess()
        {
            DataSet ds = new DataSet();
            string[] columns = { "Campo1", "Campo2" };
            string[] rows = { "599500", "599501" };

            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var data = _contractRepository.GetClientContracts("MEAJ720908H25");
            Assert.IsTrue(data.Count > 0);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContractSummaryBalanceNullReferenceError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _contractRepository.GetContractSummaryBalance("599500"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractSummaryBalanceSuccess()
        {
            DataSet ds = new DataSet();
            string[] columns = { "Valor_Fondo" };
            string[] rows = { "1000" };

            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var data = _contractRepository.GetContractSummaryBalance("599500");
            Assert.IsTrue(data == 1000);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContractDetailInfoNullReferenceError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _contractRepository.GetContractDetailInfo("599500", ref socData, out savingInformation));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractDetailInfoSuccess()
        {
            DataSet ds = new DataSet();
            ContractSOCData soc = ContractSOCData.Create("CREA");
            SavingInformation saving;
            string[] columns = { "No_Referencia", "Contrato_Fecha", "Contrato_Plan_Deducible", "Sts_Contrato_Dsc", "Contrato_Origen", "Llego_Actinver_Dsc", "Contrato_Id" };
            string[] rows = { "Ref01", "01/01/2020", "True", "Activo", "599500", "NA", "599500" };


            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var data = _contractRepository.GetContractDetailInfo("599500", ref soc, out saving);
            Assert.IsNotNull(saving);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetContractBalanceNullReferenceError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _contractRepository.GetContractBalance("599500"));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetContractBalanceSuccess()
        {
            DataSet ds = new DataSet();
            ContractSOCData soc = ContractSOCData.Create("CREA");
            SavingInformation saving;
            string[] columns = { "Emision_Id", "Emision_Nombre", "Portafolio_Tit_Imp", "Precio_Fondo", "Precio_Fondo_Venta", "Precio_Fondo_Compra", "Portafolio_Tit_Comprometidos",
            "Portafolio_Tit_Comprometidos_Venta", "Porcentaje", "Precio_Fondo_Fecha"};
            string[] rows = { "Em1", "Emision", "1000", "10", "10", "10", "1000", "1000", "100",  new DateTime().ToString() };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));

            var data = _contractRepository.GetContractBalance("599500");
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessFail")]
        public void GetBankNameNullReferenceError()
        {
            Domiciliation domi = Domiciliation.Create("12345646", 12, 1000M);
            Assert.ThrowsException<NullReferenceException>(() => _contractRepository.GetBankName("599500", 1, domi));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        [DataRow("")]
        [DataRow("12345678")]
        public void GetBankNameDataEmptySuccess(string account)
        {
            Domiciliation domi = Domiciliation.Create(account, 12, 1000M);
            string[] columns = { "Banco_Id", "Banco_Dsc" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns)));
            var data = _contractRepository.GetBankName("599500", 1, domi);
            Assert.IsTrue(data.BankName.Length == 0);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void GetBankNameSuccess()
        {
            Domiciliation domi = Domiciliation.Create("12345646", 12, 1000M);
            string[] columns = { "Banco_Id", "Banco_Dsc" };
            string[] rows = { "12", "Banamex" };
            mockSqlClientHelper.Setup(x => x.ExecuteDataSet(It.IsAny<IDbCommand>())).Returns(new DataSet().AddTable(() => new DataTable().AddColumns(columns).AddRow(rows)));
            var data = _contractRepository.GetBankName("599500", 1, domi);
            Assert.IsTrue(data.BankName.Length>0);
        }
    }
}
