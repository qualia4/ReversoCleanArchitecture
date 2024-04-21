namespace Reverso.Domain.Web;

public class Message
{
    public string Username { get; private set; }
    public string Text { get; private set; }
    public string Time { get; private set; }

    public Message(string username, string text)
    {
        Username = username;
        Text = text;
        Time = DateTime.Now.ToString("HH:mm");
    }
}