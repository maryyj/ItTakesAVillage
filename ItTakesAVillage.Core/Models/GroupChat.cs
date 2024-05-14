namespace ItTakesAVillage.Core.Models
{
    public class GroupChat
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int GroupId { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public Group? Group { get; set; }
        public ItTakesAVillageUser? User { get; set; }
    }
}
