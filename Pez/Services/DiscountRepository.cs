using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services
{
    public class DiscountRepository : BaseRepository<Discounts, Guid>, IDiscountRepository
    {
        private readonly IUserRepository _userRepository;
        public DiscountRepository(IHttpContextAccessor accessor, ApplicationDBContext context, IUserRepository userRepository) : base(accessor, context)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Discounts>> GetDiscountsWithUserAsync()
            => await Entities
                .Include(x=>x.Users)
                .ToListAsync();

        public async Task<Discounts> VerifyDisountCodeAsync(string discountCode, string mobile)
        {
            var user = await _userRepository.GetUserAsync(mobile);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");

            var discount = await Entities.FirstOrDefaultAsync(x => !x.IsUsed && x.UserId == user.Id && x.DiscountCode == discountCode && x.ExpireDate > DateTime.Now);
            if (discount == null)
                throw new Exception("کد تخفیف وجود ندارد یا قابل استفاده نیست.");

            return discount;
        }
    }
}
