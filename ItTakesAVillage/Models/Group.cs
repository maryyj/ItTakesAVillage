namespace ItTakesAVillage.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CreatorId { get; set; } = string.Empty;
        public ICollection<UserGroup>? UserGroups { get; set; }
    }
}
