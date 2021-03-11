using Common.Domain;

namespace ClientProducts.Repository
{
    public interface INotificationsProxy
    {
        OperationResult NotificationsAppSync(NotificationsProxyTypes.SendNotificationDistListRequest request);
    }
}
