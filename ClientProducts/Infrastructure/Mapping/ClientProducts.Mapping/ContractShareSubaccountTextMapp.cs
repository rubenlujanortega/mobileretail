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
    public static class ContractContributionsSubaccountTextMapp
    {
        public static List<ContractContributionsSubaccountText> GetContractShareSubaccountsTextMapping(DataSet ds)
        {
            List<ContractContributionsSubaccountText> subaccountList = new List<ContractContributionsSubaccountText>();
            foreach(DataRow row in ds.Tables[0].Rows)
            {
                var subaccountId = Convert.ToInt32(Null.SetNull(row["Id"], 0));
                var order = Convert.ToInt32(Null.SetNull(row["SubcuentaOrden"], 0));
                var headerText = Null.SetNull(row["TextoEncabezado"], string.Empty).ToString();
                var detailText = Null.SetNull(row["TextoDetalle"], string.Empty).ToString();

                var subaccount = ContractContributionsSubaccountText.Create(subaccountId, order, headerText, detailText);
                subaccountList.Add(subaccount);
            }
            return subaccountList;
        }
    }
}
