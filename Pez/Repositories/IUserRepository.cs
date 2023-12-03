using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IUserRepository : IBaseRepository<Users, Guid>
    {
        Task<Users> GetUserAsync(string mobile);
        Task<Users> GetUserAsync(string mobile, string password);
        Task<UserInfo> getUserInfoAsync(string mobile);
        Task<UserInfo> getUserInfoAsync(Guid userId);
        Task<Guid> GetUserIdByMobileAsync(string mobile);
        Task<List<Addresses>> GetUserAddressesAsync(Guid userId);
        Task<Addresses> GetAddressAsync(Guid addressId);
        Task<Addresses> GetAddressAsync(Guid addressId, Guid userId);
        Task<bool> IsUserExistAsync(string mobile);

    }
}