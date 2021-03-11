using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProducts.Repository
{
    public class NotificationsProxyTypes
    {
        public class ContextBusinessN
        {
            public string Application;
            public string Process;
            public string Contract;
            public string ClientEmail;
            public string ClientMobile;
            public string ClientIdentifier;
        }

        public class SendParametersN
        {
            public string ClientTypeId;
            public string ClientIdentifier;
            public string Contract;
            public decimal Amount;
            public string MobileClient;
            public string MobileFP;
            public string EmailClient;
            public string EmailFP;
            public Dictionary<string, string> AdditionalMessages;
        }

        public class SendNotificationDistListRequest
        {
            public ContextBusinessN ContextBusiness;
            public SendParametersN SendParameters;
            public NotificationTemplateN Plantilla;
            public string[] LstEmail;
        }

        public enum NotificationTemplateN : int
        {
            Default = 0,
            Notificacion_Actualizacion_Correo_Retail,
            Solvencia_Pin_OTP
        }
    }
}
