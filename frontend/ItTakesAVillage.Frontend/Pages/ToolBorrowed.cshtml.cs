namespace ItTakesAVillage.Pages;

public class ToolBorrowedModel(
    UserManager<ItTakesAVillageUser> userManager,
    IHttpService httpService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IHttpService _httpService = httpService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Group>? GroupsOfCurrentUser { get; set; } = [];
    public List<ToolLoan>? BorrowedTools { get; set; } = [];
    [BindProperty]
    public ToolLoan? BorrowedTool { get; set; } = new();
    public async Task<IActionResult> OnGet()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {

            GroupsOfCurrentUser = await _httpService.HttpGetRequest<List<Group>>($"Group/GroupsOfUser/{CurrentUser.Id}");
            BorrowedTools = await _httpService.HttpGetRequest<List<ToolLoan>>($"ToolLoan/AllForUserGroup/" + CurrentUser.Id);

            if (BorrowedTools != null)
                await SetCreator(BorrowedTools);
        }
        return Page();
    }
    public async Task<IActionResult> OnPostReturnTool(int toolId)
    {
        BorrowedTool = await _httpService.HttpGetRequest<ToolLoan>($"ToolLoan/" + toolId);
        if (BorrowedTool != null)
        {
            await _httpService.HttpPutRequest<ToolLoan>("ToolLoan", BorrowedTool);
        }
        //TODO: add notification for creator that tool is returned
        return RedirectToPage("/ToolBorrowed");
    }
    private async Task SetCreator(List<ToolLoan> borrowedTools)
    {
        foreach (var tool in borrowedTools)
        {
            var creator = await _userManager.FindByIdAsync(tool.ToolPool.CreatorId);
            tool.ToolPool.Creator = creator;
        }
    }
}
