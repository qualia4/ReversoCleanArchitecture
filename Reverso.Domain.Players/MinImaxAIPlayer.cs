namespace Domain.Players;
using Abstractions;
using System.Linq;

public class MinimaxAIPlayer : Player
{
    private readonly Random rand = new Random();
    private readonly bool makeDelay;
    private const int MaxDepth = 3;

    public MinimaxAIPlayer(string name, bool makeDelay = true) : base(name)
    {
        this.makeDelay = makeDelay;
    }

    public override Task<int> MakeMoveOnField(IGameField gameField)
    {
        var bestMove = Minimax(gameField, MaxDepth, true, int.MinValue, int.MaxValue).Move;
        if (makeDelay)
        {
            int delay = rand.Next(1, 2);
            Thread.Sleep((int)TimeSpan.FromSeconds(delay).TotalMilliseconds);
        }
        if (bestMove != null)
        {
            return Task.FromResult(gameField.ChangeField(bestMove.Value.xCoord, bestMove.Value.yCoord, this));
        }
        throw new InvalidOperationException("No valid moves available");
    }

    private (int Score, (int xCoord, int yCoord)? Move) Minimax(IGameField gameField, int depth, bool maximizingPlayer, int alpha, int beta)
    {
        Player? opponent = GetOpponent(gameField);
        (int xCoord, int yCoord)? bestMove = null;
        if (depth == 0 || !gameField.HasValidMoves() || opponent == null)
        {
            return (EvaluateBoard(gameField, this), bestMove);
        }

        var possibleMoves = GetAllValidMoves(gameField);

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            foreach (var (x, y) in possibleMoves)
            {
                var simulationGameField = gameField.DeepClone();
                simulationGameField.ChangeField(x, y, this);
                simulationGameField.ChangeValid(opponent);
                var eval = Minimax(simulationGameField, depth - 1, false, alpha, beta).Score;
                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestMove = (x, y);
                }
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }
            return (maxEval, bestMove);
        }
        else
        {
            int minEval = int.MaxValue;
            foreach (var (x, y) in possibleMoves)
            {
                var simulationGameField = gameField.DeepClone();
                simulationGameField.ChangeField(x, y, opponent);
                simulationGameField.ChangeValid(this);
                var eval = Minimax(simulationGameField, depth - 1, true, alpha, beta).Score;
                if (eval < minEval)
                {
                    minEval = eval;
                    bestMove = (x, y);
                }
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                    break;
            }
            return (minEval, bestMove);
        }
    }


    private IEnumerable<(int x, int y)> GetAllValidMoves(IGameField gameField)
    {
        List<(int, int)> moves = new List<(int, int)>();
        for (int x = 0; x < gameField.GetSize(); x++)
        {
            for (int y = 0; y < gameField.GetSize(); y++)
            {
                if (gameField.IsValidCell(x, y))
                    moves.Add((x, y));
            }
        }
        return moves;
    }

    private int EvaluateBoard(IGameField gameField, Player player)
    {
        int score = 0;
        for (int x = 0; x < gameField.GetSize(); x++)
        {
            for (int y = 0; y < gameField.GetSize(); y++)
            {
                var cellOwner = gameField.GetHost(x, y);
                if (cellOwner != null)
                {
                    score += (cellOwner == player) ? 1 : -1;
                }
            }
        }
        return score;
    }

    private Player? GetOpponent(IGameField gameField)
    {
        for (int i = 0; i < gameField.GetSize(); i++)
        {
            for (int j = 0; j < gameField.GetSize(); j++)
            {
                Player? potentialOpponent = gameField.GetHost(i, j);
                if (potentialOpponent != null && potentialOpponent != this)
                {
                    return potentialOpponent;
                }
            }
        }
        return null;
    }
}
