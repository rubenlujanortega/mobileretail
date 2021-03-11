using System;
using System.Collections.Generic;
using System.Text;
using ClientProducts.Domain.ProductsOfClienteAggregate;
using ClientProducts.Domain.ContractDetailAggregate;
using ClientProducts.Domain.Contributions;
using ClientProducts.Domain.Contributions;
using ClientProducts.Message;
using Common.Domain;
using prxyNotsTypes = ClientProducts.Repository.NotificationsProxyTypes;

namespace ClientProducts.Application
{
    public interface IClientProductsApp
    {
        ProductsOfClient GetClientProducts(string clientIdentifier, out OperationResult result);
        Contract GetContractDetail(string contractId, out OperationResult result);
        List<ContractContributionsSubaccountText> GetContributionsSubaccountsTexts(string productId, out OperationResult result);
        List<ContractBankAccount> GetContractBankAccounts(string contractId, out OperationResult result);
        ContractContributionsInfo GetContractContributionsInfo(string contractId, out OperationResult result);
        ClientOTP GenerateNewOTP(string userId, out OperationResult result);
        OperationResult GetOTPValidation(string userName, string pin, out bool valid, out bool expired);

    }
}
