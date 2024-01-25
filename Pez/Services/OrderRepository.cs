using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class OrderRepository : BaseRepository<Orders, Guid>, IOrderRepository
    {
        private readonly DbSet<DeliveryWays> _deliveryWays;
        private readonly DbSet<OrderDetails> _orderDetails;
        public OrderRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            _deliveryWays = context.Set<DeliveryWays>();
            _orderDetails = context.Set<OrderDetails>();
        }

        public async Task<DeliveryWays> GetDeliveryWayAsync(Guid deliveryWayId)
            => await _deliveryWays.FirstOrDefaultAsync(x => x.Id == deliveryWayId && x.IsActive);

        public async Task<List<DeliveryWays>> GetDeliveryWaysAsync()
            => await _deliveryWays.Where(x => x.IsActive).ToListAsync();

        public async Task<Orders> GetOrderByTrackingCodeAsync(string trackingCode)
            => await Entities.FirstOrDefaultAsync(x => x.TraceCode == trackingCode);

        public async Task<List<OrderDetails>> GetOrderDetailsAsync(Guid orderId)
            => await _orderDetails
            .Include(x => x.Orders)
            .Where(x => x.OrderID == orderId).ToListAsync();

        public async Task<List<Orders>> GetOrdersAsync()
            => await Entities.OrderByDescending(x => x.Date).ToListAsync();

        public async Task<List<string>> GetTrackingCodeListByUserIdAsync(Guid userId)
            => await Entities.Where(x => x.UserId == userId).Select(x => x.TraceCode).ToListAsync();

        public async Task<List<Orders>> GetUserOrderAsync(Guid userId)
            => await Entities.Where(x => x.UserId == userId).OrderByDescending(x => x.Date).ToListAsync();

        public async Task<Orders> GetWithChildrenAsync(Guid id)
            => await Entities
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> IsOrderFinalAsync(Guid orderId)
        {
            var order = await Entities.FindAsync(orderId);
            if (order == null)
                return false;

            return order.IsFinaly;
        }
    }
}
