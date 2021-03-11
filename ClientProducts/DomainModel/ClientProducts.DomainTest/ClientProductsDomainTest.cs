using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientProducts.Domain.Contributions;
using ClientProducts.Domain.ClientSolvenciaDataAggregate;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Domain.ProductsOfClienteAggregate;
using ClientProducts.Domain.InvestmentContractAggregate;
using ClientProducts.Domain.ProductAggregate;

namespace ClientProducts.DomainTest
{
    [TestClass]
    public class ClientProductsDomainTest
    {
        private ContractSOCData socData = null;
        private ContractInfo contractInfo = null;
        private Product product = null;
        private Investments investments = new Investments(new List<InvestmentContractBalance>());
        private Contract contract = null;
        private SavingAdvance savingAdvance;
        private CapitalBalance capitalBalance;
        private Domiciliation domiciliation;
        private InvestmentContractBalance investmentContractBalance;
        private InvestmentFundValue investmentFundValue;
        private Plan plan;

        [TestInitialize]
        public void Initialize()
        {
            socData = ContractSOCData.Create("CREA");
            contract = Contract.Create("123456", socData, contractInfo);
            product = Product.Create("123456", "540", "CREA");
            savingAdvance = new SavingAdvance(socData, 1, 1);
            capitalBalance = new CapitalBalance(100);
            plan = new Plan("549", "Skandia", 1);
            domiciliation = Domiciliation.Create("123456", 0, 0);
            investmentContractBalance = InvestmentContractBalance.Create("123456");
            investmentFundValue = InvestmentFundValue.Create("fund-01", "fund-01", 0M, 0M, 0M, 0M, 0M);
        }



        [TestMethod, TestCategory("ParamError")]
        [DataRow("", "fullname")]
        [DataRow("username", null)]
        public void SolvenciaDataParamError(string user, string fullName)
        {
            Assert.ThrowsException<ArgumentException>(() => SolvenciaData.Create(user, "", fullName, ""));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void SolvenciaDataSuccess()
        {
            SolvenciaData solvenciaData = SolvenciaData.Create("username", "", "fullname", "");

            Assert.IsNotNull(solvenciaData);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow(null, "banamez")]
        [DataRow("1", null)]
        public void BankParamError(string bankId, string bankName)
        {
            Assert.ThrowsException<ArgumentException>(() => Bank.Create(bankId, bankName));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void BankSuccess()
        {
            Bank bankData = Bank.Create("1", "banamex");

            Assert.IsNotNull(bankData);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void CapitalBalanceFillBalanceSuccess()
        {
            var data = capitalBalance.FillData(100M, 100000M, 0M, 1000M);

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Equals(new CapitalBalance(100M).FillData(100M, 100000M, 0M, 1000M)));
        }


        [TestMethod, TestCategory("ParamError")]
        public void ContractParamError()
        {
            ContractInfo contractInfo = null;
            Assert.ThrowsException<ArgumentException>(() => Contract.Create("", socData, contractInfo));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractCreateSuccess()
        {
            Contract newContract = Contract.Create("123456", socData, contractInfo);
            Assert.IsNotNull(newContract);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractFillProductSuccess()
        {
            var contractData = contract.FillProduct(product);
            Assert.IsNotNull(contractData.Product);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractFillInvestmentsSuccess()
        {
            var contractData = contract.FillInvestments(investments);
            Assert.IsNotNull(contractData.Investments);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void SavingInformationSuccess()
        {
            socData.FillSavingReason("Ahorro");
            socData.FillSavingInfo(100M, 1, 100M, 50M, "12/12/2030");

            var data = new SavingInformation(socData);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Equals(new SavingInformation(socData)));
            Assert.AreEqual(data.SavingGoal, 100M);
            Assert.AreEqual(data.SavingTerm, 1);
            Assert.AreEqual(data.CommitedAmount, 100M);
            Assert.AreEqual(data.CurrentLifeInsuranceAmount, 50M);
            Assert.AreEqual(data.TerminationDate, "12/12/2030");
            Assert.AreEqual(data.SavingReason, "Ahorro");
            Assert.AreEqual(data.ContributionPeriodicity, "N/A");
            Assert.AreEqual(data.PaymentType, "N/A");
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractFillSavingAdvanceSuccess()
        {
            var contractData = contract.FillSavingAdvance(savingAdvance);
            Assert.IsNotNull(contractData.SavingAdvance);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractFillCapitalBalanceSuccess()
        {
            var contractData = contract.FillCapitalBalance(capitalBalance);
            Assert.IsNotNull(contractData.CapitalBalance);
            Assert.IsTrue(contractData.Equals(contract.FillCapitalBalance(capitalBalance)));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractFillDomiciliationSuccess()
        {
            var contractData = contract.FillDomiciliation(domiciliation);
            Assert.IsNotNull(contractData.Domiciliation);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("")]
        public void ContractSOCDataParamError(string productName)
        {
            Assert.ThrowsException<ArgumentException>(() => ContractSOCData.Create(productName));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractSOCDataCreateSuccess()
        {
            var socData = ContractSOCData.Create("SKANDIA");
            Assert.IsNotNull(socData);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("")]
        public void InvestmentContractBalanceParamError(string contractId)
        {
            Assert.ThrowsException<ArgumentException>(() => InvestmentContractBalance.Create(contractId));
        }


        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentContractBalanceCreateSuccess()
        {
            var data = InvestmentContractBalance.Create("123456");
            Assert.IsNotNull(data);
        }


        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentContractBalanceFillListsSuccess()
        {
            List<InvestmentFundValue> fundsBalance = new List<InvestmentFundValue>();
            List<InvestmentFundValue> cashBalance = new List<InvestmentFundValue>();
            List<InvestmentFundAllocation> source = new List<InvestmentFundAllocation>();
            var data = investmentContractBalance.FillLists(fundsBalance, cashBalance, source);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("")]
        public void ContractSOCDataFillSavingInfoParamError(string terminationDate)
        {
            Assert.ThrowsException<ArgumentException>(() => socData.FillSavingInfo(0M, 0, 0M, 0M, terminationDate));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractSOCDataFillSavingInfoSuccess()
        {
            var socDataTest = socData.FillSavingInfo(0M, 0, 0M, 0M, "01/01/2020");
            Assert.IsNotNull(socDataTest);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("refnmbr", "01/01/2000", false, "activo", 0, 1, null)]
        [DataRow("refnmbr", "01/01/2000", false, "activo", 1, 1, null)]
        public void ContractInfoParamError(string referenceNumber, string afiliationDate, bool applyDeductible, string status, int platform, int type, ContractSOCData socData)
        {
            Assert.ThrowsException<ArgumentException>(() => new ContractInfo(referenceNumber, afiliationDate, applyDeductible, status, platform, type, socData));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractInfoSuccess()
        {
            var contractInfo = new ContractInfo("refnmbr", "01/01/2000", false, "activo", 1, 1, socData);
            Assert.IsNotNull(contractInfo);
            Assert.IsTrue(contractInfo.Equals(new ContractInfo("refnmbr", "01/01/2000", false, "activo", 1, 1, socData)));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractSOCDataFillSavingReasonSuccess()
        {
            var socDataTest = socData.FillSavingReason("Ahorro");
            Assert.IsNotNull(socData.SavingReason);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow(null)]
        public void FundParamError(string fundId)
        {
            Assert.ThrowsException<ArgumentException>(() => Fund.Create(fundId, 0M, 0D, 0D, 0M, false));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void FundCreateSuccess()
        {
            var data = Fund.Create("Fund-01", 0M, 0D, 0D, 0M, false);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        public void InvestmentFundAllocationParamError()
        {
            Assert.ThrowsException<ArgumentException>(() => new InvestmentFundAllocation(null, 0M));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentFundAllocationCreateSuccess()
        {
            InvestmentFundValue investmentFundValue = InvestmentFundValue.Create("fund-01", "fund-01", 0M, 0M, 0M, 0M, 0M);
            var data = new InvestmentFundAllocation(investmentFundValue, 0M);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Equals(new InvestmentFundAllocation(investmentFundValue, 0M)));
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("", "name")]
        [DataRow("identifier", null)]
        public void InvestmentFundValueParamError(string identifier, string fundname)
        {
            Assert.ThrowsException<ArgumentException>(() => InvestmentFundValue.Create(identifier, fundname, 0M, 0M, 0M, 0M, 0M));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentFundValueCreateSuccess()
        {
            var data = InvestmentFundValue.Create("fund-01", "fund-01", 0M, 0M, 0M, 0M, 0M);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentFundValueFillPriceDataSuccess()
        {
            var data = investmentFundValue.FillPriceData(0D, 0D, 0D, new DateTime());
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentFundValueFillMarketDataSuccess()
        {
            var data = investmentFundValue.FillMarketData(0D, 0D, 0D, 0M);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        public void InvestmentsFillFundsListParamError()
        {
            Assert.ThrowsException<ArgumentException>(() => new Investments(null));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentsFillFundsListSuccess()
        {
            InvestmentContractBalance contractBalance = InvestmentContractBalance.Create("599500");
            InvestmentFundValue fundValue = InvestmentFundValue.Create("F-01", "F-01", 100M, 0M, 100M, 100M, 0M);
            InvestmentFundValue fundCash = InvestmentFundValue.Create("Pendientes", "Pendientes", 100M, 0M, 0M, 0M, 0M);
            InvestmentFundAllocation fundPending = new InvestmentFundAllocation(fundValue, 100M);
            contractBalance
                .FillLists(new List<InvestmentFundValue>() { fundValue }, new List<InvestmentFundValue>() { fundCash }, new List<InvestmentFundAllocation>() { fundPending });

            var data = new Investments(new List<InvestmentContractBalance>() { contractBalance });
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow(null)]
        public void ContractSOCDataFillSavingReasonParamError(string savingReason)
        {
            Assert.ThrowsException<ArgumentException>(() => socData.FillSavingReason(savingReason));
        }


        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractSOCDataFillTotalWithDrawalsSuccess()
        {
            var socDataTest = socData.FillTotalWithDrawals(0);
            Assert.IsNotNull(socData.TotalWithDrawals);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractSOCDataFillAdditionalContributionsSuccess()
        {
            var socDataTest = socData.FillAdditionalContributions(0);
            Assert.IsNotNull(socData.AdditionalContributions);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractSOCDataFillTotalContributionsSuccess()
        {
            var socDataTest = socData.FillTotalContributions(0);
            Assert.IsNotNull(socData.TotalContributions);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void FillBankSuccess()
        {
            var domiciliationBank = domiciliation.FillBank(Bank.Create("1","BBVA"));
            Assert.IsNotNull(domiciliationBank);
        }



        [TestMethod, TestCategory("ParamError")]
        [DataRow(0, 1, "header", "detail")]
        [DataRow(1, 0, "header", "detail")]
        public void ContractShareSubaccountTextParamError(int subaccountId, int order, string headerText, string detailText)
        {
            Assert.ThrowsException<ArgumentException>(() => ContractContributionsSubaccountText.Create(subaccountId, order, headerText, detailText));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractShareSubaccountTextSuccess()
        {
            ContractContributionsSubaccountText subaccountTexts = ContractContributionsSubaccountText.Create(1, 1, "header", "detail");

            Assert.IsNotNull(subaccountTexts);
            Assert.IsTrue(subaccountTexts.Id.Equals(1));
            Assert.IsTrue(subaccountTexts.Order.Equals(1));
            Assert.IsTrue(subaccountTexts.HeaderText.Equals("header"));
            Assert.IsTrue(subaccountTexts.DetailText.Equals("detail"));
        }

        [TestMethod, TestCategory("ParamError")]
        public void ProductsOfClientParamError()
        {
            Assert.ThrowsException<ArgumentException>(() => new ProductsOfClient(""));
        }

        [TestMethod, TestCategory("ParamError")]
        public void ProductsOfClientAddProductParamError()
        {
            var product = new ProductsOfClient("MEAJ720808H25");
            Assert.ThrowsException<ArgumentException>(() => product.AddProduct(null));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ProductsOfClientSuccess()
        {
            var data = new ProductsOfClient("MEAJ720808J25");
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ProductsOfClientAddProductSuccess()
        {
            var product = new ProductsOfClient("MEAJ720808J25");
            product.AddProduct(Domain.ProductAggregate.ProductSummary.Create("123456",0D));
            Assert.IsNotNull(product.Products.Count() > 0);
        }

        [TestMethod, TestCategory("ParamError")]
        public void InvestmentSourceParamError()
        {
            Assert.ThrowsException<ArgumentException>(() => new InvestmentSource("", 1));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void InvestmentSourceCreateSuccess()
        {
            var data = new InvestmentSource("Source-01", 1);
            Assert.IsNotNull(data);
            Assert.IsTrue(data.Equals(new InvestmentSource("Source-01", 1)));
            Assert.IsTrue(data.ReferenceNumber.Equals("Source-01"));
            Assert.IsTrue(data.InvestmentSourceType.Equals(1));


        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("","planname")]
        [DataRow("planId", "")]
        public void PlanParamError(string planId, string planComercialName)
        {
            Assert.ThrowsException<ArgumentException>(() => new Plan(planId, planComercialName, 1));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void PlanCreateSuccess()
        {
            var data = new Plan("549", "Skandia", 1);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        public void ProductSummaryParamError()
        {
            Assert.ThrowsException<ArgumentException>(() => ProductSummary.Create("", 0D));
        }


        [TestMethod, TestCategory("ParamError")]
        public void ProductSummaryFillPlanParamError()
        {
            var summary = ProductSummary.Create("549", 0D);
            Assert.ThrowsException<ArgumentException>(() => summary.FillPlan(null));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ProductSummaryCreateSuccess()
        {
            var data = ProductSummary.Create("549", 0D);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ProductSummaryFillPlanSuccess()
        {
            var summary = ProductSummary.Create("549", 0D);
            var data = summary.FillPlan(plan);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        [DataRow("", "123456")]
        [DataRow("userId", null)]
        public void ClientOTPParamError(string userId, string newOtp)
        {
            Assert.ThrowsException<ArgumentException>(() => ClientOTP.Create(userId, newOtp, "", ""));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ClientOTPCreateSuccess()
        {
            var data = ClientOTP.Create("userId", "123456", "correo", "telefono");
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ParamError")]
        public void ContractBankAccountParamError()
        {
            Assert.ThrowsException<ArgumentException>(() => ContractBankAccount.Create("", 1, 1, "123456", "Banamex", 5));
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void ContractBankAccountCreateSuccess()
        {
            var data = ContractBankAccount.Create("123456", 1, 1, "123456", "Banamex", 5);
            Assert.IsNotNull(data);
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void TransactionsSummaryInfoCreateSuccess()
        {
            var data = new TransactionsSummaryInfo(0M, 0M, 0, 0);
            Assert.IsNotNull(data);
        }

    }
}
