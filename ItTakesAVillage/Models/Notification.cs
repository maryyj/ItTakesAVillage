namespace ItTakesAVillage.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public BaseEvent RelatedEvent { get; set; } = new BaseEvent();
    }
}
