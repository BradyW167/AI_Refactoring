# Refactor Log

## Phase 1 — Extracting Player, Enemy, and MovingPlatform Logic

### New files
- `Player.cs` — owns horizontal movement, jump state, gravity, and sprite orientation. Public surface: `Picture`, `JumpSpeed`, `Grounded`, `Jumping`, `Reset(Point)`, `OnLeftDown/Up`, `OnRightDown/Up`, `OnJumpDown/Up`, `StopMovingLeft/Right`, `Update(screenWidth, screenHeight)` (returns true when the player has fallen off the bottom).
- `Enemy.cs` — a paces-on-platform enemy. Public surface: `Picture`, `Speed`, `Platform`, `GoRight`, `Reset(Label)`, `Update()`.
- `MovingPlatform.cs` — vertically oscillating platform between two Y limits. Public surface: `Label`, `Speed`, `TopLimit`, `BottomLimit`, `GoDown`, `Update()`.

### Changes in `frmMain.cs`
- Removed fields now owned by the new classes: `PlayerSpeed`, `Enemy{1,2}Speed`, `MovingPlatform{1,2}Speed`, `Platform{1,2}{Top,Bottom}`, `JumpSpeed`, `Gravity`, `Grounded` (moved to `Player.Grounded`), `Jumping`, `GoLeft`, `GoRight`, `Enemy{1,2}GoRight`, `Enemy{1,2}Platform`, `Platform{1,2}GoDown`, `LastKeyLeft`, `LastKeyRight`.
- Removed methods now owned by the new classes: `ResetPlayer`, `ResetEnemies`, `PutEnemyOnPlatform`, `CheckPlayerMovement`, `MoveEnemy`, `MovePlatform`, `FlipHorizontal`.
- Added instance fields: `player`, `enemy1`, `enemy2`, `movingPlatform1`, `movingPlatform2` (camelCase to avoid collision with class names). Constructed after the designer's PictureBox/Label wiring with the speeds/limits from the former constants inlined.
- `KeyIsDown` / `KeyIsUp` now forward directly to `Player.On{Left,Right,Jump}{Down,Up}`.
- `MoveImages` now calls `player.Update(...)`, `enemy{1,2}.Update()`, `movingPlatform{1,2}.Update()`. If `player.Update` reports an off-screen fall it calls `PlayerDeath()` (same behavior the former `CheckPlayerMovement` had).
- `PlatformCollisionCheck` still performs all collision logic per phase scope, but now reads/writes `player.JumpSpeed`, `player.Grounded`, and calls `player.StopMovingLeft/Right()` instead of mutating form fields directly. The redundant nested `if (picPlayer.Bounds.IntersectsWith(platform.Bounds))` check was collapsed.
- End-of-tick grounded reset now sets `player.Grounded = false`.

### Intentionally NOT moved (per Phase 1 rules)
- Collision detection (`EnemyCollisionCheck`, `CoinCollisionCheck`, `PlatformCollisionCheck`, `DoorCollisionCheck`) and the `LandedOnPlatform` tick-scratch flag remain in `frmMain`.
- Scoring (`Score` field, coin pickup increment) remains in `frmMain`.
- Database, audio, and form-to-form navigation remain untouched.

### Design note
`Player.Grounded` is a public settable property on the Player, but the *detection* of groundedness (the platform-top collision case) still lives in `frmMain.PlatformCollisionCheck`. The phase rule forbids moving the grounded *check*, not the state field; this split keeps Player's jump logic self-contained without pulling collision into the Player class.

### Build
- `dotnet build` — 0 warnings, 0 errors.
- `dotnet build --configuration Release` — 0 warnings, 0 errors.

## Phase 2 — Extracting GameEngine Logic

### New file
- `GameEngine.cs` — coordinates per-tick movement updates, collisions (enemy / coin / platform / door), scoring, grounded tracking, and victory/death detection. Public surface: `Player`, `Enemy1`, `Enemy2`, `MovingPlatform1`, `MovingPlatform2`, `Score`, `Ticks`, `PlayerDied` event, `PlayerWon` event, `Reset()`, `Tick(screenWidth, screenHeight)`. Constructor takes the form's `Control.ControlCollection` plus the player/enemy PictureBoxes and moving-platform Labels; the engine reads all other collision controls (static platforms, coins, door) by iterating the passed collection and looking at each control's `Tag`.

### Changes in `frmMain.cs`
- Removed fields now owned by the engine: `player`, `enemy1`, `enemy2`, `movingPlatform1`, `movingPlatform2`, `Score`, `Time`, `LandedOnPlatform`, and `PlayerStart`.
- Removed methods now owned by the engine: `ResetCharacters`, `ResetCoins`, `GetRandomPlatform`, `MoveImages`, `CheckCollision`, `EnemyCollisionCheck`, `CoinCollisionCheck`, `PlatformCollisionCheck`, `DoorCollisionCheck`, plus the small `UpdateTime` / `UpdateScore` helpers (their one-liner bodies were inlined into the tick handler).
- Added a single `engine` field, constructed after designer-supplied controls are wired, with `PlayerDied` bound to `PlayerDeath` and `PlayerWon` bound to `Victory`.
- `KeyIsDown` / `KeyIsUp` now call `engine.Player.On{Left,Right,Jump}{Down,Up}` instead of the former `player.` field.
- `tmrGame_Tick` is now three lines: update the score/time labels from `engine.Ticks` / `engine.Score`, then call `engine.Tick(ClientSize.Width, ClientSize.Height)`.
- `InitializeGame` now calls `engine.Reset()` instead of the two reset helpers + score/time zeroing.
- `Victory` and `PlayerDeath` are invoked by the engine via events, but their bodies (timer stop, audio, DB insert in the victory case, `RestartPrompt`) are unchanged.
- `RestartPrompt` now reads `engine.Score` for the win message.

### Intentionally NOT moved (kept in `frmMain`)
- The game timer (`tmrGame`) and its interval constant — the timer is a UI-framework concern, and the engine is the passive body it drives.
- Keyboard handlers — still forward directly to `engine.Player`.
- Audio playback (`MusicPlayer`, `PlayNewAudio`) — per Phase 3 preview, DB and audio are UI-shell concerns this phase does not touch.
- Database (`OpenConnection`, `InsertScore`, `Connection`, `Command`, `ConnectionString`) — left for Phase 3.
- Form navigation (`Logout`, `lblLogout_Click`, `frmMain_FormClosed`, launching `frmScores`) and the restart-prompt dialog — UI shell.
- The `TicksPerSecond` conversion — lives with the label formatting in `frmMain` since it depends on the timer's `GameTickSpeed`, which is also a UI concern.

### Design notes
- **Death / victory as events rather than direct calls.** The engine exposes `PlayerDied` and `PlayerWon` events that the form subscribes to. This lets the engine stay UI-agnostic (no audio, no `MessageBox`, no `Application.Exit`) while still preserving the original control flow: event handlers are invoked synchronously, so when `PlayerDeath` shows its modal dialog and the user picks "yes" → `InitializeGame` → `engine.Reset`, execution returns into `GameEngine.Tick`'s in-progress collision pass on the freshly reset state — the same behavior the original had when `PlayerDeath` was called inline from `MoveImages`.
- **Tick counter timing preserved.** The original read `Time` into the label *before* incrementing it; the engine's `Tick` increments `Ticks` at the start, but the form reads `engine.Ticks` into the label *before* calling `Tick`. Both result in the same one-tick-behind display and same final value.
- **Score display lag preserved.** Original displayed the score before the collision pass; new code does the same (labels are set before `engine.Tick`). A coin picked up on tick N shows on tick N+1 in both versions.
- **Engine gets the form's `Control.ControlCollection` directly.** Alternatives considered: pass explicit lists of platforms / coins / enemies / door, or expose typed collections. The direct collection was chosen because (a) the existing collision pass already iterates `this.Controls` and dispatches on `Tag`, so the engine inherits that without change, and (b) `GetRandomPlatform` needs to look up Labels by name, which is what `ControlCollection[string]` does. The coupling is to `System.Windows.Forms.Control.ControlCollection`, not to `frmMain` itself, so the engine is still testable with any container of controls.
- **PlatformCollisionCheck is copied verbatim.** The body (including the `SafetyThreshold` constant, the `Top - 5` fudge, and the "small bounce back" comment) was moved unchanged. Grounded *detection* still lives in the platform collision code — it is now in the engine rather than the form, matching Phase 2's scope of "grounded/platform logic" moving to the engine.
- **`LandedOnPlatform` is an internal scratch flag on the engine.** It is only read/written inside `Tick`; external callers never see it. The end-of-tick "if no platform caught us, clear Grounded" bookkeeping moved with it.

### Build
- `dotnet build` — 0 warnings, 0 errors.
- `dotnet build --configuration Release` — 0 warnings, 0 errors.
