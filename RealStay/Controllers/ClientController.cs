using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealStay.Controllers
{
    public class ClientController : Controller
    {
        [Authorize(Roles = "Client")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
