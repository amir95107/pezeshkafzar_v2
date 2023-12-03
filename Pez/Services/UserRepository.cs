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

        public async Task<Addresses> GetAddressAsync(Guid addressId)
            => await _addresses.FirstOrDefaultAsync(x => x.Id == addressId);

        public async Task<Addresses> GetAddressAsync(Guid addressId, Guid userId)
        => await _addresses.FirstOrDefaultAsync(x => x.Id == addressId && x.UserId == userId);

        public async Task<List<Addresses>> GetUserAddressesAsync(Guid userId)
            => await _addresses.Include(x => x.Users).Where(x => x.UserId == userId).ToListAsync();

        public async Task<Users> GetUserAsync(string mobile)
            => await Entities.FirstOrDefaultAsync(x => x.PhoneNumber == mobile);

        public async Task<Users> GetUserAsync(string mobile, string password)
        {
            password = SecretHasher.Hash(password);
            var user = await Entities.FirstOrDefaultAsync(x => x.PhoneNumber == mobile && x.PasswordHash == password);
            if (user == null)
                throw new Exception("موبایل یا رمز عبور اشتباه است.");
            return user;
        }

        public async Task<Guid> GetUserIdByMobileAsync(string mobile)
            => await Entities.Where(x => x.PhoneNumber == mobile).Select(x => x.Id).FirstOrDefaultAsync();

        public async Task<UserInfo> getUserInfoAsync(string mobile)
        {
            var user = await Entities.Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.PhoneNumber == mobile);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");
            return user.UserInfo.FirstOrDefault();
        }

        public async Task<UserInfo> getUserInfoAsync(Guid userId)
        {
            var user = await Entities.Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new Exception("کاربر یافت نشد.");
            return user.UserInfo.FirstOrDefault();
        }

        public async Task<bool> IsUserExistAsync(string mobiole)
            => await Entities.AnyAsync(x => x.PhoneNumber == mobiole);

        Task<Addresses> IUserRepository.GetAddressAsync(Guid addressId)
        {
            throw new NotImplementedException();
        }

        Task<Addresses> IUserRepository.GetAddressAsync(Guid addressId, Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<List<Addresses>> IUserRepository.GetUserAddressesAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<Users> IUserRepository.GetUserAsync(string mobile)
        {
            throw new NotImplementedException();
        }

        Task<Users> IUserRepository.GetUserAsync(string mobile, string password)
        {
            throw new NotImplementedException();
        }

        Task<Guid> IUserRepository.GetUserIdByMobileAsync(string mobile)
        {
            throw new NotImplementedException();
        }

        Task<UserInfo> IUserRepository.getUserInfoAsync(string mobile)
        {
            throw new NotImplementedException();
        }

        Task<UserInfo> IUserRepository.getUserInfoAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserRepository.IsUserExistAsync(string mobile)
        {
            throw new NotImplementedException();
        }
    }
}
