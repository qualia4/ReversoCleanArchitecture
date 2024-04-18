namespace Reverso.Domain.Web;

public class Message
{
    public Guid Id { get; private set; }
    public Guid GameId { get; private set; }
    public Guid UserId { get; private set; }
    public string Text { get; private set; }
    public string Time { get; private set; }

    public Message(Guid gameId, Guid userId, string text)
    {
        Id = Guid.NewGuid();
        GameId = gameId;
        UserId = userId;
        Text = text;
        Time = DateTime.Now.TimeOfDay.ToString();
    }
}