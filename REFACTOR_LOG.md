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
