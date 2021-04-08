namespace Models.DTO
{
    public class RemoveTeamFromEventDto
    {
        public int TeamId { get; set; }
        public int EventId { get; set; }
    }


    public class CreateTeamForEventDto
    {
        public int EventId { get; set; }
    }


    public class AddOrRemoveTeamCaptainDto
    {
        public int EventId { get; set; }
        public int TeamId { get; set; }
        public string CaptainId { get; set; }
    }
}
