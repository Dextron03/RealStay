using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStay.Models;
using Application.Interfaces.Properties;
using Application.ViewModels.Properties;
using Application.Interfaces;

namespace RealStay.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;

        public HomeController(
            IPropertyService propertyService,
            IPropertyTypeService propertyTypeService,
            ISaleTypeService saleTypeService)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
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
