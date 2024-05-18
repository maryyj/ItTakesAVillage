namespace ItTakesAVillage.Models
{
    public class GroupEvent
    {
        public int Id { get; set; }
        public string CreatorId { get; set; } = string.Empty;
        public ItTakesAVillageUser? Creator { get; set; }
        public int GroupId { get; set; }
        public DateTime DateTime { get; set; }
        public string? Message { get; set; }
        public string? ChildName { get; set; }
        public string? InvitedChildName { get; set; }
        public string? Location { get; set; }
        public string? Course { get; set; }
    }
}
