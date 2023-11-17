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

        public async Task<DeliveryWays> GetDeliveryWayAsync(Guid deliveryWayId, CancellationToken cancellationToken)
            => await _deliveryWays.FirstOrDefaultAsync(x => x.Id == deliveryWayId && x.IsActive, cancellationToken);

        public async Task<List<DeliveryWays>> GetDeliveryWaysAsync(CancellationToken cancellationToken)
            => await _deliveryWays.Where(x => x.IsActive).ToListAsync();

        public async Task<Orders> GetOrderByTrackingCodeAsync(string trackingCode, CancellationToken cancellationToken)
            => await Entities.FirstOrDefaultAsync(x => x.TraceCode == trackingCode, cancellationToken);

        public async Task<List<OrderDetails>> GetOrderDetailsAsync(Guid orderId, CancellationToken cancellationToken)
            => await _orderDetails.Where(x => x.OrderID == orderId).ToListAsync(cancellationToken);

        public async Task<List<string>> GetTrackingCodeListByUserIdAsync(Guid userId, CancellationToken cancellationToken)
            => await Entities.Where(x => x.UserID == userId).Select(x => x.TraceCode).ToListAsync(cancellationToken);

        public async Task<List<Orders>> GetUserOrderAsync(Guid userId, CancellationToken cancellationToken)
            => await Entities.Where(x => x.UserID == userId).ToListAsync(cancellationToken);

        public async Task<bool> IsOrderFinalAsync(Guid orderId, CancellationToken cancellationToken)
        {
            var order = await Entities.FindAsync(orderId);
            if(order == null)
                return false;

            return order.IsFinaly;
        }
    }
}
