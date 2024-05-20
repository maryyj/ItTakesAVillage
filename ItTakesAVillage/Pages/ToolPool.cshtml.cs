namespace ItTakesAVillage.Pages;

public class ToolLoanModel(
    UserManager<ItTakesAVillageUser> userManager,
    IGroupService groupService,
    IEventService<ToolPool> toolPoolService,
    IEventService<ToolLoan> toolLoanService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IGroupService _groupService = groupService;
    private readonly IEventService<ToolPool> _toolPoolService = toolPoolService;
    private readonly IEventService<ToolLoan> _toolLoanService = toolLoanService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Models.Group?> GroupsOfCurrentUser { get; set; } = [];
    public List<ToolPool>? Tools { get; set; } = [];
    [BindProperty]
    public ToolPool? Tool { get; set; }
    [BindProperty]
    public ToolLoan NewToolLoan { get; set; } = new();

    [BindProperty]
    public ToolPool EditTool { get; set; } = new();
    [BindProperty]
    public IFormFile? UploadedImage { get; set; }
    public async Task<IActionResult> OnGet()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
            ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
            Tools = await _toolPoolService.GetAllOfGroup(CurrentUser.Id);
        }
        return Page();
    }
    public async Task<IActionResult> OnPostRemoveToolFromPool(int toolId)
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            bool success = await _toolPoolService.Delete(toolId);
        }
        return RedirectToPage("/ToolPool");
    }
    public async Task<IActionResult> OnPostBorrowTool()
    {
        if (ModelState.IsValid)
        {
            Tools = await _toolPoolService.GetAllOfGroup(NewToolLoan.BorrowerId);
            var tool = Tools.Find(x => x.Id == NewToolLoan.ToolId);
            NewToolLoan.ToolPool = tool;
            bool success = await _toolLoanService.Create(NewToolLoan);
        }
        return RedirectToPage("/ToolPool");
    }
    public async Task<IActionResult> OnPostEditTool()
    {
        if (ModelState.IsValid)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            EditTool.Creator = CurrentUser;
            bool success = await _toolPoolService.Update(EditTool);
        }
        return RedirectToPage("/ToolPool");
    }
}
