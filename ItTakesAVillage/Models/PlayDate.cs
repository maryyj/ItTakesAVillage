namespace ItTakesAVillage.Models
{
    public class PlayDate : BaseEvent
    {
        public string? ChildName { get; set; }
        public string? InvitedChildName { get; set; }
        public string? Location { get; set; }
    }
}
