namespace ItTakesAVillage.Frontend.Pages;

public class PlayDateModel(UserManager<ItTakesAVillageUser> userManager,
    IHttpService httpService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly IHttpService _httpService = httpService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    [BindProperty]
    public PlayDate NewPlayDate { get; set; } = new();
    public List<Notification>? Notifications { get; set; } = [];
    public List<Group>? GroupsOfCurrentUser { get; set; } = [];
    [TempData]
    public bool IsSuccess { get; set; }
    [TempData]
    public bool IsUnsuccessful { get; set; }
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
            IsSuccess = await _httpService.HttpPostRequest("PlayDate", NewPlayDate);
            if(!IsSuccess)
            {
                IsUnsuccessful = true;
            }
        }
        return RedirectToPage("/PlayDate");
    }
    private async Task SetRelatedevent(List<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            var baseEvent = await _httpService.HttpGetRequest<PlayDate>($"PlayDate/{notification.RelatedEvent.Id}");
            if (baseEvent != null)
            {
                notification.RelatedEvent = baseEvent;
            }

        }
    }
}
