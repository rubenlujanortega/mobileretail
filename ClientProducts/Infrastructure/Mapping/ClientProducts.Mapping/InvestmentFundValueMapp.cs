using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Mapping
{
    public static class InvestmentContractBalanceMapp
    {
        public static InvestmentContractBalance InvestmentContractMapping(DataSet dsShares, DataTable dtPortfolio)
        {
            InvestmentContractBalance investmentBalance = null;
            DataRow[] drShares = null;
            var shares = 0M;
            var marketValue = 0D;
            var marketValueSale = 0D;
            var marketValueBuy = 0D;
            List<InvestmentFundValue> investmentFundsBalance = new List<InvestmentFundValue>();
            List<InvestmentFundValue> cash = new List<InvestmentFundValue>();
            List<InvestmentFundAllocation> allocation = new List<InvestmentFundAllocation>();

            foreach (DataRow dr in dtPortfolio.Rows)
            {
                var identifier = dr["Emision_Id"].ToString();
                var name = dr["Emision_Nombre"].ToString();
                drShares = dsShares.Tables[0].Select("Emision_Id='" + dr["Emision_Id"].ToString().Trim() + "'");
                InvestmentFundValue currentFundValue = null;
                if (drShares != null && drShares.Length > 0)
                {
                    shares = Convert.ToDecimal(drShares[0]["Portafolio_Tit_Imp"]);
                    if (identifier.IndexOf("|") >= 0)
                    {
                        marketValue = Convert.ToDouble(dr["Precio_Fondo"]) * Math.Round(Convert.ToDouble(drShares[0]["Portafolio_Tit_Imp"]), 2);
                        marketValueSale = Convert.ToDouble(dr["Precio_Fondo_Venta"]) * Math.Round(Convert.ToDouble(drShares[0]["Portafolio_Tit_Imp"]), 2);
                        marketValueBuy = Convert.ToDouble(dr["Precio_Fondo_Compra"]) * Math.Round(Convert.ToDouble(drShares[0]["Portafolio_Tit_Imp"]), 2);
                    }
                    else
                    {
                        marketValue = Convert.ToDouble(dr["Precio_Fondo"]) * Convert.ToInt32(drShares[0]["Portafolio_Tit_Imp"]);
                        marketValueSale = Convert.ToDouble(dr["Precio_Fondo_Venta"]) * Convert.ToInt32(drShares[0]["Portafolio_Tit_Imp"]);
                        marketValueBuy = Convert.ToDouble(dr["Precio_Fondo_Compra"]) * Convert.ToInt32(drShares[0]["Portafolio_Tit_Imp"]);
                    }

                    var valuation = marketValue;
                    var sale = marketValueSale;
                    var buy = marketValueBuy;

                    var CompromisedShares = Convert.ToDecimal(dr["Portafolio_Tit_Comprometidos"]);
                    var CompromisedSharesSales = Convert.ToDecimal(dr["Portafolio_Tit_Comprometidos_Venta"]);
                    var CompromisedAmount = CompromisedShares * Convert.ToDecimal(dr["Precio_Fondo_Venta"]);
                    var CompromisedAmountSales = CompromisedSharesSales * Convert.ToDecimal(dr["Precio_Fondo_Venta"]);
                    var percentage = Convert.ToDecimal(dr["Porcentaje"]);
                    currentFundValue = InvestmentFundValue
                        .Create(identifier, name, shares, CompromisedShares, CompromisedSharesSales, CompromisedAmount, CompromisedAmountSales)
                        .FillMarketData(valuation, buy, sale, percentage);
                }

                var priceValuation = Convert.ToDouble(dr["Precio_Fondo"]);
                var priceSale = Convert.ToDouble(dr["Precio_Fondo_Venta"]);
                var priceBuy = Convert.ToDouble(dr["Precio_Fondo_Compra"]);
                var pricePriceDate = Convert.ToDateTime(dr["Precio_Fondo_Fecha"]);

                currentFundValue.FillPriceData(priceValuation, priceSale, priceBuy, pricePriceDate);

                if (currentFundValue.Identifier.IndexOf("|") >= 0)
                {
                    cash.Add(currentFundValue);
                }
                else
                {
                    investmentFundsBalance.Add(currentFundValue);
                }

                investmentBalance = InvestmentContractBalance
                    .Create(currentFundValue.Identifier.Replace("|", ""))
                    .FillLists(investmentFundsBalance, cash, allocation);
            }

            return investmentBalance;
        }
    }
}
