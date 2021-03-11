using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SqlClient.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using ClientProducts.Repository;

namespace ClientProducts.RepositoryTest
{
    [TestClass]
    public class NotificationsProxyTest
    {
        private readonly NotificationsProxy _notificationsProxyRepository;

        public NotificationsProxyTest()
        {
            _notificationsProxyRepository = new NotificationsProxy();
        }

        [TestMethod]
        public void NotificationsAppSyncNullError()
        {
            Assert.ThrowsException<NullReferenceException>(() => _notificationsProxyRepository.NotificationsAppSync(new NotificationsProxyTypes.SendNotificationDistListRequest()));
        }

        [TestMethod]
        public void NotificationsAppSyncSuccess()
        {
            NotificationsProxyTypes.SendNotificationDistListRequest request = new NotificationsProxyTypes.SendNotificationDistListRequest()
            {
                ContextBusiness = new NotificationsProxyTypes.ContextBusinessN()
                {
                    Application = "Portal Retail",
                    Process = "NOTIFICACION ACTUALIZACION DATOS CONTRATO",
                    ClientIdentifier = "000000000",
                    ClientMobile = "",
                    Contract = "00000000",
                    ClientEmail = "test@skandia.com.mx",
                },
                SendParameters = new NotificationsProxyTypes.SendParametersN()
                {
                    Amount = 0,
                    ClientIdentifier = "000000000",
                    ClientTypeId = "C",
                    Contract = "00000000",
                    EmailFP = "",
                    MobileClient = "",
                    MobileFP = "",
                    EmailClient = "test@skandia.com.mx",
                    AdditionalMessages = new System.Collections.Generic.Dictionary<string, string>()
                },
                Plantilla = NotificationsProxyTypes.NotificationTemplateN.Default,
                LstEmail = new List<string>().ToArray()
            };
            var result = _notificationsProxyRepository.NotificationsAppSync(request);
            Assert.IsNotNull(result);
        }
    }
}
