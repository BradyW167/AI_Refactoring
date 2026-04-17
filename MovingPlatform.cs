namespace JumpGame
{
    public class MovingPlatform
    {
        private readonly int speed;
        private readonly int topLimit;
        private readonly int bottomLimit;
        private bool goDown;

        public Label Label { get; }

        public MovingPlatform(Label label, int speed, int topLimit, int bottomLimit)
        {
            Label = label;
            this.speed = speed;
            this.topLimit = topLimit;
            this.bottomLimit = bottomLimit;
            goDown = false;
        }

        public void Update()
        {
            if (goDown)
                Label.Top += speed;
            else
                Label.Top -= speed;

            if (Label.Top < topLimit)
                goDown = true;
            else if (Label.Top > bottomLimit)
                goDown = false;
        }
    }
}
