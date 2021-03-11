using System.Web.Http;
using System.Web.Http.Description;
using ClientProducts.Application;
using ClientProducts.Message;
using Common.Domain;

namespace MobileRetail.Api.Controllers
{
    /// <summary>
    /// ClientProductsController
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class ClientProductsController : ApiController
    {
        private readonly IClientProductsApp bL;

        /// <summary>
        /// Constrcutor
        /// </summary>
        /// <param name="clientProductsRetailApp"></param>
        public ClientProductsController(IClientProductsApp clientProductsRetailApp)
        {
            this.bL = clientProductsRetailApp;
        }

        /// <summary>
        /// Get the products with balance from a client
        /// </summary>
        /// <param name="request">data of the request</param>
        /// <returns>ParticipantResponse</returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(ClientProductsRequest))]
        [ActionName("GetClientProducts")]
        public ClientProductsResponse GetClientProducts(ClientProductsRequest request)
        {
            ClientProductsResponse response = new ClientProductsResponse();
            response.ClientProducts = bL.GetClientProducts(request.ClientIdentifier, out OperationResult result);
            response.Result = result;
            return response;
        }

        /// <summary>
        /// Get the contract detail information
        /// </summary>
        /// <param name="request">data of the request</param>
        /// <returns>ParticipantResponse</returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(ContractDetailRequest))]
        [ActionName("GetContractDetail")]
        public ContractDetailResponse GetContractDetail(ContractDetailRequest request)
        {
            ContractDetailResponse response = new ContractDetailResponse();
            response.ClientContractDetail = bL.GetContractDetail(request.ContractIdentifier, out OperationResult result);
            response.Result = result;
            return response;
        }

        ///// <summary>
        ///// Get the subaccounts texts for mobile app
        ///// </summary>
        ///// <param name="request">data of the request</param>
        ///// <returns>Subaccount List</returns>
        //[HttpPost]
        //[AllowAnonymous]
        //[ResponseType(typeof(ContributionsSubaccountsTextsRequest))]
        //[ActionName("GetContributionsSubaccountsTexts")]
        //public ContributionsSubaccountsTextsResponse GetContributionsSubaccountsTexts(ContributionsSubaccountsTextsRequest request)
        //{
        //    ContributionsSubaccountsTextsResponse response = new ContributionsSubaccountsTextsResponse();
        //    response.SubaccountsTexts = bL.GetContributionsSubaccountsTexts(request.ProductId, out OperationResult result);
        //    response.Result = result;
        //    return response;
        //}

        /// <summary>
        /// Get the contract's bank accounts 
        /// </summary>
        /// <param name="request">data of the request</param>
        /// <returns>Bank accounts List</returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(ContractBankAccountsRequest))]
        [ActionName("GetContractBankAccounts")]
        public ContractBankAccountsResponse GetContractBankAccounts(ContractBankAccountsRequest request)
        {
            ContractBankAccountsResponse response = new ContractBankAccountsResponse();
            response.ContractBankAccounts = bL.GetContractBankAccounts(request.ContractId, out OperationResult result);
            response.Result = result;
            return response;
        }

        /// <summary>
        /// Get the contract's DOM initial info
        /// </summary>
        /// <param name="request">data of the request</param>
        /// <returns>DOM Info</returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(ClientContractContributionsInfoRequest))]
        [ActionName("GetContractContributionsInfo")]
        public ClientContractContributionsInfoResponse GetContractContributionsInfo(ClientContractContributionsInfoRequest request)
        {
            ClientContractContributionsInfoResponse response = new ClientContractContributionsInfoResponse();
            response.ContractContributionsInfo = bL.GetContractContributionsInfo(request.ContractId, out OperationResult result);
            response.Result = result;
            return response;
        }

        // <summary>
        // Create a new OTP for validate user identity
        // </summary>
        // <param name = "request" > data of the request</param>
        // <returns>PIN six digits</returns>
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(GenerateNewOTPRequest))]
        [ActionName("GenerateNewOTP")]
        public GenerateNewOTPResponse GenerateNewOTP(GenerateNewOTPRequest request)
        {
            GenerateNewOTPResponse response = new GenerateNewOTPResponse();
            response.OTPData = bL.GenerateNewOTP(request.UserId, out OperationResult result);
            response.Result = result;
            return response;
        }

        // <summary>
        // Validate a PIN
        // </summary>
        // <param name = "request" > GetNewOTPRequest </ param >
        [HttpPost]
        [AllowAnonymous]
        [ResponseType(typeof(GetOTPValidationResponse))]
        [ActionName("GetOTPValidation")]
        public GetOTPValidationResponse GetOTPValidation(GetOTPValidationRequest request)
        {
            return new GetOTPValidationResponse
            {
                Result = bL.GetOTPValidation(request.UserId, request.PIN, out bool valid, out bool expired),
                PinValid = valid,
                PinExpired = expired
            };
        }
    }
}
