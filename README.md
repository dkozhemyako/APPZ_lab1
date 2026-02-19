# APPZ Lab 1.2 — Variant 14 (Board Games)

## Overview
Console application (C#) that simulates playing board games according to **Variant 14**:
- **Monopoly**
- **Checkers**
- **Backgammon**
- **Chess**

Implemented requirements:
- Game starts only if the **required components** are present and **no extra** components exist.
- Game starts only if the **number of players** matches rules (e.g., Monopoly 2..6, others 2..2).
- Players act **in turns only** (strict order).
- **One action per turn**.
- Game ends when a **winner is declared**.
- Domain notifies UI via **events** (`TurnChanged`, `GameFinished`).

---

## Project structure
- `Domain/` — domain logic (entities, rules, session, turn manager)
- `Program.cs` — console UI (input/output, subscriptions to events)
- `docs/screenshots/` — screenshots

---

## How to run
### Option A — Visual Studio
1. Open the solution in **Visual Studio**.
2. Select the project as **Startup Project** (if needed).
3. Run:
   - **Ctrl + F5** (Run without debugging)  
   or  
   - **Start** button.

### Interactive mode
Flow:
1. Choose game rules (Chess / Checkers / Backgammon / Monopoly)
2. Enter player count and names (count is validated by rules)
3. Choose components “on the table”
4. Start the game (validation happens automatically)

Commands
- `a` — perform an action (only actions allowed by the selected game rules)
- `e` — end turn (switch to the next player)
- `w` — declare winner (finish game)
- `q` — quit interactive session

Events (UI output):
- `TurnChanged` — prints the current player
- `GameFinished` — prints the winner

### Demo mode
Demonstrates:
- Start rejected due to **missing** required component
- Start rejected due to **extra** component
- Start rejected due to **invalid player count** (Monopoly)
- Full flow: turn order, one action per turn, declaring winner, events

### Screenshots
**01 — Visual Studio project opened**  
![01](docs/screenshots/01.png)

**02 — Program menu**  
![02](docs/screenshots/02.png)

**03 — Demo: invalid components (missing Figures)**  
![03](docs/screenshots/03.png)

**04 — Demo: invalid components (extra Dice)**  
![04](docs/screenshots/04.png)

**05 — Demo: invalid players count (Monopoly)**  
![05](docs/screenshots/05.png)

**06 — Demo: full flow (turn order + one action per turn + winner + events)**  
![06](docs/screenshots/06.png)