using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class RegisterUserCommand: IRequest<RegisterUserResult>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RegisterUserResult
{
    public bool UserCreated { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
}

public class RegisterUserUseCase : IRequestHandler<RegisterUserCommand, RegisterUserResult>
{
    private readonly IUserStorage userStorage;
    private readonly IPasswordHasher passwordHasher;

    public RegisterUserUseCase(IUserStorage _userStorage, IPasswordHasher _passwordHasher)
    {
        userStorage = _userStorage;
        passwordHasher = _passwordHasher;
    }

    public async Task<RegisterUserResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await userStorage.UserExists(request.Username))
        {
            return new RegisterUserResult {UserCreated = false, Message = "User already exists."};
        }
        var hashedPassword = passwordHasher.HashPassword(request.Password);
        var user = new User(request.Username, hashedPassword);
        await userStorage.AddAsync(user);
        return new RegisterUserResult {UserCreated = true, Message = "User created successfully.", Username = user.Username};
    }
}
