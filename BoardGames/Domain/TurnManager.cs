namespace BoardGames.Domain;

public sealed class TurnManager
{
    private bool actionPerformedThisTurn;

    public TurnManager()
    {
        actionPerformedThisTurn = false;
    }

    public bool CanAct(Player player, GameState state)
    {
        if (!state.IsStarted || state.IsFinished)
            return false;

        if (actionPerformedThisTurn)
            return false;

        return state.GetCurrentPlayer() == player;
    }

    public bool RegisterAction()
    {
        if (actionPerformedThisTurn)
            return false;

        actionPerformedThisTurn = true;
        return true;
    }

    public void NextPlayer(GameState state)
    {
        state.MoveToNextPlayer();
        ResetTurn();
    }

    public void ResetTurn()
    {
        actionPerformedThisTurn = false;
    }
}
