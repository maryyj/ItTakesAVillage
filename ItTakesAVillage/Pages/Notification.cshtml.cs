namespace ItTakesAVillage.Pages
{
    public class NotificationModel(UserManager<ItTakesAVillageUser> userManager,
        INotificationService notificationService,
        IHttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IHttpService _httpService = httpService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }
        public List<Notification>? Notifications { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
            {
                Notifications = await _notificationService.GetAsync(CurrentUser.Id);
                //Notifications = await _httpService.HttpGetRequest<List<Notification>>($"Notification/All/{CurrentUser.Id}");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostHandleAccordionClick([FromBody] int notificationId)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (notificationId != 0 && CurrentUser != null)
            {
                var notification = await _httpService.HttpGetRequest<Notification>($"Notification/{notificationId}");
                if (notification != null)
                    await _httpService.HttpPutRequest($"Notification", notification);

                int unreadNotificationCount = await _httpService.HttpGetRequest<int>($"Notification/Count/{CurrentUser.Id}");

                return new JsonResult(new { success = true, unreadCount = unreadNotificationCount });
            }
            return new JsonResult(new { success = false });
        }
    }
}
