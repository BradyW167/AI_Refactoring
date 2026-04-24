using System.Windows.Forms;

namespace JumpGame
{
    // A platform label that oscillates vertically between two Y limits.
    public class MovingPlatform
    {
        public Label Label { get; }
        public int Speed { get; }
        public int TopLimit { get; }
        public int BottomLimit { get; }
        public bool GoDown { get; private set; }

        public MovingPlatform(Label label, int speed, int topLimit, int bottomLimit)
        {
            Label = label;
            Speed = speed;
            TopLimit = topLimit;
            BottomLimit = bottomLimit;
        }

        public void Update()
        {
            if (GoDown)
                Label.Top += Speed;
            else
                Label.Top -= Speed;

            if (Label.Top < TopLimit)
                GoDown = true;
            else if (Label.Top > BottomLimit)
                GoDown = false;
        }
    }
}
