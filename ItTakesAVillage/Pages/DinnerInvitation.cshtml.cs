namespace ItTakesAVillage.Pages
{
    public class DinnerInvitationModel(
        UserManager<ItTakesAVillageUser> userManager,
        INotificationService notificationService,
        IHttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IHttpService _httpService = httpService;

        [BindProperty]
        public DinnerInvitation NewInvitation { get; set; } = new();
        public ItTakesAVillageUser? CurrentUser { get; set; }
        public List<Group>? GroupsOfCurrentUser { get; set; } = [];
        public List<Notification> Notifications { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
            {
                GroupsOfCurrentUser = await _httpService.HttpGetRequest<List<Group>>($"Group/GroupsOfUser/{CurrentUser.Id}");
                ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
                Notifications = await _notificationService.GetAsync(CurrentUser.Id);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                bool success = await _httpService.HttpPostRequest("DinnerInvitation/", NewInvitation);
                if (success)
                    await _notificationService.NotifyGroupAsync(NewInvitation);
            }
            return RedirectToPage("/DinnerInvitation");
        }
    }
}
