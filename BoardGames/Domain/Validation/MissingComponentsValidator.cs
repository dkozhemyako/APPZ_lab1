using System.Collections.Generic;
using System.Linq;
using BoardGames.Domain;

namespace BoardGames.Domain.Validation;

public sealed class MissingComponentsValidator : ISetupValidator
{
    public OperationResult Validate(GameState state, IGameRules rules)
    {
        List<ComponentType> missing = rules.RequiredComponents
            .Where(req => state.Components.Contains(req) == false)
            .ToList();

        if (missing.Count > 0)
        {
            string list = string.Join(", ", missing);
            return OperationResult.Fail($"Missing components: {list}.");
        }

        return OperationResult.Ok();
    }
}