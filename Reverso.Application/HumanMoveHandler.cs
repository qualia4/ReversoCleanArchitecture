using Domain.Abstractions;
namespace Application;

public class HumanMoveHandler: IInputHandler
{
    public int[] GetPlayerCoords(IGameField reversoGameField)
    {
        return new []{1, 1};
    }
}