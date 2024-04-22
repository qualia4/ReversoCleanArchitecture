namespace Domain.Game;

using Abstractions;

public class ReversoField: IGameField
{
    private const int FieldSize = 8;
    private Cell[,] Cells { get; set; }

    public ReversoField()
    {
        Cells = new Cell[FieldSize, FieldSize];
        for (var x = 0; x < FieldSize; x++)
        {
            for (var y = 0; y < FieldSize; y++)
            {
                Cells[x, y] = new Cell();
            }
        }
    }

    public void Initialize(Player firstPlayer, Player secondPlayer)
    {
        Cells = new Cell[FieldSize, FieldSize];
        for (var x = 0; x < FieldSize; x++)
        {
            for (var y = 0; y < FieldSize; y++)
            {
                Cells[x, y] = new Cell();
            }
        }
        Cells[3,4].Rehost(firstPlayer);
        Cells[4,3].Rehost(firstPlayer);
        firstPlayer.AddPoints(2);
        Cells[3,3].Rehost(secondPlayer);
        Cells[4,4].Rehost(secondPlayer);
        secondPlayer.AddPoints(2);
        ChangeValid(firstPlayer);
    }

    public void ChangeValid(Player CurrentPlayer)
    {
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                Cells[x, y].SetValid(IsValidMove(x, y, CurrentPlayer));
            }
        }
    }

    private bool IsValidMove(int x, int y, Player CurrentPlayer)
    {
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

        if (!Cells[x, y].IfEmpty || !IsInBounds(x, y))
        {
            return false;
        }

        for (int dir = 0; dir < 8; dir++)
        {
            if (IsValidDirection(x, y, dx[dir], dy[dir], CurrentPlayer))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsValidDirection(int x, int y, int dirX, int dirY, Player CurrentPlayer)
    {
        bool foundOpponent = false;
        while (true)
        {
            x += dirX;
            y += dirY;
            if (!IsInBounds(x, y) || Cells[x, y].IfEmpty)
            {
                break;
            }

            if (Cells[x, y].GetHost() == CurrentPlayer)
            {
                if (foundOpponent)
                {
                    return true;
                }
                break;
            }
            foundOpponent = true;
        }
        return false;
    }

    public bool IsInBounds(int x, int y)
    {
        return x is >= 0 and < FieldSize && y is >= 0 and < FieldSize;
    }

    public int ChangeField(int x, int y, Player CurrentPlayer)
    {
        MarkCell(x, y, CurrentPlayer);
        int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };
        int pointsToDistribute = 0;

        for (int dir = 0; dir < 8; dir++)
        {
            if (IsValidDirection(x, y, dx[dir], dy[dir], CurrentPlayer))
            {
                pointsToDistribute += FlipPiecesInDirection(x, y, dx[dir], dy[dir], CurrentPlayer);
            }
        }

        return pointsToDistribute;
    }

    private void MarkCell(int x, int y, Player CurrentPlayer)
    {
        Cells[x, y].Rehost(CurrentPlayer);
    }

    private int FlipPiecesInDirection(int startX, int startY, int dx, int dy, Player CurrentPlayer)
    {
        int x = startX + dx;
        int y = startY + dy;
        int pointsToRedistribute = 0;

        while (IsInBounds(x, y) && Cells[x, y].IfEmpty == false &&
               Cells[x, y].GetHost() != CurrentPlayer)
        {
            Cells[x, y].Rehost(CurrentPlayer);
            pointsToRedistribute++;
            x += dx;
            y += dy;
        }
        return pointsToRedistribute;
    }

    public bool HasValidMoves()
    {
        for (var x = 0; x < Cells.GetLength(0); x++)
        {
            for (var y = 0; y < Cells.GetLength(1); y++)
            {
                if (Cells[x, y].IfValid)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public IGameField DeepClone()
    {
        var newField = new ReversoField();
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                if (!Cells[x, y].IfEmpty)
                {
                    newField.Cells[x, y].Rehost(Cells[x, y].GetHost());
                    newField.Cells[x, y].SetValid(Cells[x, y].IfValid);
                }
            }
        }
        return newField;
    }

    public Player GetHost(int x, int y)
    {
        if (IsInBounds(x, y) && !Cells[x, y].IfEmpty)
        {
            return Cells[x, y].GetHost() as Player;
        }
        return null;
    }

    public Cell[,] GetCells()
    {
        return Cells;
    }

    public bool IsValidCell(int x, int y)
    {
        return Cells[x, y].IfValid;
    }

    public int GetSize()
    {
        return FieldSize;
    }


}