# Refactor Log

## Phase 1 — Extracting Player, Enemy, and MovingPlatform Logic

### New Classes

**Player.cs**
- Owns: horizontal movement, gravity, jump physics, directional input, sprite flipping, screen-bounds clamping
- Properties exposed for collision code: `JumpSpeed`, `GoLeft`, `GoRight`, `Picture`
- `HandleKeyDown(key, grounded)` — called by frmMain's KeyIsDown handler
- `HandleKeyUp(key)` — called by frmMain's KeyIsUp handler
- `Update(grounded, screenWidth, screenHeight)` — moves player each tick; returns `true` if player fell off screen
- `Reset()` — repositions player and restores sprite orientation

**Enemy.cs**
- Owns: horizontal patrol movement, direction tracking, sprite flipping, platform bounds
- `Update()` — advances enemy and reverses direction at platform edges
- `Reset(platform)` — places enemy on a new platform facing right

**MovingPlatform.cs**
- Owns: vertical oscillation, direction tracking, top/bottom limits
- `Update()` — moves platform and reverses at limits

### Changes to frmMain.cs

Removed fields: `PlayerSpeed`, `Enemy1Speed`, `Enemy2Speed`, `MovingPlatform1Speed`, `MovingPlatform2Speed`, `PlayerStart`, `JumpSpeed`, `Gravity`, `Jumping`, `GoLeft`, `GoRight`, `Enemy1GoRight`, `Enemy2GoRight`, `Platform1GoDown`, `Platform2GoDown`, `Enemy1Platform`, `Enemy2Platform`, `LastKeyLeft`, `LastKeyRight`

Removed methods: `CheckPlayerMovement`, `MoveEnemy`, `MovePlatform`, `FlipHorizontal`, `PutEnemyOnPlatform`

Updated methods:
- `MoveImages` — delegates to `player.Update()`, `enemy1/2.Update()`, `platform1/2.Update()`
- `KeyIsDown` / `KeyIsUp` — forward input to `player.HandleKeyDown/Up()`
- `ResetPlayer` — delegates to `player.Reset()`, keeps `Grounded = false`
- `ResetEnemies` — delegates to `enemy1/2.Reset(GetRandomPlatform())`
- `PlatformCollisionCheck` — references `player.JumpSpeed`, `player.GoLeft`, `player.GoRight`

Collision logic, grounded/LandedOnPlatform state, and scoring remain in frmMain unchanged.
