using BoardGames.Domain;

namespace BoardGames.Domain.Validation;

public interface ISetupValidator
{
    OperationResult Validate(GameState state, IGameRules rules);
}