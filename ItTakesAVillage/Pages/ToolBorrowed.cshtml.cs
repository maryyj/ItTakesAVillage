namespace ItTakesAVillage.Pages;

public class ToolBorrowedModel(
    UserManager<ItTakesAVillageUser> userManager,
    IGroupService groupService,
    IEventService<ToolLoan> toolLoanService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IGroupService _groupService = groupService;
    private readonly IEventService<ToolLoan> _toolLoanService = toolLoanService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Models.Group?> GroupsOfCurrentUser { get; set; } = [];
    public List<ToolLoan>? BorrowedTools { get; set; } = [];
    [BindProperty]
    public ToolLoan BorrowedTool { get; set; } = new();
    public async Task<IActionResult> OnGet()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
            BorrowedTools = await _toolLoanService.GetAllOfGroup(CurrentUser.Id);
        }
        return Page();
    }
    public async Task<IActionResult> OnPostReturnTool(int toolId)
    {
        BorrowedTools = await _toolLoanService.GetAllOfGroup(CurrentUser.Id);
        var loan = BorrowedTools.Find(x => x.ToolId == toolId);
        if (loan != null)
        {
            await _toolLoanService.Update(loan);
        }
        return RedirectToPage("/ToolBorrowed");
    }
}
