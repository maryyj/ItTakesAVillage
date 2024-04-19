using Microsoft.AspNetCore.Mvc;

namespace ItTakesAVillage.API.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
