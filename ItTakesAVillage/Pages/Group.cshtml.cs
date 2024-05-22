namespace ItTakesAVillage.Pages
{
    public class GroupModel(
        UserManager<ItTakesAVillageUser> userManager,
        HttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly HttpService _httpService = httpService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public Group NewGroup { get; set; } = new();
        [BindProperty]
        public UserGroup NewUserGroup { get; set; } = new();
        public List<Group>? GroupsOfCurrentUser { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
            {
                GroupsOfCurrentUser = await _httpService.HttpGetRequest<List<Group>>($"Group/GroupsOfUser/{CurrentUser.Id}");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostNewGroupAsync()
        {
            if (ModelState.IsValid)
            {
                bool success = await _httpService.HttpPostRequest($"Group/CreateGroup/{NewGroup.CreatorId}", NewGroup);
            }
            return RedirectToPage("/Group");
        }
    }
}