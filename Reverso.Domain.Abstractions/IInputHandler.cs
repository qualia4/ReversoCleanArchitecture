namespace Domain.Abstractions;

public interface IInputHandler
{
    public int[] GetPlayerCoords(IGameField reversoGameField);
}