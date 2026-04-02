using BoardGames.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("~Board Games Console~");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1) Interactive mode");
    Console.WriteLine("2) Demo mode (current scenarios)");
    Console.WriteLine("0) Exit");
    Console.Write("Choose: ");

    string? choice = Console.ReadLine();

    if (choice == "0")
        break;

    if (choice == "2")
    {
        RunDemoMode();
        continue;
    }

    if (choice == "1")
    {
        RunInteractiveMode();
        continue;
    }

    Console.WriteLine("Unknown option.");
}

void RunInteractiveMode()
{
    Console.WriteLine();
    Console.WriteLine("~INTERACTIVE MODE~");

    IGameRules rules = ChooseGameRules();

    List<Player> players = ReadPlayers(rules.MinPlayers, rules.MaxPlayers);
    List<ComponentType> components = ChooseComponents();

    var state = new GameState(players, components);
    var turnManager = new TurnManager();
    var session = new GameSession(rules, state, turnManager);


    session.TurnChanged += OnTurnChanged;
    session.GameFinished += OnGameFinished;

    Console.WriteLine();
    Console.WriteLine("Trying to start the game...");
    bool started = session.Start();
    Console.WriteLine($"Start result: {started}");

    if (started == false)
    {
        Console.WriteLine("Game did not start. Reasons can be:");
        Console.WriteLine("- wrong number of players");
        Console.WriteLine("- missing required components");
        Console.WriteLine("- extra components present");
        return;
    }

  
    while (true)
    {
        if (state.IsFinished)
            break;

        Player current = state.GetCurrentPlayer();

        Console.WriteLine();
        Console.WriteLine($"Current player: {current}");
        Console.WriteLine($"Allowed actions: {string.Join(", ", rules.AllowedActions)}");
        Console.WriteLine("Commands:");
        Console.WriteLine("  a) Do action");
        Console.WriteLine("  e) End turn");
        Console.WriteLine("  w) Declare winner (finish game)");
        Console.WriteLine("  q) Quit interactive session");
        Console.Write("Choose command: ");

        string? cmd = Console.ReadLine();

        if (cmd == "q")
            break;

        if (cmd == "a")
        {
            ActionType action = ReadAction(rules.AllowedActions);

            bool ok = session.PerformAction(current, action);
            Console.WriteLine($"Action result: {ok}");

            if (ok == false)
            {
                Console.WriteLine("Action rejected. Possible reasons:");
                Console.WriteLine("- not your turn");
                Console.WriteLine("- action already done this turn");
                Console.WriteLine("- action not allowed by rules");
                Console.WriteLine("- game not started or already finished");
            }

            continue;
        }

        if (cmd == "e")
        {
            session.EndTurn();
            continue;
        }

        if (cmd == "w")
        {
            Console.WriteLine("Select winner by index:");
            for (int i = 0; i < players.Count; i++)
                Console.WriteLine($"{i + 1}) {players[i].Name}");

            int winIndex = ReadIntInRange("Winner: ", 1, players.Count) - 1;
            session.DeclareWinner(players[winIndex]);
            continue;
        }

        Console.WriteLine("Unknown command.");
    }

    session.TurnChanged -= OnTurnChanged;
    session.GameFinished -= OnGameFinished;

    Console.WriteLine("Interactive session ended.");
}

IGameRules ChooseGameRules()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("Choose game:");
        Console.WriteLine("1) Chess");
        Console.WriteLine("2) Checkers");
        Console.WriteLine("3) Backgammon");
        Console.WriteLine("4) Monopoly");
        Console.Write("Choose: ");

        string? s = Console.ReadLine();

        if (int.TryParse(s, out int value) == false)
        {
            Console.WriteLine("Enter 1..4.");
            continue;
        }

        if (Enum.IsDefined(typeof(GameType), value) == false)
        {
            Console.WriteLine("Enter 1..4.");
            continue;
        }

        GameType type = (GameType)value;
        return GameRulesFactory.Create(type);
    }
}

List<Player> ReadPlayers(int min, int max)
{
    Console.WriteLine();
    int count = ReadIntInRange($"Enter players count ({min}..{max}): ", min, max);

    var players = new List<Player>();
    for (int i = 0; i < count; i++)
    {
        Console.Write($"Player {i + 1} name: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
            name = $"Player{i + 1}";

        players.Add(new Player(name));
    }

    return players;
}

List<ComponentType> ChooseComponents()
{
    Console.WriteLine();
    Console.WriteLine("Choose components on the table (enter numbers separated by commas).");
    Console.WriteLine("Available components:");

    ComponentType[] all = Enum.GetValues<ComponentType>();
    for (int i = 0; i < all.Length; i++)
        Console.WriteLine($"{i + 1}) {all[i]}");

    Console.Write("Your selection (e.g. 1,2): ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
        return new List<ComponentType>();

    string[] parts = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

    var result = new List<ComponentType>();

    foreach (string p in parts)
    {
        if (int.TryParse(p.Trim(), out int idx) == false)
            continue;

        if (idx < 1 || idx > all.Length)
            continue;

        result.Add(all[idx - 1]);
    }

    return result;
}

ActionType ReadAction(List<ActionType> allowed)
{
    while (true)
    {
        Console.WriteLine("Choose action:");
        for (int i = 0; i < allowed.Count; i++)
            Console.WriteLine($"{i + 1}) {allowed[i]}");

        int idx = ReadIntInRange("Action: ", 1, allowed.Count);
        return allowed[idx - 1];
    }
}

int ReadIntInRange(string prompt, int min, int max)
{
    while (true)
    {
        Console.Write(prompt);
        string? s = Console.ReadLine();

        if (int.TryParse(s, out int value) && value >= min && value <= max)
            return value;

        Console.WriteLine($"Enter integer in range {min}..{max}.");
    }
}

void OnTurnChanged(Player current)
{
    Console.WriteLine($"[EVENT] Turn changed. Current player: {current}");
}

void OnGameFinished(Player winner)
{
    Console.WriteLine($"[EVENT] Game finished. Winner: {winner}");
}

void RunDemoMode()
{
    Console.WriteLine();
    Console.WriteLine("~DEMO: invalid components (missing Figures)~");

    var badState1 = new GameState(
        new List<Player> { new Player("A"), new Player("B") },
        new List<ComponentType> { ComponentType.Board }
    );

    var badSession1 = new GameSession(new ChessRules(), badState1, new TurnManager());
    bool badStart1 = badSession1.Start();
    Console.WriteLine($"Start result: {badStart1}");

    Console.WriteLine();
    Console.WriteLine("~DEMO: invalid components (extra Dice)~");

    var badState2 = new GameState(
        new List<Player> { new Player("A"), new Player("B") },
        new List<ComponentType> { ComponentType.Board, ComponentType.Figures, ComponentType.Dice }
    );

    var badSession2 = new GameSession(new ChessRules(), badState2, new TurnManager());
    bool badStart2 = badSession2.Start();
    Console.WriteLine($"Start result: {badStart2}");

    Console.WriteLine();
    Console.WriteLine("~DEMO: invalid players count (Monopoly)~");

    var monopolyBadPlayers = new GameState(
        new List<Player> { new Player("A") },
        new List<ComponentType> { ComponentType.Board, ComponentType.Dice, ComponentType.Chips }
    );

    var monopolySession = new GameSession(new MonopolyRules(), monopolyBadPlayers, new TurnManager());
    bool monopolyStart = monopolySession.Start();
    Console.WriteLine($"Start result: {monopolyStart}");

    Console.WriteLine();
    Console.WriteLine("~DEMO: full flow (Chess)~");

    IGameRules rules = new ChessRules();

    var players = new List<Player> { new Player("Alice"), new Player("Bob") };
    var components = new List<ComponentType> { ComponentType.Board, ComponentType.Figures };

    var state = new GameState(players, components);
    var tm = new TurnManager();
    var session = new GameSession(rules, state, tm);

    session.TurnChanged += OnTurnChanged;
    session.GameFinished += OnGameFinished;

    bool started = session.Start();
    Console.WriteLine($"Start result: {started}");

    Console.WriteLine("Try action by Alice: Move");
    bool a1 = session.PerformAction(players[0], ActionType.Move);
    Console.WriteLine($"Action result: {a1}");

    Console.WriteLine("Try second action by Alice in same turn: Take");
    bool a2 = session.PerformAction(players[0], ActionType.Take);
    Console.WriteLine($"Action result: {a2}");

    Console.WriteLine("Try action by Bob (but still Alice turn until EndTurn): Move");
    bool b1 = session.PerformAction(players[1], ActionType.Move);
    Console.WriteLine($"Action result: {b1}");

    Console.WriteLine("EndTurn()");
    session.EndTurn();

    Console.WriteLine("Try action by Bob now: Move");
    bool b2 = session.PerformAction(players[1], ActionType.Move);
    Console.WriteLine($"Action result: {b2}");

    Console.WriteLine("DeclareWinner(Bob)");
    session.DeclareWinner(players[1]);

    Console.WriteLine("Try action after finish: Move");
    bool after = session.PerformAction(players[1], ActionType.Move);
    Console.WriteLine($"Action result: {after}");

    session.TurnChanged -= OnTurnChanged;
    session.GameFinished -= OnGameFinished;
}
