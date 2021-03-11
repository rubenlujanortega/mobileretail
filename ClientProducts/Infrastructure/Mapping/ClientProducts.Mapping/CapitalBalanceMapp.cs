using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Mapping
{
    public static class CapitalBalanceMapp
    {
        public static CapitalBalance CapitalBalanceMapping(decimal totalBalance)
        {
            return new CapitalBalance(totalBalance);
        }
    }
}
