namespace Domain.Game;
using Abstractions;
using System;

public class ReversoGameWithEvents : ReversoGame
{
    public event Action? GameStarted;

    public event Action<Cell[,]>? FieldUpdated;

    public event Action<(string, int)?>? GameEnded;

    public event Action<Dictionary<string, int>>? PointsUpdated;

    public override void StartGame(Player firstPlayer, Player secondPlayer)
    {
        base.StartGame(firstPlayer, secondPlayer);
        FieldUpdated?.Invoke(GetField());
        PointsUpdated?.Invoke(GetPoints());
        GameStarted?.Invoke();
    }

    protected override void ChangeField()
    {
        base.ChangeField();
        FieldUpdated?.Invoke(GetField());
        PointsUpdated?.Invoke(GetPoints());
    }

    protected override void EndGame()
    {
        base.EndGame();
        GameEnded?.Invoke(GetWinner());
    }


}