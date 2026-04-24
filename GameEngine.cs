using System;
using System.Drawing;
using System.Windows.Forms;

namespace JumpGame
{
    // Coordinates game rules: per-tick movement updates, collision detection
    // (enemy / coin / platform / door), scoring, grounded tracking, and
    // victory / death detection. The owning form drives Tick() from its timer
    // and handles audio, DB persistence, and navigation by subscribing to the
    // PlayerDied and PlayerWon events.
    public class GameEngine
    {
        private static readonly Point PlayerStart = new Point(10, 525);

        // Label-name suffixes of platforms eligible for enemy spawn.
        private static readonly int[] SpawnPlatformNumbers = { 2, 3, 5, 7, 8 };

        public Player Player { get; }
        public Enemy Enemy1 { get; }
        public Enemy Enemy2 { get; }
        public MovingPlatform MovingPlatform1 { get; }
        public MovingPlatform MovingPlatform2 { get; }

        public int Score { get; private set; }
        // Count of ticks since the current game started. Seconds-conversion is
        // the UI's responsibility since the tick interval lives there.
        public int Ticks { get; private set; }

        // Source for controls that participate in collisions (tagged as
        // "platform" / "movingPlatform" / "enemy" / "coin" / "door") and for
        // looking up spawn platforms by name.
        private readonly Control.ControlCollection controls;

        private readonly Random random = new Random();

        // Set during the collision pass when the player lands on any platform
        // this tick; consumed at end of Tick to update Player.Grounded.
        private bool LandedOnPlatform { get; set; }

        public event Action? PlayerDied;
        public event Action? PlayerWon;

        public GameEngine(
            Control.ControlCollection controls,
            PictureBox playerPicture,
            PictureBox enemy1Picture,
            PictureBox enemy2Picture,
            Label movingPlatform1Label,
            Label movingPlatform2Label)
        {
            this.controls = controls;

            Player = new Player(playerPicture);
            Enemy1 = new Enemy(enemy1Picture, speed: 6);
            Enemy2 = new Enemy(enemy2Picture, speed: 8);
            MovingPlatform1 = new MovingPlatform(movingPlatform1Label, speed: 3, topLimit: 350, bottomLimit: 470);
            MovingPlatform2 = new MovingPlatform(movingPlatform2Label, speed: 7, topLimit: 160, bottomLimit: 280);
        }

        // Resets characters, coins, score and tick counter for a fresh game.
        public void Reset()
        {
            ResetCharacters();
            ResetCoins();

            Score = 0;
            Ticks = 0;
        }

        // Advances one game step: tick counter, movement, collision pass, and
        // end-of-tick grounded bookkeeping. Death / victory are signalled via
        // events rather than direct calls so the UI can own the response.
        public void Tick(int screenWidth, int screenHeight)
        {
            Ticks += 1;

            MoveObjects(screenWidth, screenHeight);

            LandedOnPlatform = false;

            foreach (Control control in controls)
            {
                if (control.Tag is string tagValue)
                {
                    CheckCollision(control, tagValue);
                }
            }

            // After all collision checks, clear Grounded if no platform caught
            // the player this tick. Needed because moving platforms can step
            // out from under the player without any side/below collision firing.
            if (!LandedOnPlatform)
            {
                Player.Grounded = false;
            }
        }

        private void ResetCharacters()
        {
            Player.Reset(PlayerStart);
            Enemy1.Reset(GetRandomPlatform());
            Enemy2.Reset(GetRandomPlatform());
        }

        private void ResetCoins()
        {
            foreach (Control control in controls)
            {
                if (control.Tag != null)
                {
                    if (control is PictureBox && (string)control.Tag == "coin")
                    {
                        control.Visible = true;
                    }
                }
            }
        }

        private Label GetRandomPlatform()
        {
            int randIndex = random.Next(SpawnPlatformNumbers.Length);
            string randomPlatformName = $"lblPlatform{SpawnPlatformNumbers[randIndex]}";

            if (controls[randomPlatformName] is Label randomPlatform)
                return randomPlatform;
            else
                return new Label();
        }

        private void MoveObjects(int screenWidth, int screenHeight)
        {
            if (Player.Update(screenWidth, screenHeight))
            {
                PlayerDied?.Invoke();
            }

            Enemy1.Update();
            Enemy2.Update();

            MovingPlatform1.Update();
            MovingPlatform2.Update();
        }

        private void CheckCollision(Control control, string tagValue)
        {
            if (tagValue == "enemy")
            {
                EnemyCollisionCheck(control);
            }

            if (tagValue == "coin")
            {
                CoinCollisionCheck(control);
            }

            if (tagValue == "platform" || tagValue == "movingPlatform")
            {
                PlatformCollisionCheck(control);
            }
            if (tagValue == "door")
            {
                DoorCollisionCheck(control);
            }
        }

        private void EnemyCollisionCheck(Control enemy)
        {
            if (Player.Picture.Bounds.IntersectsWith(enemy.Bounds))
            {
                PlayerDied?.Invoke();
            }
        }

        private void CoinCollisionCheck(Control coin)
        {
            if (Player.Picture.Bounds.IntersectsWith(coin.Bounds) && coin.Visible)
            {
                coin.Visible = false;
                Score += 1;
            }
        }

        private void PlatformCollisionCheck(Control platform)
        {
            PictureBox picPlayer = Player.Picture;

            if (picPlayer.Bounds.IntersectsWith(platform.Bounds))
            {
                Rectangle plat = platform.Bounds;

                // Additional pixels added to collision checks
                // Prevents player image from bouncing while on platforms
                const int SafetyThreshold = 5;

                // Landing on top of platform
                if (picPlayer.Bottom >= plat.Top - 5 &&
                    picPlayer.Top < plat.Top &&
                    Player.JumpSpeed >= 0)
                {
                    LandedOnPlatform = true;
                    Player.Grounded = true;
                    Player.JumpSpeed = 0;
                    picPlayer.Top = plat.Top - picPlayer.Height + 1;
                }
                // Hitting platform from the sides
                else if (picPlayer.Right > plat.Left && picPlayer.Left < plat.Left && picPlayer.Bottom > plat.Top + SafetyThreshold)
                {
                    picPlayer.Left = plat.Left - picPlayer.Width;
                    Player.StopMovingRight();
                }
                else if (picPlayer.Left < plat.Right && picPlayer.Right > plat.Right && picPlayer.Bottom > plat.Top + SafetyThreshold)
                {
                    picPlayer.Left = plat.Right;
                    Player.StopMovingLeft();
                }
                // Hitting platform from below
                else if (picPlayer.Top < plat.Bottom && picPlayer.Bottom > plat.Bottom)
                {
                    Player.JumpSpeed = 5; // small bounce back
                    picPlayer.Top = plat.Bottom;
                }
            }
        }

        private void DoorCollisionCheck(Control door)
        {
            if (Player.Picture.Bounds.IntersectsWith(door.Bounds))
            {
                PlayerWon?.Invoke();
            }
        }
    }
}
