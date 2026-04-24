namespace JumpGame
{
    // Represents one completed game's result. Mirrors one row of the Scores table.
    // Property is named Points (rather than Score) because C# disallows a member
    // with the same name as its enclosing type.
    public class Score
    {
        public string Username { get; set; }
        public int Points { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }

        public Score(string username, int points, int time, DateTime date)
        {
            Username = username;
            Points = points;
            Time = time;
            Date = date;
        }
    }
}
