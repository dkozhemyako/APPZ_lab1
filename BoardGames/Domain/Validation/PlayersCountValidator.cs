using BoardGames.Domain;

namespace BoardGames.Domain.Validation;

public sealed class PlayersCountValidator : ISetupValidator
{
    public OperationResult Validate(GameState state, IGameRules rules)
    {
        int count = state.Players.Count;

        if (count < rules.MinPlayers || count > rules.MaxPlayers)
        {
            return OperationResult.Fail(
                $"Invalid players count: expected {rules.MinPlayers}..{rules.MaxPlayers}, got {count}."
            );
        }

        return OperationResult.Ok();
    }
}