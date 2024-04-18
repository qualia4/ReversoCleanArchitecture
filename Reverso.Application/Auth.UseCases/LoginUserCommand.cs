using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class LoginUserCommand: IRequest<LoginUserResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginUserResult
{
    public bool UserExists { get; set; }
    public bool UserLogedIn { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
}

public class LoginUserUseCase : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly IUserStorage userStorage;
    private readonly IPasswordHasher passwordHasher;

    public LoginUserUseCase(IUserStorage _userStorage, IPasswordHasher _passwordHasher)
    {
        userStorage = _userStorage;
        passwordHasher = _passwordHasher;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (!await userStorage.UserExists(request.Username))
        {
            return new LoginUserResult {UserExists = false, Message = "User doesn't exist"};
        }
        var hashedPassword = passwordHasher.HashPassword(request.Password);
        var user = userStorage.FindByUsernameAsync(request.Username).Result;
        if (user.ComparePassword(hashedPassword))
        {
            return new LoginUserResult {UserExists = true, UserLogedIn = true, Username = user.Username};
        }
        return new LoginUserResult {UserLogedIn = false, Message = "Invalid password or username"};
    }
}