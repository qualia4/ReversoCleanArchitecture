using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class WriteMessageCommand: IRequest<WriteMessageResult>
{
    public Guid LobbyId { get; set; }
    public string Username { get; set; }
    public string Text { get; set; }
}

public class WriteMessageResult
{
    public bool LobbyExists { get; set; }
    public bool UserExists { get; set; }
}

public class WriteMessageUseCase : IRequestHandler<WriteMessageCommand, WriteMessageResult>
{
    private readonly ILobbyStorage lobbyStorage;

    public WriteMessageUseCase(ILobbyStorage _lobbyStorage)
    {
        lobbyStorage = _lobbyStorage;
    }

    public async Task<WriteMessageResult> Handle(WriteMessageCommand request, CancellationToken cancellationToken)
    {
        if (!await lobbyStorage.LobbyExists(request.LobbyId))
        {
            return new WriteMessageResult() {LobbyExists = false};
        }
        bool messageWritten = await lobbyStorage.AddMessageByLobbyId(request.LobbyId, request.Username, request.Text);
        return new WriteMessageResult() {LobbyExists = true, UserExists = messageWritten};
    }
}