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
    public static class DomiciliationMapp
    {
        public static Domiciliation DomiciliationMapping(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            var encryptAccount = Null.SetNull(row["Contrato_Cta_Num"].ToString(), string.Empty).ToString();
            var elementCounter = encryptAccount.Length - 4;
            encryptAccount = encryptAccount.Remove(0, encryptAccount.Length - 4);
            for (int element = 0; element < elementCounter; element++)
            {
                encryptAccount = "*" + encryptAccount;
            }
            var amount = Convert.ToDecimal(Null.SetNull(row["Cobro_Monto"].ToString(), new decimal()));
            var payDay = Convert.ToInt32(Null.SetNull(row["Cobro_Dia_Mes"].ToString(), new Int32()));

            return Domiciliation.Create(encryptAccount, payDay, amount);
        }

        public static Domiciliation DomiciliationMappingRecurring(DataSet ds)
        {
            var rows = ds.Tables[0].Select("Sts_Domiciliacion_Id = 5");
            if (rows.Count() > 0)
            {
                var row = rows[0];
                var encryptAccount = Null.SetNull(row["Cuenta_Id_Cliente"].ToString(), string.Empty).ToString();
                var elementCounter = encryptAccount.Length - 4;
                encryptAccount = encryptAccount.Remove(0, encryptAccount.Length - 4);
                for (int element = 0; element < elementCounter; element++)
                {
                    encryptAccount = "*" + encryptAccount;
                }
                if(encryptAccount.Length>16)
                {
                    encryptAccount = encryptAccount.Substring(3, encryptAccount.Length - 3);
                }
                var amount = Convert.ToDecimal(Null.SetNull(row["Domiciliacion_Importe"].ToString(), new decimal()));
                var payDay = Convert.ToInt32(Null.SetNull(row["Dia_Cobro"].ToString(), new Int32()));
                return Domiciliation.Create(encryptAccount, payDay, amount);
            }
            else
            {
                return DomiciliationMapp.DomiciliationMappingEmpty();
            }
        }

        public static Domiciliation DomiciliationMappingEmpty()
        {
            return Domiciliation.Create("", 0, 0);
        }
    }
}
