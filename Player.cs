namespace JumpGame
{
    public class Player
    {
        private const int Speed = 10;
        private const int MaxFallSpeed = 22;
        private const int InitialJumpSpeed = -22;
        private static readonly Point StartPosition = new Point(10, 525);

        public PictureBox Picture { get; }
        public int JumpSpeed { get; set; }
        public int Gravity { get; private set; }
        public bool Jumping { get; private set; }
        public bool GoLeft { get; set; }
        public bool GoRight { get; set; }

        private bool lastKeyLeft;
        private bool lastKeyRight;

        public Player(PictureBox picture)
        {
            Picture = picture;
            lastKeyLeft = false;
            lastKeyRight = true;
        }

        public void Reset()
        {
            Picture.Location = StartPosition;
            JumpSpeed = 0;
            Gravity = 2;
            Jumping = false;
            GoLeft = false;
            GoRight = false;

            if (lastKeyLeft)
                FlipHorizontal();

            Picture.BringToFront();
        }

        public void HandleKeyDown(Keys key, bool grounded)
        {
            if (key == Keys.Left)
            {
                GoLeft = true;
                if (lastKeyRight)
                    FlipHorizontal();
                lastKeyRight = false;
                lastKeyLeft = true;
            }

            if (key == Keys.Right)
            {
                GoRight = true;
                if (lastKeyLeft)
                    FlipHorizontal();
                lastKeyRight = true;
                lastKeyLeft = false;
            }

            if ((key == Keys.Space || key == Keys.Up) && grounded && !Jumping)
                Jumping = true;
        }

        public void HandleKeyUp(Keys key)
        {
            if (key == Keys.Left)
                GoLeft = false;
            if (key == Keys.Right)
                GoRight = false;
            if ((key == Keys.Space || key == Keys.Up) && Jumping)
                Jumping = false;
        }

        // Returns true if player fell off the bottom of the screen
        public bool Update(bool grounded, int screenWidth, int screenHeight)
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

            // Track whether a jump is starting this tick so gravity applies immediately
            bool startingJump = Jumping && grounded;
            if (startingJump)
                JumpSpeed = InitialJumpSpeed;

            if (!grounded || startingJump)
            {
                JumpSpeed += Gravity;
                if (JumpSpeed > MaxFallSpeed)
                    JumpSpeed = MaxFallSpeed;
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

        private void FlipHorizontal()
        {
            Picture.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Picture.Refresh();
        }
    }
}
