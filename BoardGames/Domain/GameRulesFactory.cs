using System;

namespace BoardGames.Domain;

public static class GameRulesFactory
{
    public static IGameRules Create(GameType gameType)
    {
        switch (gameType)
        {
            case GameType.Chess:
                return new ChessRules();
            case GameType.Checkers:
                return new CheckersRules();
            case GameType.Backgammon:
                return new BackgammonRules();
            case GameType.Monopoly:
                return new MonopolyRules();
            default:
                throw new ArgumentOutOfRangeException(nameof(gameType));
        }
    }
}