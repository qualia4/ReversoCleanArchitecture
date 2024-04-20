namespace Domain.Game;
using Abstractions;

public class ReversoGame : ITwoPlayerGame
{
    private Player? FirstPlayer;
    private Player? SecondPlayer;
    private Player? CurrentPlayer { get; set; }
    private ReversoField ReversoGameField { get; } = new ReversoField();
    public bool Ended { get; private set; }
    public Cell[,] GetField() => ReversoGameField.GetCells();

    public virtual async Task MakeMove()
    {
        CheckGameStarted();
        await ChangeField();
        CheckGameEnd();
    }

    protected virtual async Task ChangeField()
    {
        CheckGameStarted();
        int pointsToChange = await CurrentPlayer.MakeMoveOnField(ReversoGameField);
        RedistributePoints(pointsToChange);
        SwitchPlayer();
        ReversoGameField.ChangeValid(CurrentPlayer);
    }

    public virtual Task StartGame(Player firstPlayer, Player secondPlayer)
    {
        Ended = false;
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
        firstPlayer.ResetPoints();
        secondPlayer.ResetPoints();
        CurrentPlayer = firstPlayer;
        ReversoGameField.Initialize(firstPlayer, secondPlayer);
        return Task.CompletedTask;
    }

    private void CheckGameEnd()
    {
        if (ReversoGameField.HasValidMoves())
        {
            return;
        }

        EndGame();
    }

    public Dictionary<string, int> GetPoints()
    {
        CheckGameStarted();
        var players = new Dictionary<string, int>()
        {
            {FirstPlayer.GetName(), FirstPlayer.GetPoints()},
            {SecondPlayer.GetName(), SecondPlayer.GetPoints()}
        };
        return players;
    }

    protected virtual Task EndGame()
    {
        CheckGameStarted();
        Ended = true;
        return Task.CompletedTask;
    }

    private void RedistributePoints(int points)
    {
        if (CurrentPlayer == FirstPlayer)
        {
            SecondPlayer?.RemovePoints(points);
            FirstPlayer?.AddPoints(points);
            return;
        }

        FirstPlayer?.RemovePoints(points);
        SecondPlayer?.AddPoints(points);
    }

    public (string, int)? GetWinner()
    {
        CheckGameStarted();
        Player? winner = (FirstPlayer?.GetPoints() > SecondPlayer?.GetPoints()) ? FirstPlayer :
            (SecondPlayer?.GetPoints() > FirstPlayer?.GetPoints()) ? SecondPlayer : null;
        if (winner == null)
        {
            return null;
        }
        (string, int) winnerData = (winner.GetName(), winner.GetPoints());
        return winnerData;
    }

    private void SwitchPlayer()
    {
        CurrentPlayer = CurrentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
    }

    private void CheckGameStarted()
    {
        if (FirstPlayer == null || SecondPlayer == null)
        {
            throw new Exception("Game has not been started");
        }
    }

    public bool GetEnded()
    {
        return Ended;
    }

    public string GetCurrentPlayerName()
    {
        CheckGameStarted();
        return CurrentPlayer.GetName();
    }

}