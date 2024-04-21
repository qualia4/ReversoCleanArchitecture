namespace Application;

public class MoveEndpointListener
{
    public event Action<(int, int), string>? MoveEndpointReceived;

    public async Task InvokeEvent((int, int) coordinates, string username)
    {
        await Task.Run(() => MoveEndpointReceived?.Invoke(coordinates, username));
    }
}