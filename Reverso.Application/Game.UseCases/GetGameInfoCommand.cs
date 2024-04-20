using Domain.Game;
using MediatR;
using Reverso.Domain.Web;

namespace Application;

public class GetGameInfoCommand: IRequest<GetGameInfoResult>
{
    public Guid LobbyId { get; set; }
}

public class GetGameInfoResult
{
    public bool LobbyExists { get; set; }
    public bool LobbyStarted { get; set; }
    public string[][] Field { get; set; }
    public Dictionary<string, int> Points { get; set; }
    public bool GameEnded { get; set; }
    public string CurrentPlayerName { get; set; }
}

public class GetGameInfoUseCase : IRequestHandler<GetGameInfoCommand, GetGameInfoResult>
{
    private readonly ILobbyStorage lobbyStorage;

    public GetGameInfoUseCase(ILobbyStorage _lobbyStorage)
    {
        lobbyStorage = _lobbyStorage;
    }

    public async Task<GetGameInfoResult> Handle(GetGameInfoCommand request, CancellationToken cancellationToken)
    {
        if (!lobbyStorage.LobbyExists(request.LobbyId).Result)
            return new GetGameInfoResult() {LobbyExists = false};
        if (!lobbyStorage.IsLobbyStarted(request.LobbyId).Result)
            return new GetGameInfoResult() {LobbyExists = true, LobbyStarted = false};
        string[][] gameField = ConvertFieldToString(await lobbyStorage.GetFieldByLobbyId(request.LobbyId));
        var points = await lobbyStorage.GetPointsByLobbyId(request.LobbyId);
        bool ended = await lobbyStorage.CheckIfEndedByLobbyId(request.LobbyId);
        if (ended)
            return new GetGameInfoResult()
                {LobbyExists = true, LobbyStarted = true, Field = gameField, Points = points, GameEnded = ended};
        string currentPlayerName = await lobbyStorage.GetCurrentPlayerNameByLobbyId(request.LobbyId);
        return new GetGameInfoResult()
            {LobbyExists = true, LobbyStarted = true, Field = gameField, Points = points, GameEnded = ended, CurrentPlayerName = currentPlayerName};
    }

    private string[][] ConvertFieldToString(Cell[,] field)
    {
        int fieldSize = field.GetLength(0);
        Console.WriteLine(fieldSize);
        string[][] parsedField = new string[fieldSize][];
        for (int i = 0; i < fieldSize; i++)
        {
            parsedField[i] = new string[fieldSize];
            for (int j = 0; j < fieldSize; j++)
            {
                if (field[i, j].IfEmpty)
                {
                    parsedField[i][j] = field[i, j].IfValid ? "*" : "-";
                }
                else
                {
                    parsedField[i][j] = field[i, j].GetHost().GetName();
                }
            }
        }
        return parsedField;
    }
}