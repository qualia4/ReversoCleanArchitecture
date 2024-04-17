namespace Application;

public interface IGameCreator
{
    Task<Guid> CreateGameSession(string userId, string gameType);
}