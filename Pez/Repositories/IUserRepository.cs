using DataLayer.Models;

namespace Pezeshkafzar_v2.Repositories
{
    public interface IUserRepository : IBaseRepository<Users, Guid>
    {
        Task<Users> GetUserAsync(string mobile, CancellationToken cancellationToken);
        Task<Users> GetUserAsync(string mobile, string password, CancellationToken cancellationToken);
        Task<UserInfo> getUserInfoAsync(string mobile, CancellationToken cancellationToken);
        Task<UserInfo> getUserInfoAsync(Guid userId, CancellationToken cancellationToken);
        Task<Guid> GetUserIdByMobileAsync(string mobile, CancellationToken cancellationToken);
        Task<List<Addresses>> GetUserAddressesAsync(Guid userId, CancellationToken cancellationToken);
        Task<Addresses> GetAddressAsync(Guid addressId, CancellationToken cancellationToken);
        Task<Addresses> GetAddressAsync(Guid addressId, Guid userId, CancellationToken cancellationToken);

    }
}