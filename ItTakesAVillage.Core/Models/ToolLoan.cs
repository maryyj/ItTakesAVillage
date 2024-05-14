namespace ItTakesAVillage.Models
{
    public class ToolLoan
    {
        public int Id { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string? BorrowerId { get; set; } = string.Empty;
        public int ToolId { get; set; }
        public bool IsReturned { get; set; }
        public ItTakesAVillageUser? Borrower { get; set; }
        public ToolPool? ToolPool { get; set; }
    }
}
