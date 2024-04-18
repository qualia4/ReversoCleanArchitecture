using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class GetAllUsersCommand: IRequest<GetAllUsersResult>
{

}

public class GetAllUsersResult
{
    public bool UsersEmpty { get; set; }
    public List<User> Users { get; set; }
}

public class GetAllUsersUseCase : IRequestHandler<GetAllUsersCommand, GetAllUsersResult>
{
    private readonly IUserStorage userStorage;

    public GetAllUsersUseCase(IUserStorage _userStorage)
    {
        userStorage = _userStorage;
    }

    public async Task<GetAllUsersResult> Handle(GetAllUsersCommand request, CancellationToken cancellationToken)
    {
        if (await userStorage.IsUsersEmpty())
        {
            return new GetAllUsersResult {UsersEmpty = true};
        }
        return new GetAllUsersResult {UsersEmpty = false, Users = userStorage.GetAllUsers().Result};
    }
}