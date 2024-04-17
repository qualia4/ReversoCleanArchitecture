namespace Domain.Game;
using Abstractions;

public class Cell
{
    private bool IsEmpty { get; set; } = true;
    private bool IsValid { get; set; }
    private IHasName? Host { get; set; }
    public bool IfEmpty => IsEmpty;
    public bool IfValid => IsValid;

    public void Rehost(IHasName newHost)
    {
        Host = newHost;
        IsEmpty = false;
    }

    public void SetValid(bool isValid)
    {
        this.IsValid = isValid;
    }

    public IHasName? GetHost()
    {
        return Host;
    }
}