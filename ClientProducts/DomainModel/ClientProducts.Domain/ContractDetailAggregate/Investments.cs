using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class Investments : ValueObject<Investments>
    {
        private List<Fund> _funds;
        private decimal _cash;
        private readonly decimal _settledCash;

        public Investments(List<InvestmentContractBalance> balancesInfo)
        {
            if (balancesInfo == null) { throw new ArgumentException("balancesInfo no puede ser nulo."); }

            FillFundsList(balancesInfo);
            _settledCash = 0;
        }

        public List<Fund> Funds => _funds;
        public decimal Cash => _cash;
        public decimal SettledCash => _settledCash;

        private void FillFundsList(List<InvestmentContractBalance> balancesInfo)
        {

            List<Fund> funds = balancesInfo
                    .SelectMany(
                        b => b.FundsBalance
                            .Select(
                                f => Fund.Create(f.Identifier, f.Shares, f.PriceSale, f.MarketSale, f.Pecerntage,false)
                            )).ToList();

            List<Fund> fundsPending = balancesInfo
                    .SelectMany(
                        b => b.CashBalance
                            .Where(p => p.Name.Contains("Pendientes") && p.Shares != 0)
                            .Select(
                                f => Fund.Create(f.Identifier, f.Shares, f.PriceSale, f.MarketSale, f.Pecerntage, true)
                            )).ToList();

            var mergedList = funds.Union(fundsPending).ToList();

            var cash = balancesInfo.Select(b => b.CashBalance
                .Sum(c => c.Shares)).ToList().Sum();

            this._cash = cash;
            this._funds = mergedList;
        }
        

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<object>
            {
                Funds,
                Cash,
                SettledCash
            };
        }
    }
}
