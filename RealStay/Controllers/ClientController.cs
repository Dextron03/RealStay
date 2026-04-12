using Application.Interfaces.Message;
using Application.Interfaces.Offer;
using Application.Interfaces.Properties;
using Application.ViewModels.Messages;
using Application.ViewModels.Offers;
using Application.ViewModels.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealStay.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IWishListService _wishListService;
        private readonly IOfferService _offerService;
        private readonly IMessageService _messageService;

        public ClientController(
            IPropertyService propertyService,
            IWishListService wishListService,
            IOfferService offerService,
            IMessageService messageService)
        {
            _propertyService = propertyService;
            _wishListService = wishListService;
            _offerService = offerService;
            _messageService = messageService;
        }

        private string GetClientId() =>
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

        // ── Propiedades ────────────────────────────────────────────────────────
        public async Task<IActionResult> Index(PropertyFilterViewModel filters)
        {
            List<PropertyViewModel> properties;

            if (filters.TypeSaleName != null || filters.Rooms.HasValue || filters.Bathrooms.HasValue)
                properties = await _propertyService.FilterAsync(filters);
            else
                properties = await _propertyService.GetAllAsync();

            ViewData["Filters"] = filters;
            return View(properties);
        }

        [HttpGet]
        public async Task<IActionResult> PropertyDetails(string id)
        {
            try
            {
                var property = await _propertyService.GetByIdAsync(id);
                return View(property);
            }
            catch
            {
                TempData["Error"] = "Propiedad no encontrada.";
                return RedirectToAction("Index");
            }
        }

        // ── Favoritos ──────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> MyProperties()
        {
            var clientId = GetClientId();
            var wishlist = await _wishListService.GetWishListAsync(clientId);
            return View(wishlist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishList(string propertyId)
        {
            var clientId = GetClientId();
            await _wishListService.AddToWishListAsync(clientId, propertyId);
            TempData["Success"] = "Propiedad agregada a favoritos.";
            return RedirectToAction("PropertyDetails", new { id = propertyId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromWishList(string propertyId, string returnTo = "MyProperties")
        {
            var clientId = GetClientId();
            await _wishListService.RemoveFromWishListAsync(clientId, propertyId);
            TempData["Success"] = "Propiedad eliminada de favoritos.";

            if (returnTo == "Details")
                return RedirectToAction("PropertyDetails", new { id = propertyId });

            return RedirectToAction("MyProperties");
        }

        // ── Ofertas ────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeOffer(SaveOfferViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "El monto de la oferta no es válido.";
                return RedirectToAction("PropertyDetails", new { id = vm.PropertyId });
            }

            try
            {
                vm.UserId = GetClientId();
                await _offerService.CreateAsync(vm);
                TempData["Success"] = "Oferta enviada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("PropertyDetails", new { id = vm.PropertyId });
        }

        [HttpGet]
        public async Task<IActionResult> MyOffers()
        {
            var clientId = GetClientId();
            var offers = await _offerService.GetByUserAsync(clientId);
            return View(offers);
        }

        // ── Mensajes ───────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<IActionResult> Chats()
        {
            var clientId = GetClientId();
            var chats = await _messageService.GetChatsForUserAsync(clientId);
            return View(chats);
        }

        [HttpGet]
        public async Task<IActionResult> ChatDetail(string propertyId, string agentId)
        {
            var clientId = GetClientId();
            var messages = await _messageService.GetMessagesByPropertyAsync(propertyId, clientId, agentId);

            ViewData["PropertyId"] = propertyId;
            ViewData["AgentId"] = agentId;
            ViewData["ClientId"] = clientId;

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(SaveMessageViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "El mensaje no puede estar vacío.";
                return RedirectToAction("ChatDetail", new { propertyId = vm.PropertyId, agentId = vm.ReceiverId });
            }

            try
            {
                vm.SenderId = GetClientId();
                await _messageService.SendMessageAsync(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("ChatDetail", new { propertyId = vm.PropertyId, agentId = vm.ReceiverId });
        }
    }
}
