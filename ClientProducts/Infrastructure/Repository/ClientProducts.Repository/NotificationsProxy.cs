using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;
using WSFSOC = Notifications.WSFSOC;
using System.Configuration;

namespace ClientProducts.Repository
{
    public class NotificationsProxy : INotificationsProxy
    {
        public OperationResult NotificationsAppSync(NotificationsProxyTypes.SendNotificationDistListRequest sendRequest)
        {
            OperationResult result = new OperationResult();

            WSFSOC.NotificationTemplateN reqTemplate;
            Enum.TryParse(Enum.GetName(typeof(NotificationsProxyTypes.NotificationTemplateN), sendRequest.Plantilla), out reqTemplate);
            string endPoint = ConfigurationManager.AppSettings["NotificationsAppServiceContractClient"].ToString();
            WSFSOC.NotificationsAppServiceContractClient notClient = new WSFSOC.NotificationsAppServiceContractClient(WSFSOC.NotificationsAppServiceContractClient.EndpointConfiguration.NotificationsAppServiceEndpoint, endPoint);
            WSFSOC.SendNotificationDistListNRequest request = new WSFSOC.SendNotificationDistListNRequest
            {
                ContextBusinessN = new WSFSOC.ContextBusinessN
                {
                    Application = sendRequest.ContextBusiness.Application,
                    ClientEmail = sendRequest.ContextBusiness.ClientEmail,
                    ClientIdentifier = sendRequest.ContextBusiness.ClientIdentifier,
                    ClientMobile = sendRequest.ContextBusiness.ClientMobile,
                    Contract = sendRequest.ContextBusiness.Contract,
                    Process = sendRequest.ContextBusiness.Process,
                },
                SendParametersN = new WSFSOC.SendParametersN
                {
                    AdditionalMessages = sendRequest.SendParameters.AdditionalMessages,
                    Amount = sendRequest.SendParameters.Amount,
                    ClientIdentifier = sendRequest.SendParameters.ClientIdentifier,
                    ClientTypeId = sendRequest.SendParameters.ClientTypeId,
                    Contract = sendRequest.SendParameters.Contract,
                    EmailClient = sendRequest.SendParameters.EmailClient,
                    EmailFP = sendRequest.SendParameters.EmailFP,
                    MobileClient = sendRequest.SendParameters.MobileClient,
                    MobileFP = sendRequest.SendParameters.MobileFP
                },
                LstEmail = sendRequest.LstEmail,
                Plantilla = reqTemplate
            };

            WSFSOC.SendNotificationDistListResponse response = notClient.SendNotificationDistListNAsync(request.ContextBusinessN, request.LstEmail, request.Plantilla, request.SendParametersN).GetAwaiter().GetResult();
            result.SystemMessages.Add(new SystemMessage { Message = notClient.Endpoint.ListenUri.ToString(), MessageType = (SystemMessageTypes.Information) });
            result.Successful = response.Result.Successful;
            foreach (WSFSOC.SystemMessage ms in response.Result.SystemMessages)
            {
                result.SystemMessages.Add(new SystemMessage { Message = ms.Message, MessageType = (SystemMessageTypes)((int)ms.MessageType) });
            }

            return result;
        }
    }
}
