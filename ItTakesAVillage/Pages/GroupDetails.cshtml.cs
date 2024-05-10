namespace ItTakesAVillage.Pages
{
    public class GroupDetailsModel : PageModel
    {
        private readonly IGroupService _groupService;
        private readonly UserManager<ItTakesAVillageUser> _userManager;

        public GroupDetailsModel(IGroupService groupService, UserManager<ItTakesAVillageUser> userManager)
        {
            _groupService = groupService;
            _userManager = userManager;
        }
        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public UserGroup NewUserGroup { get; set; } = new();
        public List<UserGroup?> UsersInGroup { get; set; } = [];
        public int GroupId { get; set; }
        public async Task<IActionResult> OnGet(int groupId)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
            {
                GroupId = groupId;
                UsersInGroup = await _groupService.GetUsersAndGroups(groupId);
                var allUsers = _userManager.Users.Where(x => x.Id != CurrentUser.Id).ToList();
                ViewData["UserId"] = new SelectList(allUsers, "Id", "Email");
                //GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveFromGroup(string userId, int groupId)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
                await _groupService.RemoveUser(userId, groupId);
            return RedirectToPage("/Group");
        }
        public async Task<IActionResult> OnPostAddUserToGroupAsync(int groupId)
        {
            if (ModelState.IsValid)
            {
                await _groupService.AddUser(NewUserGroup.UserId, groupId);
            }
            return RedirectToPage("/Group"); //TODO: Redirecct to correct page
        }
    }
}
