using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IOrderRepository : IBaseRepository<Orders,Guid>
    {
        Task<List<Orders>> GetUserOrderAsync(Guid userId, CancellationToken cancellationToken);
        Task<List<OrderDetails>> GetOrderDetailsAsync(Guid orderId, CancellationToken cancellationToken);
        Task<List<string>> GetTrackingCodeListByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> IsOrderFinalAsync(Guid orderId,CancellationToken cancellationToken);
        Task<Orders> GetOrderByTrackingCodeAsync(string trackingCode,  CancellationToken cancellationToken);
        Task<List<DeliveryWays>> GetDeliveryWaysAsync(CancellationToken cancellationToken);
        Task<DeliveryWays> GetDeliveryWayAsync(Guid deliveryWayId,CancellationToken cancellationToken);
    }
}