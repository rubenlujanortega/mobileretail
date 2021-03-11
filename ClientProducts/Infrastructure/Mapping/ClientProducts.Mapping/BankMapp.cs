using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClientProducts.Domain.ContractDetailAggregate;
using NumericValues.Helper;

namespace ClientProducts.Mapping
{
    public static class BankMapp
    {
        public static Bank BankMapping(DataRow dr)
        {
            var bankId = Null.SetNull(dr["Banco_Id"].ToString(), string.Empty).ToString();
            var bankName = Null.SetNull(dr["Banco_Dsc"].ToString(), string.Empty).ToString();

            return Bank.Create(bankId, bankName);
        }

        public static Bank BankMappingEmpty()
        {
            return Bank.Create("", "");
        }
    }
}
