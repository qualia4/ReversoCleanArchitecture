namespace Reverso.Domain.Web;

public class GameResultNotifier
{
    public event Action<Dictionary<string, int>>? GameEnded;

    public async Task InvokeEvent(Dictionary<string, int> points)
    {
        await Task.Run(() => GameEnded?.Invoke(points));
    }
}