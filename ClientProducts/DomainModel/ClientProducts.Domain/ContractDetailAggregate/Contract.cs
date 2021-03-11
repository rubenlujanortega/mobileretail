using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class Contract : Entity<string>
    {
        private Product _product;
        private ContractInfo _contractInfo;
        private SavingInformation _savingInformation;
        private SavingAdvance _savingAdvance;
        private Domiciliation _domiciliation;
        private CapitalBalance _capitalBalance;
        private Investments _investments;

        private Contract(string contractNumber, ContractSOCData socData, ContractInfo contractInfo)
        {
            Id = contractNumber;
            _savingInformation = new SavingInformation(socData);
            _contractInfo = contractInfo;
        }

        public string ContractIdentifier => Id;
        public ContractInfo ContractBasicInfo => _contractInfo;
        public Product Product => _product;
        public SavingInformation SavingInformation => _savingInformation;
        public SavingAdvance SavingAdvance => _savingAdvance;
        public Domiciliation Domiciliation => _domiciliation;
        public CapitalBalance CapitalBalance => _capitalBalance;
        public Investments Investments => _investments;
        

        public static Contract Create(string contractId, ContractSOCData socData, ContractInfo contractInfo)
        {
            if (string.IsNullOrEmpty(contractId)) { throw new ArgumentException("contractId no puede ser nulo ni vacío"); }

            return new Contract(contractId, socData, contractInfo);
        }
      

        public  Contract FillProduct(Product product)
        {
            this._product = product;
            return this;
        }

        public Contract FillInvestments(Investments investments)
        {
            this._investments = investments;
            return this;
        }
      

        public Contract FillSavingAdvance(SavingAdvance savingAdvance)
        {
            this._savingAdvance = savingAdvance;
            return this;
        }

        public Contract FillCapitalBalance(CapitalBalance capitalBalance)
        {
            this._capitalBalance = capitalBalance;
            return this;
        }

        public Contract FillDomiciliation(Domiciliation domiciliation)
        {
            this._domiciliation = domiciliation;
            return this;
        }


    }
}
