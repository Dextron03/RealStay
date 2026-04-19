using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStay.Models;
using Application.Interfaces.Properties;
using Application.ViewModels.Properties;
using Application.Interfaces;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace RealStay.Controllers
{

    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Property> _propertyRepository;
        private readonly Application.Interfaces.Agent.IAgentService _agentService;

        public HomeController(
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            ISaleTypeService saleTypeService,
            UserManager<AppUser> userManager,
            IGenericRepository<Property> propertyRepository,
            Application.Interfaces.Agent.IAgentService agentService)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
            _userManager = userManager;
            _propertyRepository = propertyRepository;
            _agentService = agentService;
        }

        public async Task<IActionResult> Welcome()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Agent"))
                    return RedirectToAction("Index", "Agent");

                if (User.IsInRole("Client"))
                    return RedirectToAction("Index", "Client");

                if (User.IsInRole("Administrator"))
                    return RedirectToAction("Index", "Administrator");
            }

            var allProperties = await _propertyRepository.GetAllAsync();
            ViewBag.TotalProperties = allProperties.Count(p => p.Status == PropertyStatus.Available.ToString());
            ViewBag.TotalSold = allProperties.Count(p => p.Status == PropertyStatus.Sold.ToString());

            var agents = await _userManager.GetUsersInRoleAsync("Agent");
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            ViewBag.TotalAgents = agents.Count;
            ViewBag.TotalClients = clients.Count;

            return View();
        }

        public async Task<IActionResult> Index(PropertyFilterViewModel filters)
        {
            List<PropertyViewModel> properties;

            // Si algún filtro tiene valor, filtramos; si no, traemos todas
            if (!string.IsNullOrEmpty(filters.TypeSaleName) ||
                !string.IsNullOrEmpty(filters.PropertyTypeId) ||
                filters.MinPrice.HasValue ||
                filters.MaxPrice.HasValue ||
                filters.Rooms.HasValue ||
                filters.Bathrooms.HasValue)
            {
                properties = await _propertyService.FilterAsync(filters);
            }
            else
            {
                properties = await _propertyService.GetAllAsync();
            }

            ViewData["PropertyTypes"] = await _propertyTypeService.GetAllAsync();
            ViewData["SaleTypes"] = await _saleTypeService.GetAllAsync();
            ViewData["Filters"] = filters;

            return View(properties);
        }

        public async Task<IActionResult> Agents(string name)
        {
            var agents = await _agentService.SearchAgentsByNameAsync(name);
            ViewData["SearchName"] = name;
            return View(agents);
        }

        public async Task<IActionResult> AgentProperties(string agentId)
        {
            var properties = await _propertyService.GetByAgentAsync(agentId);
            var agent = await _agentService.GetAgentProfileAsync(agentId);
            ViewData["AgentName"] = $"{agent.FirstName} {agent.LastName}";
            return View(properties);
        }

        private string GetClientId() =>
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;


        [Authorize]
        public async Task<IActionResult> PropertyDetails(string id)
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null) return NotFound();
            return View(property);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}