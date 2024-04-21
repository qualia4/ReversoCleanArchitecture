namespace Domain.Game;
using Abstractions;
using System;

public class ReversoGameWithEvents : ReversoGame
{
    public event Action? CanMakeMove;
    public event Action<Dictionary<string, int>?>? GameEnded;

    public override async void StartGame(Player firstPlayer, Player secondPlayer)
    {
        base.StartGame(firstPlayer, secondPlayer);
        await Task.Run(() => CanMakeMove?.Invoke());
    }

    public override async Task MakeMove()
    {
        await base.MakeMove();
        if(!GetEnded())
            await Task.Run(() => CanMakeMove?.Invoke());
    }

    protected override async Task EndGame()
    {
        await base.EndGame();
        await Task.Run(() => GameEnded?.Invoke(GetPoints()));
    }
}