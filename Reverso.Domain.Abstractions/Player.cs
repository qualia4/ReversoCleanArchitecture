namespace Domain.Abstractions;

public abstract class Player: IHasName
{
    private string Name { get; }
    private int Points { get; set; }

    protected Player(string name)
    {
        Name = name;
        Points = 0;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    public void ResetPoints()
    {
        Points = 0;
    }

    public void RemovePoints(int pointsToRemove)
    {
        Points -= pointsToRemove;
    }

    public int GetPoints()
    {
        return Points;
    }

    public string GetName()
    {
        return Name;
    }

    public abstract Task<int> MakeMoveOnField(IGameField gameField);

}