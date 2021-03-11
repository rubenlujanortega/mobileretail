using System;
using System.Data;
using ClientProducts.Domain.ContractDetailAggregate;

namespace ClientProducts.Mapping
{
    public static class SavingInfoMapp
    {
        public static SavingInformation SavingInfoMapping(ContractSOCData contracSOCInfo)
        {
            SavingInformation savingInfo = new SavingInformation(contracSOCInfo);
            return savingInfo;
        }
    }
}
