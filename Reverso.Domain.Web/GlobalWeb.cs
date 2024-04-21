namespace Reverso.Domain.Web;

public class GlobalWeb
{
    public static GameResultNotifier ResultNotifier { get; private set; } = new GameResultNotifier();
}