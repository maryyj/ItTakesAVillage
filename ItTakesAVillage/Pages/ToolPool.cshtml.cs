using ItTakesAVillage.Models;

namespace ItTakesAVillage.Pages;

public class ToolLoanModel(
    UserManager<ItTakesAVillageUser> userManager,
    IGroupService groupService,
    IEventService<ToolPool> toolPoolService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IGroupService _groupService = groupService;
    private readonly IEventService<ToolPool> _toolPoolService = toolPoolService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Models.Group?> GroupsOfCurrentUser { get; set; } = [];
    public List<ToolPool>? Tools { get; set; } = [];
    public async Task<IActionResult> OnGet()
    {
        public void OnGet()
        {
            GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
            ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
            Tools = await _toolPoolService.GetAllOfGroup(CurrentUser.Id);
        }
        return Page();
    }
}
