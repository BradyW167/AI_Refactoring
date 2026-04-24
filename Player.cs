using System.Drawing;
using System.Windows.Forms;

namespace JumpGame
{
    // Owns player movement, jumping, gravity, and sprite orientation.
    // Collision detection (including grounded checks) lives in frmMain, which
    // sets Grounded/JumpSpeed/position via this object during its collision pass.
    public class Player
    {
        private const int Speed = 10;
        private const int InitialJumpSpeed = -22;
        private const int MaxFallSpeed = 22;
        private const int GravityAmount = 2;

        public PictureBox Picture { get; }

        // Vertical velocity. Read/written by collision code in frmMain.
        public int JumpSpeed { get; set; }

        // Set by frmMain's collision pass each tick.
        public bool Grounded { get; set; }

        public bool Jumping { get; private set; }

        private bool GoLeft { get; set; }
        private bool GoRight { get; set; }

        private bool LastKeyLeft { get; set; }
        private bool LastKeyRight { get; set; }

        public Player(PictureBox picture)
        {
            Picture = picture;
            LastKeyRight = true;
        }

        // Places player at start, clears movement state, and faces right.
        public void Reset(Point startLocation)
        {
            Picture.Location = startLocation;

            JumpSpeed = 0;
            Grounded = false;
            Jumping = false;
            GoLeft = false;
            GoRight = false;

            if (LastKeyLeft)
            {
                FlipSprite();
            }
            LastKeyLeft = false;
            LastKeyRight = true;

            Picture.BringToFront();
        }

        public void OnLeftDown()
        {
            GoLeft = true;
            if (LastKeyRight)
            {
                FlipSprite();
            }
            LastKeyRight = false;
            LastKeyLeft = true;
        }

        public void OnRightDown()
        {
            GoRight = true;
            if (LastKeyLeft)
            {
                FlipSprite();
            }
            LastKeyRight = true;
            LastKeyLeft = false;
        }

        public void OnLeftUp() => GoLeft = false;
        public void OnRightUp() => GoRight = false;

        public void OnJumpDown()
        {
            if (Grounded && !Jumping)
            {
                Jumping = true;
            }
        }

        public void OnJumpUp()
        {
            if (Jumping)
            {
                Jumping = false;
            }
        }

        // Cancels horizontal intent. Used by frmMain's side-collision handling
        // so the player stops walking into a platform.
        public void StopMovingRight() => GoRight = false;
        public void StopMovingLeft() => GoLeft = false;

        // Applies horizontal movement, jump start, gravity, and top clamp.
        // Returns true if the player has fallen off the bottom of the screen,
        // so the caller can trigger death.
        public bool Update(int screenWidth, int screenHeight)
        {
            if (GoLeft)
            {
                Picture.Left -= Speed;
                if (Picture.Left < 0)
                {
                    Picture.Left = 0;
                    GoLeft = false;
                }
            }

            if (GoRight)
            {
                Picture.Left += Speed;
                if (Picture.Right > screenWidth)
                {
                    Picture.Left = screenWidth - Picture.Width;
                    GoRight = false;
                }
            }

            if (Jumping && Grounded)
            {
                JumpSpeed = InitialJumpSpeed;
                Grounded = false;
            }

            if (!Grounded)
            {
                JumpSpeed += GravityAmount;
                if (JumpSpeed > MaxFallSpeed) JumpSpeed = MaxFallSpeed;
                Picture.Top += JumpSpeed;
            }
            else
            {
                JumpSpeed = 0;
            }

            if (Picture.Top < 0)
            {
                Picture.Top = 0;
                JumpSpeed = 0;
            }
            else if (Picture.Top > screenHeight)
            {
                return true;
            }

            return false;
        }

        private void FlipSprite()
        {
            Picture.Image?.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Picture.Refresh();
        }
    }
}
