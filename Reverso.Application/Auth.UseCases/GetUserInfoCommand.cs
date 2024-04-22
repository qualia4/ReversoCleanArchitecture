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
    public UserToShow User { get; set; }
}

public class UserToShow
{
    public UserToShow(User user)
    {
        Username = user.Username;
        GamesPlayed = user.GamesPlayed;
        Draws = user.Draws;
        GamesWon = user.GamesWon;
        GamesLost = user.GamesLost;
    }
    public string Username { get; private set; }
    public int GamesPlayed { get; private set; }
    public int Draws { get; private set; }
    public int GamesWon { get; private set; }
    public int GamesLost { get; private set; }
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

        UserToShow user = new UserToShow(await userStorage.GetUserByUsername(request.Username));
        return new GetUserInfoResult() {UserFound = true, User = user};
    }
}