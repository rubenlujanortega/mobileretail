using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ClientProducts.Domain.Contributions;
using ClientProducts.Domain.ContractDetailAggregate;
using NumericValues.Helper;

namespace ClientProducts.Mapping
{
    public static class ContractContributionSubaccountMapp
    {
        public static List<ContractContributionSubaccount> GetContractContributionsSubaccountsTextMapping(DataSet ds, OffspringGrouperContract subaccountsContracts)
        {
            List<ContractContributionSubaccount> subaccountList = new List<ContractContributionSubaccount>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var subaccountId = Convert.ToInt32(Null.SetNull(row["Id"], 0));
                var order = Convert.ToInt32(Null.SetNull(row["SubcuentaOrden"], 0));
                var headerText = Null.SetNull(row["TextoEncabezado"], string.Empty).ToString();
                var detailText = Null.SetNull(row["TextoDetalle"], string.Empty).ToString();

                var subaccount = ContractContributionSubaccount.Create(subaccountId, order, headerText, detailText, subaccountsContracts);
                subaccountList.Add(subaccount);
            }
            return subaccountList;
        }
    }
}
