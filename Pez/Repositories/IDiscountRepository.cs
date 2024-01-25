using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IDiscountRepository : IBaseRepository<Discounts,Guid>
    {
        Task<Discounts> VerifyDisountCodeAsync(string discountCode, string mobile);
        Task<List<Discounts>> GetDiscountsWithUserAsync();
        
    }
}