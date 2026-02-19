namespace BoardGames.Domain;

public sealed class Player
{
    public string Name { get; }

    public Player(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}
