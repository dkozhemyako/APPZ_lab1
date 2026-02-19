using System.Collections.Generic;
using System.Linq;

namespace BoardGames.Domain;

public sealed class MonopolyRules : IGameRules
{
    public int MinPlayers => 2;
    public int MaxPlayers => 6;

    public List<ComponentType> RequiredComponents { get; } =
        new() { ComponentType.Board, ComponentType.Dice, ComponentType.Chips };

    public List<ActionType> AllowedActions { get; } =
        new() { ActionType.Move, ActionType.Buy, ActionType.Sell };

    public bool ValidateSetup(int playersCount, List<ComponentType> components)
    {
        if (playersCount < MinPlayers || playersCount > MaxPlayers)
            return false;

        return components.Distinct().OrderBy(x => x)
            .SequenceEqual(RequiredComponents.Distinct().OrderBy(x => x));
    }
}
