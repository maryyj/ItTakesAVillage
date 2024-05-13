namespace ItTakesAVillage.Pages;

public class GroupChatModel(
    UserManager<ItTakesAVillageUser> userManager,
    IGroupService groupService,
    IGroupChatService groupChatService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IGroupService _groupService = groupService;
    private readonly IGroupChatService _groupChatService = groupChatService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public Group? CurrentGroup { get; set; }
    public List<UserGroup?> UsersInGroup { get; set; } = [];
    public List<GroupChat> GroupMessages { get; set; } = [];
    [BindProperty]
    public GroupChat NewMessage { get; set; } = new();
    public async Task<IActionResult> OnGet(int groupId)
    {
        if (groupId == 0)
            return RedirectToPage("/Group");

        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            CurrentGroup = await _groupService.Get(groupId);
            UsersInGroup = await _groupService.GetUsersAndGroups(groupId);
            GroupMessages = await _groupChatService.Get(groupId);
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAddMessage(int groupId)
    {
        if (ModelState.IsValid)
        {
            NewMessage.GroupId = groupId;
            bool success = await _groupChatService.Add(NewMessage);
        }

        return RedirectToPage("/GroupChat", new { groupId });
    }
}
