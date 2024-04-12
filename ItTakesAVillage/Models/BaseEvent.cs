namespace ItTakesAVillage.Models
{
    public class BaseEvent
    {
        public int Id { get; set; }
        public string CreatorId { get; set; } = string.Empty;
        public ItTakesAVillageUser? Creator { get; set; } 
        public int GroupId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Message { get; set; }
    }
}