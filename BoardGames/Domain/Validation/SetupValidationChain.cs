using System.Collections.Generic;
using BoardGames.Domain;

namespace BoardGames.Domain.Validation;

public sealed class SetupValidationChain
{
    private readonly List<ISetupValidator> validators;

    public SetupValidationChain(List<ISetupValidator> validators)
    {
        this.validators = validators;
    }

    public OperationResult Validate(GameState state, IGameRules rules)
    {
        foreach (ISetupValidator v in validators)
        {
            OperationResult result = v.Validate(state, rules);
            if (result.Success == false)
            {
                return result;
            }
        }

        return OperationResult.Ok("Setup is valid.");
    }
}