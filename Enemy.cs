namespace JumpGame
{
    public class Enemy
    {
        private readonly int speed;
        private bool goRight;

        public PictureBox Picture { get; }
        public Label Platform { get; private set; }

        public Enemy(PictureBox picture, int speed)
        {
            Picture = picture;
            this.speed = speed;
            goRight = true;
            Platform = new Label();
        }

        public void Reset(Label platform)
        {
            if (!goRight)
                FlipHorizontal();
            goRight = true;

            Platform = platform;
            Picture.Location = new Point(platform.Left + Picture.Width, platform.Top - Picture.Height);
            Picture.BringToFront();
        }

        public void Update()
        {
            if (goRight)
                Picture.Left += speed;
            else
                Picture.Left -= speed;

            if (Picture.Left < Platform.Left)
            {
                goRight = true;
                FlipHorizontal();
            }
            else if (Picture.Right > Platform.Right)
            {
                goRight = false;
                FlipHorizontal();
            }
        }

        private void FlipHorizontal()
        {
            Picture.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Picture.Refresh();
        }
    }
}
