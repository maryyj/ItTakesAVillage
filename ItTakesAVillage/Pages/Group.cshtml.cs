namespace ItTakesAVillage.Pages
{
    public class GroupModel(
        IGroupService groupService, 
        UserManager<ItTakesAVillageUser> userManager) : PageModel
    {
        private readonly IGroupService _groupService = groupService;
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public Group NewGroup { get; set; } = new();
        [BindProperty]
        public UserGroup NewUserGroup { get; set; } = new();
        public List<Group?> GroupsOfCurrentUser { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
            {
                var allUsers = _userManager.Users.Where(x => x.Id != CurrentUser.Id).ToList();
                ViewData["UserId"] = new SelectList(allUsers, "Id", "Email");
                GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
                ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostNewGroupAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid && CurrentUser != null)
            {
                NewGroup.CreatorId = CurrentUser.Id;
                var newGroupId = await _groupService.Save(NewGroup, CurrentUser.Id);

                if (newGroupId != 0)
                {
                    await _groupService.AddUser(CurrentUser.Id, newGroupId);
                }
            }
            return RedirectToPage("/Group");
        }
    }
}