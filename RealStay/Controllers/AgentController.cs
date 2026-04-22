using Application.DTOs.Administrator.Improvements;
using Application.DTOs.Administrator.Properties;
using Application.DTOs.Administrator.Sales;
using Application.DTOs.Agent;
using Application.Interfaces;
using Application.Interfaces.Agent;
using Application.ViewModels.Agent.Profile;
using Application.ViewModels.Agent.Properties;
using Application.ViewModels.Messages;
using Application.ViewModels.Offers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealStay.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentController : Controller
    {
        private readonly IAgentService _agentService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IImprovementService _improvementService;

        public AgentController(
            IAgentService agentService,
            IPropertyTypeService propertyTypeService,
            ISaleTypeService saleTypeService,
            IImprovementService improvementService)
        {
            _agentService = agentService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
            _improvementService = improvementService;
        }

        public async Task<IActionResult> Index()
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            var properties = await _agentService.GetPropertiesByAgentIdAsync(agentId);
            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> Properties()
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            var properties = await _agentService.GetPropertiesByAgentIdAsync(agentId);
            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProperty()
        {
            try
            {
                var propertyTypes = await _propertyTypeService.GetAllAsync();
                var typeSales = await _saleTypeService.GetAllAsync();
                var improvements = await _improvementService.GetAllAsync();

                if (!propertyTypes.Any() || !typeSales.Any() || !improvements.Any())
                {
                    TempData["Error"] = "No existen tipos de propiedades, tipos de ventas o mejoras creadas. Contacte al administrador.";
                    return RedirectToAction("Properties");
                }

                ViewData["PropertyTypes"] = propertyTypes;
                ViewData["TypeSales"] = typeSales;
                ViewData["Improvements"] = improvements;

                return View(new CreateAgentPropertyViewModel());
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Properties");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProperty(CreateAgentPropertyViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(agentId))
                    return RedirectToAction("Index", "Account");

                await _agentService.CreatePropertyAsync(model, agentId);
                TempData["Success"] = "Propiedad creada exitosamente";
                return RedirectToAction("Properties");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProperty(string id)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            try
            {
                var property = await _agentService.GetPropertyByIdAsync(id, agentId);

                await LoadPropertyLookupsAsync();
                ViewBag.PropertyId = id;

                var editModel = new EditAgentPropertyViewModel
                {
                    PropertyTypeId = property.PropertyTypeId,
                    TypeSaleId = property.TypeSaleId,
                    Price = property.Price,
                    Description = property.Description,
                    Meters = property.Meters,
                    NumberRooms = property.NumberRooms,
                    NumberBaths = property.NumberBaths,
                    Location = property.Location,
                    ImprovementIds = property.ImprovementIds,
                    ExistingImages = property.ImageUrls
                };
                return View(editModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Properties");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProperty(string id, EditAgentPropertyViewModel model)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            if (!ModelState.IsValid)
            {
                await LoadPropertyLookupsAsync();
                ViewBag.PropertyId = id;
                return View(model);
            }

            try
            {
                await _agentService.UpdatePropertyAsync(id, agentId, model);
                TempData["Success"] = "Propiedad actualizada exitosamente";
                return RedirectToAction("Properties");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadPropertyLookupsAsync();
                ViewBag.PropertyId = id;
                return View(model);
            }
        }

        private async Task LoadPropertyLookupsAsync()
        {
            ViewData["PropertyTypes"] = await _propertyTypeService.GetAllAsync();
            ViewData["TypeSales"] = await _saleTypeService.GetAllAsync();
            ViewData["Improvements"] = await _improvementService.GetAllAsync();
        }

        [HttpGet]
        public async Task<IActionResult> DeleteProperty(string id)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            try
            {
                await _agentService.DeletePropertyAsync(id, agentId);
                TempData["Success"] = "Propiedad eliminada exitosamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Properties");
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            try
            {
                var profile = await _agentService.GetAgentProfileAsync(agentId);
                var viewModel = new AgentProfileViewModel
                {
                    Id = profile.Id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    PhoneNumber = profile.Phone,
                    CurrentImage = profile.PathImg
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(AgentProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(agentId))
                    return RedirectToAction("Index", "Account");

                var updateModel = new UpdateAgentProfileDto
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Image = model.Image
                };

                await _agentService.UpdateAgentProfileAsync(agentId, updateModel);
                TempData["Success"] = "Perfil actualizado exitosamente";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> PropertyDetails(string id)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            try
            {
                var property = await _agentService.GetPropertyByIdAsync(id, agentId);
                return View(property);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Properties");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Chats()
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            var chats = await _agentService.GetChatsForAgentAsync(agentId);
            return View(chats);
        }

        [HttpGet]
        public async Task<IActionResult> ChatDetail(string propertyId, string clientId)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            var messages = await _agentService.GetMessagesByPropertyAsync(propertyId, clientId, agentId);

            ViewData["PropertyId"] = propertyId;
            ViewData["ClientId"] = clientId;
            ViewData["AgentId"] = agentId;

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(SaveMessageViewModel vm)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "El mensaje no puede estar vacío.";
                return RedirectToAction("ChatDetail", new { propertyId = vm.PropertyId, clientId = vm.ReceiverId });
            }

            try
            {
                vm.SenderId = agentId;
                await _agentService.SendMessageAsync(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ChatDetail", new { propertyId = vm.PropertyId, clientId = vm.ReceiverId });
        }

        [HttpGet]
        public async Task<IActionResult> Offers()
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            var offers = await _agentService.GetOffersByAgentAsync(agentId);
            return View(offers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptOffer(string offerId)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            try
            {
                await _agentService.AcceptOfferAsync(offerId);
                TempData["Success"] = "Oferta aceptada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Offers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectOffer(string offerId)
        {
            var agentId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(agentId))
                return RedirectToAction("Index", "Account");

            try
            {
                await _agentService.RejectOfferAsync(offerId);
                TempData["Success"] = "Oferta rechazada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Offers");
        }
    }
}