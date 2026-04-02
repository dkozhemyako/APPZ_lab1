using System.Collections.Generic;
using System.Linq;
using BoardGames.Domain;

namespace BoardGames.Domain.Validation;

public sealed class ExtraComponentsValidator : ISetupValidator
{
    public OperationResult Validate(GameState state, IGameRules rules)
    {
        List<ComponentType> extra = state.Components
            .Where(c => rules.RequiredComponents.Contains(c) == false)
            .ToList();

        if (extra.Count > 0)
        {
            string list = string.Join(", ", extra);
            return OperationResult.Fail($"Extra components: {list}.");
        }

        return OperationResult.Ok();
    }
}