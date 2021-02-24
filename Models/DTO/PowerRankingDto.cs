namespace Models.DTO
{

    public class CreateRankingDto
    {
        public int EventId { get; set; }
        public string UserId { get; set; }
        public int Handicap { get; set; }
        public int Rank { get; set; }
        public string Trending { get; set; }
        public string Writeup { get; set; }
    }
}
