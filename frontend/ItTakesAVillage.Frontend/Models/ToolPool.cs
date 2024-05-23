namespace ItTakesAVillage.Frontend.Models;

public class ToolPool : BaseEvent
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public bool IsBorrowed { get; set; }
}
