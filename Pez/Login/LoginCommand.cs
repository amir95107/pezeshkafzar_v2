
using Pezeshkafzar_v2.Repositories;
using Pezeshkafzar_v2.Utilities;

namespace Pezeshkafzar_v2.Login;
public class LoginHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;
    public LoginHandler(
        IUserRepository userRepository,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> ReuqestAsync(LoginWithMobilePass request,CancellationToken cancellationToken)
    {
        var password = SecretHasher.Hash(request.Password);
        var user = await _userRepository.GetUserAsync(request.Mobile, password, cancellationToken);
        if (user == null)
            throw new Exception("موبایل یا رمز اشتباه است.");

        return _jwtProvider.Generate(user);
    }
}

public class LoginWithMobilePass
{
    public string Mobile { get; set; }
    public string Password { get; set; }
}

public class LoginWithOtp
{
    public string Mobile { get; set; }
}

