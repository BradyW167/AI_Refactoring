using System.Drawing;
using System.Windows.Forms;

namespace JumpGame
{
    // An enemy that paces left/right across the bounds of a platform label.
    public class Enemy
    {
        public PictureBox Picture { get; }
        public int Speed { get; }
        public Label Platform { get; private set; }
        public bool GoRight { get; private set; } = true;

        public Enemy(PictureBox picture, int speed)
        {
            Picture = picture;
            Speed = speed;
            Platform = new Label(); // placeholder until Reset is called
        }

        // Places the enemy on the given platform, resets direction to right,
        // and brings its sprite in front of coin graphics.
        public void Reset(Label platform)
        {
            Platform = platform;

            int top = platform.Top - Picture.Height;
            int left = platform.Left + Picture.Width;
            Picture.Location = new Point(left, top);

            if (!GoRight)
            {
                FlipSprite();
            }
            GoRight = true;

            Picture.BringToFront();
        }

        // Steps the enemy one tick; flips at the platform edges.
        public void Update()
        {
            if (GoRight)
                Picture.Left += Speed;
            else
                Picture.Left -= Speed;

            if (Picture.Left < Platform.Left)
            {
                GoRight = true;
                FlipSprite();
            }
            else if (Picture.Right > Platform.Right)
            {
                GoRight = false;
                FlipSprite();
            }
        }

        private void FlipSprite()
        {
            Picture.Image?.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Picture.Refresh();
        }
    }
}
