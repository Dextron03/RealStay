using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealStay.Controllers
{
    public class AdministratorController : Controller
    {
        [Authorize(Roles ="Administrator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
