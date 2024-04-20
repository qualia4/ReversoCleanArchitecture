using Domain.Abstractions;
namespace Application;

public class HumanMoveHandler: IInputHandler
{
    public Task<int[]> GetPlayerCoords(IGameField reversoGameField, string username)
    {
        return Task.FromResult(new []{1, 1});
    }
}