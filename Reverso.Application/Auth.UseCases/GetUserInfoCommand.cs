using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class GetUserInfoCommand: IRequest<GetUserInfoResult>
{
    public string Username { get; set; }
}

public class GetUserInfoResult
{
    public bool UserFound { get; set; }
    public User User { get; set; }
}

public class GetUserInfoUseCase : IRequestHandler<GetUserInfoCommand, GetUserInfoResult>
{
    private readonly IUserStorage userStorage;

    public GetUserInfoUseCase(IUserStorage _userStorage)
    {
        userStorage = _userStorage;
    }

    public async Task<GetUserInfoResult> Handle(GetUserInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await userStorage.UserExists(request.Username))
        {
            return new GetUserInfoResult(){UserFound = false};
        }
        return new GetUserInfoResult() {UserFound = true, User = await userStorage.GetUserByUsername(request.Username)};
    }
}