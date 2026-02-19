using System.Collections.Generic;

namespace BoardGames.Domain;

public sealed class GameState
{
    public List<Player> Players { get; }
    public List<ComponentType> Components { get; }

    public int CurrentPlayerIndex { get; private set; }

    public bool IsStarted { get; private set; }
    public bool IsFinished { get; private set; }

    public Player? Winner { get; private set; }

    public GameState(List<Player> players, List<ComponentType> components)
    {
        Players = players;
        Components = components;

        CurrentPlayerIndex = 0;
        IsStarted = false;
        IsFinished = false;
        Winner = null;
    }

    public Player GetCurrentPlayer()
    {
        return Players[CurrentPlayerIndex];
    }

    public void MarkStarted()
    {
        IsStarted = true;
    }

    public void MarkFinished(Player winner)
    {
        IsFinished = true;
        Winner = winner;
    }

    public void MoveToNextPlayer()
    {
        CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
    }
}
