using DataLayer.Models;

namespace Pezeshkafzar_v2.Login;
public interface IJwtProvider
{
    string Generate(Users user);
}
