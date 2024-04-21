using Domain.Abstractions;
namespace Application;

public class HumanMoveHandler: IInputHandler
{
    private readonly MoveEndpointListener _listener;
    private TaskCompletionSource<(int, int)> _moveTaskSource = new TaskCompletionSource<(int, int)>();
    public string HumanUsername { get; private set; }

    public HumanMoveHandler(string username)
    {
        _listener = GlobalResources.EndpointListener;
        _listener.MoveEndpointReceived += OnMoveReceived;
        HumanUsername = username;
    }

    private void OnMoveReceived((int, int) coordinates, string username)
    {
        if (username == HumanUsername)
        {
            _moveTaskSource.TrySetResult((coordinates.Item1, coordinates.Item2));
        }
    }

    public async Task<int[]> GetPlayerCoords(IGameField reversoGameField)
    {
        while (true)
        {
            Console.WriteLine("Waiting for + " + HumanUsername + " move...");
            var result = await _moveTaskSource.Task;
            _moveTaskSource = new TaskCompletionSource<(int, int)>();
            var x = result.Item1;
            var y = result.Item2;
            if (reversoGameField.IsInBounds(x, y) && reversoGameField.IsValidCell(x, y))
            {
                int[] coords = new int[] {x, y};
                return coords;
            }
        }
    }
}