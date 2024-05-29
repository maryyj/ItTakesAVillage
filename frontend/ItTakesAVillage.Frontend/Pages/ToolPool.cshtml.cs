namespace ItTakesAVillage.Frontend.Pages;

public class ToolLoanModel(
    UserManager<ItTakesAVillageUser> userManager,
    IHttpService httpService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IHttpService _httpService = httpService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Group>? GroupsOfCurrentUser { get; set; } = [];
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
            GroupsOfCurrentUser = await _httpService.HttpGetRequest<List<Group>>($"Group/GroupsOfUser/{CurrentUser.Id}");
            ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
            Tools = await _httpService.HttpGetRequest<List<ToolPool>>($"ToolPool/AllForUserGroup/{CurrentUser.Id}");
        }
        return Page();
    }
    public async Task<IActionResult> OnPostRemoveToolFromPool(int toolId)
    {
        if (toolId != 0)
        {
            bool success = await _httpService.HttpDeleteRequest<ToolPool>($"ToolPool/{toolId}");
        }

        return RedirectToPage("/ToolPool");
    }
    public async Task<IActionResult> OnPostBorrowTool()
    {
        if (ModelState.IsValid)
        {
            bool success = await _httpService.HttpPostRequest($"ToolLoan", NewToolLoan);
        }
        return RedirectToPage("/ToolPool");
    }
    public async Task<IActionResult> OnPostEditTool()
    {
        if (ModelState.IsValid)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            EditTool.Creator = CurrentUser;
            bool success = await _httpService.HttpPutRequest($"ToolPool", EditTool);
        }
        return RedirectToPage("/ToolPool");
    }
}
