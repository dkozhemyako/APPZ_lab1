using BoardGames.Domain.Validation;
using System.Collections.Generic;
namespace BoardGames.Domain;


public sealed class GameSession
{
    private readonly IGameRules rules;
    private readonly GameState state;
    private readonly TurnManager turnManager;

    public event Action<Player>? TurnChanged;
    public event Action<Player>? GameFinished;

    public GameSession(IGameRules rules, GameState state, TurnManager turnManager)
    {
        this.rules = rules;
        this.state = state;
        this.turnManager = turnManager;
    }

    public OperationResult Start()
    {
        if (state.IsStarted || state.IsFinished)
            return OperationResult.Fail("Game cannot be started: already started or finished.");

        var chain = new SetupValidationChain(new List<ISetupValidator>
    {
        new PlayersCountValidator(),
        new MissingComponentsValidator(),
        new ExtraComponentsValidator()
    });

        OperationResult setupResult = chain.Validate(state, rules);
        if (setupResult.Success == false)
            return setupResult;

        state.MarkStarted();
        TurnChanged?.Invoke(state.GetCurrentPlayer());

        return OperationResult.Ok("Game started successfully.");
    }

    public bool PerformAction(Player player, ActionType action)
    {
        if (!state.IsStarted || state.IsFinished)
            return false;

        if (!turnManager.CanAct(player, state))
            return false;

        if (!rules.AllowedActions.Contains(action))
            return false;

        return turnManager.RegisterAction();
    }

    public void EndTurn()
    {
        if (!state.IsStarted || state.IsFinished)
            return;

        state.MoveToNextPlayer();
        turnManager.ResetTurn();

        TurnChanged?.Invoke(state.GetCurrentPlayer());
    }

    public void DeclareWinner(Player player)
    {
        if (!state.IsStarted || state.IsFinished)
            return;

        state.MarkFinished(player);
        GameFinished?.Invoke(player);
    }
}
