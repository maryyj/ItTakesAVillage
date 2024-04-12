using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace ItTakesAVillage.Pages
{
    public class NotificationModel : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager;
        private readonly INotificationService _notificationService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }
        public List<Notification> Notifications { get; set; } = new();
        public NotificationModel(UserManager<ItTakesAVillageUser> userManager,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);

            if (CurrentUser != null)
                Notifications = await _notificationService.GetAsync(CurrentUser.Id);

            return Page();
        }
        public async Task<IActionResult> OnPostHandleAccordionClick([FromBody] int notificationId)
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (notificationId != 0 && CurrentUser != null)
            {
                await _notificationService.UpdateIsReadAsync(notificationId);
                int unreadNotificationCount = await _notificationService.CountAsync(CurrentUser.Id);

                return new JsonResult(new { success = true, unreadCount = unreadNotificationCount });
            }

            return new JsonResult(new { success = false });
        }
    }
}
