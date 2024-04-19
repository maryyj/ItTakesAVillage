using Microsoft.AspNetCore.Mvc;

namespace ItTakesAVillage.API.Controllers
{
    public class DinnerInvitationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
