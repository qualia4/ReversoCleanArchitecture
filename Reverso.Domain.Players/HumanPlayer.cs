namespace Domain.Players;
using Abstractions;

public class HumanPlayer : Player
{
    private readonly IInputHandler _inputHandler;
    public HumanPlayer(string name, IInputHandler inputHandler) : base(name)
    {
        _inputHandler = inputHandler;
    }

    public async override Task<int> MakeMoveOnField(IGameField gameField)
    {
        int[] coordinates =  await _inputHandler.GetPlayerCoords(gameField);
        return gameField.ChangeField(coordinates[0], coordinates[1], this);
    }
}