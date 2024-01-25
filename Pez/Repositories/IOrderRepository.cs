using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IOrderRepository : IBaseRepository<Orders,Guid>
    {
        Task<Orders> GetWithChildrenAsync(Guid id);
        Task<List<Orders>> GetUserOrderAsync(Guid userId);
        Task<List<Orders>> GetOrdersAsync();
        Task<List<OrderDetails>> GetOrderDetailsAsync(Guid orderId);
        Task<List<string>> GetTrackingCodeListByUserIdAsync(Guid userId);
        Task<bool> IsOrderFinalAsync(Guid orderId);
        Task<Orders> GetOrderByTrackingCodeAsync(string trackingCode);
        Task<List<DeliveryWays>> GetDeliveryWaysAsync();
        Task<DeliveryWays> GetDeliveryWayAsync(Guid deliveryWayId);
    }
}