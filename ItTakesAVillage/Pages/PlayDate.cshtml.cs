namespace ItTakesAVillage.Pages;

public class PlayDateModel(UserManager<ItTakesAVillageUser> userManager,
    INotificationService notificationService,
    HttpService httpService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly HttpService _httpService = httpService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    [BindProperty]
    public PlayDate NewPlayDate { get; set; } = new();
    public List<Notification> Notifications { get; set; } = [];
    public List<Group>? GroupsOfCurrentUser { get; set; } = [];

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
            bool success = await _httpService.HttpPostRequest("PlayDate" , NewPlayDate);
            if (success)
                await _notificationService.NotifyGroupAsync(NewPlayDate);
        }
        return RedirectToPage("/PlayDate");
    }
}
