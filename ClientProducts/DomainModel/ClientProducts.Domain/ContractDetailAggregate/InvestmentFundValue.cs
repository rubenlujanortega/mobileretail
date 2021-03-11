using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ContractDetailAggregate
{
    public class InvestmentFundValue : Entity<string>
    {
        private string _identifier;
        private string _name;
        private decimal _shares;
        private decimal _compromisedShares;
        private decimal _compromisedSharesSales;
        private decimal _compromisedAmount;
        private decimal _compromisedAmountSales;
        private double _priceValuation;
        private double _priceBuy;
        private double _priceSale;
        private DateTime _priceDate;
        private double _marketValuation;
        private double _marketBuy;
        private double _marketSale;
        private decimal _percentage;

        private InvestmentFundValue(string identifier, string name, decimal shares, decimal cShares, decimal cSharesSales, decimal cAmount, decimal cAmountSales)
        {
            _identifier = identifier;
            _name = name;
            _shares = shares;
            _compromisedShares = cShares;
            _compromisedSharesSales = cSharesSales;
            _compromisedAmount = cAmount;
            _compromisedAmountSales = cAmountSales;
        }

        public  string Identifier => _identifier;
        public  string Name => _name;
        public  decimal Shares => _shares;
        public  decimal CompromisedShares => _compromisedShares;
        public  decimal CompromisedSharesSales => _compromisedSharesSales;
        public  decimal CompromisedAmount => _compromisedAmount;
        public  decimal CompromisedAmountSales  => _compromisedAmountSales;
        public  double PriceValuation => _priceValuation;
        public  double PriceBuy => _priceBuy;
        public  double PriceSale => _priceSale;
        public  DateTime PriceDate => _priceDate;
        public  double MarketValuation => _marketValuation;
        public  double MarketBuy => _marketBuy;
        public  double MarketSale => _marketSale;
        public decimal Pecerntage => _percentage;

        public static InvestmentFundValue Create(string identifier, string name, decimal shares, decimal cShares, decimal cSharesSales, decimal cAmount, decimal cAmountSales)
        {
            if (string.IsNullOrEmpty(identifier)) { throw new ArgumentException("identifier no puede ser nulo o vacío."); }
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("name no puede ser nulo o vacío."); }
            return new InvestmentFundValue(identifier, name, shares, cShares, cSharesSales, cAmount, cAmountSales);
        }

        public InvestmentFundValue FillPriceData(double valuation, double sale, double buy, DateTime priceDate)
        {
            this._priceValuation = valuation;
            this._priceSale = sale;
            this._priceBuy = buy;
            this._priceDate = priceDate;
            return this;
        }

        public InvestmentFundValue FillMarketData(double marketValuation, double marketBuy, double marketSale, decimal percentage)
        {
            this._marketValuation = marketValuation;
            this._marketBuy = marketBuy;
            this._marketSale = marketSale;
            this._percentage = percentage;
            return this;
        }
    }
}
