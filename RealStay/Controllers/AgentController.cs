using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealStay.Controllers
{
    public class AgentController : Controller
    {
        [Authorize(Roles = "Agent")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
