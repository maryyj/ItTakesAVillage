namespace ItTakesAVillage.Frontend.Pages
{
    public class NotificationModel(UserManager<ItTakesAVillageUser> userManager,
        IHttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
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
                Notifications = await _httpService.HttpGetRequest<List<Notification>>($"Notification/All/{CurrentUser.Id}");
                if (Notifications != null)
                    await SetRelatedevent(Notifications);
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
        private async Task SetRelatedevent(List<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                var playDateEvent = await _httpService.HttpGetRequest<PlayDate>($"PlayDate/{notification.RelatedEvent.Id}");
                var dinnerEvent = await _httpService.HttpGetRequest<DinnerInvitation>($"DinnerInvitation/{notification.RelatedEvent.Id}");
                var toolPoolEvent = await _httpService.HttpGetRequest<ToolPool>($"ToolPool/{notification.RelatedEvent.Id}");
                if (playDateEvent != null)
                {
                    notification.RelatedEvent = playDateEvent;
                }
                if (dinnerEvent != null)
                {
                    notification.RelatedEvent = dinnerEvent;
                }
                if (toolPoolEvent != null)
                {
                    notification.RelatedEvent = toolPoolEvent;
                }
            }
        }
    }
}
