using ItTakesAVillage.Contracts;
using ItTakesAVillage.Data;
using ItTakesAVillage.Models;
using ItTakesAVillage.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ItTakesAVillage.Pages
{
    public class DinnerInvitationModel : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager;
        private readonly IEventService<DinnerInvitation> _dinnerInvitationService;
        private readonly INotificationService _notificationService;
        private readonly IGroupService _groupService;

        [BindProperty]
        public DinnerInvitation NewInvitation { get; set; } = new DinnerInvitation();
        public ItTakesAVillageUser? CurrentUser { get; set; }
        public List<Models.Group?> GroupsOfCurrentUser { get; set; } = new List<Group?>();
        public List<Notification> Notifications { get; set; } = new();

        public DinnerInvitationModel(IEventService<DinnerInvitation> dinnerInvitationService,
            UserManager<ItTakesAVillageUser> userManager,
            INotificationService notificationService,
            IGroupService groupService)
        {
            _dinnerInvitationService = dinnerInvitationService;
            _userManager = userManager;
            _notificationService = notificationService;
            _groupService = groupService;
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
                    NewInvitation.CreatorId = CurrentUser.Id;
                    bool success = await _dinnerInvitationService.Create(NewInvitation);
                    if (success)
                        await _notificationService.NotifyGroupAsync(NewInvitation);
                }
            }
            return RedirectToPage("/DinnerInvitation");
        }
    }
}
