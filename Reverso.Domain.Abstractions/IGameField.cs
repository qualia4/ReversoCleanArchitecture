namespace Domain.Abstractions;

public interface IGameField
{
    public int ChangeField(int x, int y, Player CurrentPlayer);
    public void ChangeValid(Player CurrentPlayer);
    public bool IsInBounds(int x, int y);
    public bool IsValidCell(int x, int y);
    public bool HasValidMoves();
    public int GetSize();
    public IGameField DeepClone();
    public Player? GetHost(int x, int y);
}