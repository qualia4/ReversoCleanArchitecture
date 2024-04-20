namespace Domain.Game;
using Abstractions;
using System;

public class ReversoGameWithEvents : ReversoGame
{
    public event Action? GameStarted;
    public event Action? CanMakeMove;
    public event Action<Cell[,]>? FieldUpdated;

    public event Action<(string, int)?>? GameEnded;

    public event Action<Dictionary<string, int>>? PointsUpdated;

    public override async Task StartGame(Player firstPlayer, Player secondPlayer)
    {
        await base.StartGame(firstPlayer, secondPlayer);
        FieldUpdated?.Invoke(GetField());
        PointsUpdated?.Invoke(GetPoints());
        GameStarted?.Invoke();
        await Task.Run(() => CanMakeMove?.Invoke());
    }

    public override async Task MakeMove()
    {
        await base.MakeMove();
        if(!GetEnded())
            await Task.Run(() => CanMakeMove?.Invoke());
    }

    protected override async Task ChangeField()
    {
        await base.ChangeField();
        FieldUpdated?.Invoke(GetField());
        PointsUpdated?.Invoke(GetPoints());
    }

    protected override async Task EndGame()
    {
        await base.EndGame();
        await Task.Run(() => GameEnded?.Invoke(GetWinner()));
    }
}