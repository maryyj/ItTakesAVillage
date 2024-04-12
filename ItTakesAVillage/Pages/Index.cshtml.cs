using ItTakesAVillage.Contracts;
using ItTakesAVillage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ItTakesAVillage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ItTakesAVillageUser> _userManager;
        private readonly INotificationService _notificationService;

        public ItTakesAVillageUser? CurrentUser { get; set; }
        public List<Notification> Notifications { get; set; } = new();


        public IndexModel(UserManager<ItTakesAVillageUser> userManager, 
            INotificationService notificationService)
        {
            _userManager = userManager;
            _notificationService = notificationService;
        }

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
