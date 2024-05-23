namespace ItTakesAVillage.Pages;

public class ToolPoolModel(
    UserManager<ItTakesAVillageUser> userManager,
    INotificationService notificationService,
    IHttpService httpService) : PageModel
{
    private readonly UserManager<ItTakesAVillageUser> _userManager = userManager;
    private readonly INotificationService _notificationService = notificationService;
    private readonly IHttpService _httpService = httpService;

    public ItTakesAVillageUser? CurrentUser { get; set; }
    public List<Group>? GroupsOfCurrentUser { get; set; } = [];
    public List<Notification> Notifications { get; set; } = [];

    [BindProperty]
    public ToolPool NewToolPool { get; set; } = new();
    [BindProperty]
    public IFormFile? UploadedImage { get; set; }
    public async Task<IActionResult> OnGet()
    {
        CurrentUser = await _userManager.GetUserAsync(User);
        if (CurrentUser != null)
        {
            GroupsOfCurrentUser = await _httpService.HttpGetRequest<List<Group>>($"Group/GroupsOfUser/{CurrentUser.Id}");
            Notifications = await _notificationService.GetAsync(CurrentUser.Id);
            ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");

        }
        return Page();
    }
    public async Task<IActionResult> OnPostAddToolToPool()
    {
        if (ModelState.IsValid)
        {
            if (UploadedImage != null)
            {
                var uniqueFileName = await SetUniqueFileName(UploadedImage);
                NewToolPool.Image = uniqueFileName;
            }
            bool success = await _httpService.HttpPostRequest("ToolPool", NewToolPool);
            if (success)
                await _httpService.HttpPostRequest("Notification", NewToolPool);
            //await _notificationService.NotifyGroupAsync(NewToolPool);
        }
        return RedirectToPage("/ToolPool");
    }
    private static async Task<string> SetUniqueFileName(IFormFile uploadedImage)
    {
        var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadedImage.FileName;
        var uploads = Path.Combine("wwwroot/uploadedImg", uniqueFileName);
        if (!Directory.Exists(uploads))
        {
            Directory.CreateDirectory(uploads);
        }
        var filePath = Path.Combine(uploads, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await uploadedImage.CopyToAsync(fileStream);
        }
        return uniqueFileName;
    }
}
