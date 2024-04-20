namespace Domain.Abstractions;

public interface ITwoPlayerGame: ITwoPlayerStartable
{
    public Task MakeMove();
    public bool GetEnded();
}