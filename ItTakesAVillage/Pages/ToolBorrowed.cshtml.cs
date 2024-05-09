namespace ItTakesAVillage.Pages;

public class ToolBorrowedModel(
    UserManager<ItTakesAVillageUser> userManager, 
    IGroupService groupService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IGroupService _groupService = groupService;
    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Models.Group?> GroupsOfCurrentUser { get; set; } = [];
    public async Task<IActionResult> OnGet()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
        }
        return Page();
    }
}
