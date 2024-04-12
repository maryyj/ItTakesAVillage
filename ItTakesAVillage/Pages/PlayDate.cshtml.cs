using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ItTakesAVillage.Pages
{
    public class PlayDateModel : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager;
        private readonly IGroupService _groupService;
        private readonly INotificationService _notificationService;
        private readonly IEventService<PlayDate> _playDateService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public PlayDate NewPlayDate { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();
        public List<Models.Group?> GroupsOfCurrentUser { get; set; } = new();
        public PlayDateModel(UserManager<ItTakesAVillageUser> userManager,
            IGroupService groupService,
            INotificationService notificationService,
            IEventService<PlayDate> playDateService)
        {
            _userManager = userManager;
            _groupService = groupService;
            _notificationService = notificationService;
            _playDateService = playDateService;
        }
        public async Task<IActionResult> OnGet()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser != null)
            {
                ViewData["GroupId"] = new SelectList(await _groupService.GetGroupsByUserId(CurrentUser.Id), "Id", "Name");
                GroupsOfCurrentUser = await _groupService.GetGroupsByUserId(CurrentUser.Id);
                ViewData["GroupId"] = new SelectList(GroupsOfCurrentUser, "Id", "Name");
                Notifications = await _notificationService.GetAsync(CurrentUser.Id);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                CurrentUser = await _userManager.GetUserAsync(User);
                if (CurrentUser != null)
                {
                    NewPlayDate.CreatorId = CurrentUser.Id;
                    bool success = await _playDateService.Create(NewPlayDate);
                    if (success)
                        await _notificationService.NotifyGroupAsync(NewPlayDate);
                }
            }
            return RedirectToPage("/PlayDate");
        }
    }
}
