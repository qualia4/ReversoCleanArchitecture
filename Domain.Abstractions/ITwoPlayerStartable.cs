namespace Domain.Abstractions;

public interface ITwoPlayerStartable
{
    public void StartGame(Player firstPlayer, Player secondPlayer);
}