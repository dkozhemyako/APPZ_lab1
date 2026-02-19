using System.Collections.Generic;

namespace BoardGames.Domain;

public interface IGameRules
{
    List<ComponentType> RequiredComponents { get; }
    int MinPlayers { get; }
    int MaxPlayers { get; }
    List<ActionType> AllowedActions { get; }

    bool ValidateSetup(int playersCount, List<ComponentType> components);
}
