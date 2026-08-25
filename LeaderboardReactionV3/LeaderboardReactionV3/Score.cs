namespace LeaderboardReactionV3
{
    public class Score
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int Milliseconds { get; set; }

    }

    public class CreateScoreDto
    {
        public string PlayerName { get; set; }
        public int Milliseconds { get; set; }
    }
}
