namespace Domain.Abstractions;

public interface IInputHandler
{
    public Task<int[]> GetPlayerCoords(IGameField reversoGameField);
}