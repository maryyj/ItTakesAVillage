namespace ItTakesAVillage.Pages
{
    public class IndexModel(UserManager<ItTakesAVillageUser> userManager,
        INotificationService notificationService) : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
        private readonly INotificationService _notificationService = notificationService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        public List<Notification> Notifications { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return Redirect("/Identity/Account/Register");
            
            Notifications = await _notificationService.GetAsync(CurrentUser.Id);
            return Page();
        }
    }
}
