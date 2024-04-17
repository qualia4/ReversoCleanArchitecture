namespace Domain.Players;
using Abstractions;

public class AIPlayer: Player
{
    private readonly Random rand = new Random();
    private readonly bool makeDelay;

    public AIPlayer(string name, bool makeDelay = true) : base(name)
    {
        this.makeDelay = makeDelay;
    }

    public override int MakeMoveOnField(IGameField gameReversoField)
    {
        int[] coordinates = GenerateRandomMove(gameReversoField);
        return gameReversoField.ChangeField(coordinates[0], coordinates[1], this);
    }

    private int[] GenerateRandomMove(IGameField gameReversoField)
    {
        while (true)
        {
            int x = rand.Next(0, gameReversoField.GetSize());
            int y = rand.Next(0, gameReversoField.GetSize());
            if (gameReversoField.IsValidCell(x, y))
            {
                int[] coords = new int[] {x, y};
                if (makeDelay)
                {
                    int delay = rand.Next(1, 3);
                    Thread.Sleep((int)TimeSpan.FromSeconds(delay).TotalMilliseconds);
                }
                return coords;
            }
        }

    }
}