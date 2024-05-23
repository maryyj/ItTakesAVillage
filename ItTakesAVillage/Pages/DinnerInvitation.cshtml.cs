namespace ItTakesAVillage.Pages
{
    public class DinnerInvitationModel(
        UserManager<ItTakesAVillageUser> userManager,
        IHttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly IHttpService _httpService = httpService;

        [BindProperty]
        public DinnerInvitation NewInvitation { get; set; } = new();
        public ItTakesAVillageUser? CurrentUser { get; set; }
        public List<Group>? GroupsOfCurrentUser { get; set; } = [];
        public List<Notification>? Notifications { get; set; } = [];

        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
            {
                GroupsOfCurrentUser = await _httpService.HttpGetRequest<List<Group>>($"Group/GroupsOfUser/{CurrentUser.Id}");
                ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
                Notifications = await _httpService.HttpGetRequest<List<Notification>>($"Notification/All/{CurrentUser.Id}");

                if (Notifications != null)
                    await SetRelatedevent(Notifications);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                bool success = await _httpService.HttpPostRequest("DinnerInvitation/", NewInvitation);
                if (success)
                    await _httpService.HttpPostRequest("Notification", NewInvitation);
            }
            return RedirectToPage("/DinnerInvitation");
        }
        private async Task SetRelatedevent(List<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                var baseEvent = await _httpService.HttpGetRequest<DinnerInvitation>($"DinnerInvitation/{notification.RelatedEvent.Id}");
                if (baseEvent != null)
                {
                    notification.RelatedEvent = baseEvent;
                }
            }
        }
    }
}
