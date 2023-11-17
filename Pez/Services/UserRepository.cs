using DataLayer.Data;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;

namespace Pezeshkafzar_v2.Services
{
    public class UserRepository : BaseRepository<Users, Guid>, IUserRepository
    {
        private readonly DbSet<Addresses> _addresses;
        public UserRepository(IHttpContextAccessor accessor, ApplicationDBContext context) : base(accessor, context)
        {
            _addresses = context.Set<Addresses>();
        }

        public async Task<Addresses> GetAddressAsync(Guid addressId, CancellationToken cancellationToken)
            => await _addresses.FirstOrDefaultAsync(x => x.Id == addressId, cancellationToken);

        public async Task<Addresses> GetAddressAsync(Guid addressId, Guid userId, CancellationToken cancellationToken)
        => await _addresses.FirstOrDefaultAsync(x => x.Id == addressId && x.UserID == userId,cancellationToken);

        public async Task<List<Addresses>> GetUserAddressesAsync(Guid userId, CancellationToken cancellationToken)
            => await _addresses.Include(x => x.Users).Where(x => x.UserID == userId).ToListAsync(cancellationToken);

        public async Task<Users> GetUserAsync(string mobile, CancellationToken cancellationToken)
            => await Entities.FirstOrDefaultAsync(x => x.Mobile == mobile, cancellationToken);

        public async Task<Users> GetUserAsync(string mobile, string password, CancellationToken cancellationToken)
        {
            password = SecretHasher.Hash(password);
            var user = await Entities.FirstOrDefaultAsync(x => x.Mobile == mobile && x.Password == password, cancellationToken);
            if (user == null)
                throw new Exception("موبایل یا رمز عبور اشتباه است.");
            return user;
        }

        public async Task<Guid> GetUserIdByMobileAsync(string mobile, CancellationToken cancellationToken)
            => await Entities.Where(x => x.Mobile == mobile).Select(x => x.Id).FirstOrDefaultAsync(cancellationToken);

        public async Task<UserInfo> getUserInfoAsync(string mobile, CancellationToken cancellationToken)
        {
            var user = await Entities.Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.Mobile == mobile, cancellationToken);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");
            return user.UserInfo.FirstOrDefault();
        }

        public async Task<UserInfo> getUserInfoAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Entities.Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");
            return user.UserInfo.FirstOrDefault();
        }
    }
}
