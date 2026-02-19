using System.Collections.Generic;
using System.Linq;

namespace BoardGames.Domain;

public sealed class BackgammonRules : IGameRules
{
    public int MinPlayers => 2;
    public int MaxPlayers => 2;

    public List<ComponentType> RequiredComponents { get; } =
        new() { ComponentType.Board, ComponentType.Dice, ComponentType.Chips };

    public List<ActionType> AllowedActions { get; } =
        new() { ActionType.Move };

    public bool ValidateSetup(int playersCount, List<ComponentType> components)
    {
        if (playersCount < MinPlayers || playersCount > MaxPlayers)
            return false;

        return components.Distinct().OrderBy(x => x)
            .SequenceEqual(RequiredComponents.Distinct().OrderBy(x => x));
    }
}
