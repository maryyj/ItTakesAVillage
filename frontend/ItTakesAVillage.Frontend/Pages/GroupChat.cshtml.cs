namespace ItTakesAVillage.Frontend.Pages;

public class GroupChatModel(
    UserManager<ItTakesAVillageUser> userManager,
    IHttpService httpService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IHttpService _httpService = httpService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public ItTakesAVillageUser? MemberOfGroup { get; set; }
    public Group? CurrentGroup { get; set; }
    public List<UserGroup>? UsersInGroup { get; set; } = [];
    public List<GroupChat>? GroupMessages { get; set; } = [];
    [BindProperty]
    public GroupChat NewMessage { get; set; } = new();
    public async Task<IActionResult> OnGet(int groupId)
    {
        if (groupId == 0)
            return RedirectToPage("/Group");

        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            CurrentGroup = await _httpService.HttpGetRequest<Group>($"Group/{groupId}");
            UsersInGroup = await _httpService.HttpGetRequest<List<UserGroup>>($"Group/UsersGroup/{groupId}");
            MemberOfGroup = UserExistsInGroup(UsersInGroup, CurrentUser);

            GroupMessages = await _httpService.HttpGetRequest<List<GroupChat>>($"GroupChat/{groupId}");
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAddMessage(int groupId)
    {
        if (ModelState.IsValid)
        {
            NewMessage.GroupId = groupId;
            bool success = await _httpService.HttpPostRequest("GroupChat",NewMessage);
        }

        return RedirectToPage("/GroupChat", new { groupId });
    }
    private ItTakesAVillageUser UserExistsInGroup(List<UserGroup> usersInGroup, ItTakesAVillageUser currentUser)
    {
        var member = usersInGroup.Find(x => x.UserId == currentUser.Id);
        if (member?.User == null)
        {
            return null;
        }
        else
        {
            return member.User;
        }
    }
}
