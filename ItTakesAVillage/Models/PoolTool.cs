namespace ItTakesAVillage.Models;

public class PoolTool : BaseEvent
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public bool IsBorrowed { get; set; }
    public string? BorrowerId { get; set; } = string.Empty;
    public ItTakesAVillageUser? Borrower { get; set; }
    public string? Image { get; set; }
}
