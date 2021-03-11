using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.Contributions;
using NumericValues.Helper;

namespace ClientProducts.Mapping
{
    public static class ContractBankAccountMapp
    {
        public static List<ContractBankAccount> ContractBankAccountMapping(DataSet ds)
        {
            List<ContractBankAccount> contractBankAccounts = new List<ContractBankAccount>();

            if (ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    var accountType = Null.SetNull(row["Tipo_Uso_Cuenta_Id"], string.Empty).ToString();
                    if(accountType == "15" || accountType == "10")
                    {
                        var contractId = Null.SetNull(row["Contrato_Id"], string.Empty).ToString();
                        var accountId = Convert.ToInt32(Null.SetNull(row["Contrato_Cta_Id"], string.Empty).ToString());
                        var accountNumber = Null.SetNull(row["Contrato_Cta_Numero"], string.Empty).ToString();
                        var accountCLABE = Null.SetNull(row["Contrato_Cta_Clabe"], string.Empty).ToString();
                        accountNumber = string.IsNullOrEmpty(accountNumber) ? accountCLABE : accountNumber;
                        var bankId = Convert.ToInt32(Null.SetNull(row["Banco_Id"], string.Empty).ToString());
                        var accountBankName = Null.SetNull(row["Banco_Dsc_Corta"], string.Empty).ToString();
                        var bankAccountTypeId = Convert.ToInt32(Null.SetNull(row["Clasif_Cuenta_Banco_Id"], string.Empty).ToString());
                        contractBankAccounts.Add(ContractBankAccount.Create(contractId, bankId, accountId, accountNumber, accountBankName, bankAccountTypeId));
                    }
                }
            }

            return contractBankAccounts;
        }
    }
}
