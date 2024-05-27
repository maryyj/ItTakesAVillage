namespace ItTakesAVillage.Frontend.Pages
{
    public class IndexModel(UserManager<ItTakesAVillageUser> userManager,
        IHttpService httpService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly IHttpService _httpService = httpService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        public List<Notification>? Notifications { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            //CurrentUser = await _userManager.GetUserAsync(User);
            var user = await _userManager.GetUserAsync(User);
            CurrentUser = await _httpService.HttpGetRequest<ItTakesAVillageUser>($"User/{user?.Id}");
            if (CurrentUser == null)
                return Redirect("/Identity/Account/Register");
            Notifications = await _httpService.HttpGetRequest<List<Notification>>($"Notification/All/{CurrentUser.Id}");
            if (Notifications != null)
                await SetRelatedevent(Notifications);
            return Page();
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
