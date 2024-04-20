namespace Domain.Abstractions;

public interface ITwoPlayerStartable
{
    public Task StartGame(Player firstPlayer, Player secondPlayer);
}