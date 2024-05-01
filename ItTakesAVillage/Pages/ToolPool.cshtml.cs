
namespace ItTakesAVillage.Pages;

public class ToolPoolModel(
    UserManager<ItTakesAVillageUser> userManager,
    IGroupService groupService,
    INotificationService notificationService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IGroupService _groupService = groupService;
    private readonly INotificationService _notificationService = notificationService;
    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Models.Group?> GroupsOfCurrentUser { get; set; } = [];
    public List<Notification> Notifications { get; set; } = [];

    [BindProperty]
    public ToolPool NewToolPool { get; set; } = new();
    public async Task<IActionResult> OnGet()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
            Notifications = await _notificationService.GetAsync(CurrentUser.Id);
            //ViewData["GroupId"] = new SelectList(await _groupService.GetGroupsByUserId(CurrentUser.Id), "Id", "Name");
            ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");

        }
        return Page();
    }
}
