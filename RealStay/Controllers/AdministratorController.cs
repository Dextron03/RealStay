using Application.DTOs.Administrator.Improvements;
using Application.DTOs.Administrator.Properties;
using Application.DTOs.Administrator.Sales;
using Application.DTOs.Administrator.User;
using Application.Interfaces;
using Application.ViewModels.Administrator.Improvements;
using Application.ViewModels.Administrator.Properties;
using Application.ViewModels.Administrator.Sales;
using Application.ViewModels.Administrator.User;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

    namespace RealStay.Controllers
    {
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly IAdminUserService _adminUserService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IImprovementService _improvementService;
        private readonly UserManager<AppUser> _userManager;

        public AdministratorController(
            IAdminDashboardService dashboardService, IAdminUserService adminUserService, IPropertyTypeService propertyTypeService, ISaleTypeService saleTypeService, IImprovementService improvementService, UserManager<AppUser> userManager)
        {
            _dashboardService = dashboardService;
            _adminUserService = adminUserService;
            _propertyTypeService = propertyTypeService;
            _saleTypeService = saleTypeService;
            _improvementService = improvementService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService.GetDashboardDataAsync();
            return View(data);
        }

        public async Task<IActionResult> Agents()
        {
            var agents = await _adminUserService.GetAllAgentsAsync();
            return View(agents);
        }

        public async Task<IActionResult> ToggleAgentActive(string id)
        {
            await _adminUserService.ToggleActiveAsync(id);
            return RedirectToAction(nameof(Agents));
        }

        public async Task<IActionResult> DeleteAgent(string id)
        {
            var agents = await _adminUserService.GetAllAgentsAsync();
            var agent = agents.FirstOrDefault(a => a.Id == id);
            if (agent == null) return RedirectToAction(nameof(Agents));
            return View(agent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAgentConfirmed(string id)
        {
            await _adminUserService.DeleteAgentAsync(id);
            return RedirectToAction(nameof(Agents));
        }

        public async Task<IActionResult> Admins()
        {
            var admins = await _adminUserService.GetAllAdminsAsync();
            return View(admins);
        }

        public IActionResult CreateAdmin()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(CreateUserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                var dto = new CreateUserDto
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    UserName = vm.UserName,
                    Cedula = vm.Cedula,
                    Email = vm.Email,
                    Password = vm.Password,
                    ConfirmPassword = vm.ConfirmPassword
                };
                await _adminUserService.CreateAdminAsync(dto);
                return RedirectToAction(nameof(Admins));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> EditAdmin(string id)
        {
            var admins = await _adminUserService.GetAllAdminsAsync();
            var admin = admins.FirstOrDefault(a => a.Id == id);
            if (admin == null) return RedirectToAction(nameof(Admins));
            var vm = new EditUserViewModel
            {
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                UserName = admin.UserName,
                Cedula = admin.Cedula,
                Email = admin.Email,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
            ViewBag.UserId = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdmin(string id, EditUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                return View(vm);
            }
            try
            {
                var dto = new EditUserDto
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    UserName = vm.UserName,
                    Cedula = vm.Cedula,
                    Email = vm.Email,
                    Password = vm.Password,
                    ConfirmPassword = vm.ConfirmPassword
                };
                await _adminUserService.UpdateUserAsync(id, dto);
                return RedirectToAction(nameof(Admins));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.UserId = id;
                return View(vm);
            }
        }

        public async Task<IActionResult> ToggleAdminActive(string id)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (id == currentUserId) return RedirectToAction(nameof(Admins));
            await _adminUserService.ToggleActiveAsync(id);
            return RedirectToAction(nameof(Admins));
        }

        public async Task<IActionResult> Developers()
        {
            var developers = await _adminUserService.GetAllDevelopersAsync();
            return View(developers);
        }

        public IActionResult CreateDeveloper()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDeveloper(CreateUserViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                var dto = new CreateUserDto
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    UserName = vm.UserName,
                    Cedula = vm.Cedula,
                    Email = vm.Email,
                    Password = vm.Password,
                    ConfirmPassword = vm.ConfirmPassword
                };
                await _adminUserService.CreateDeveloperAsync(dto);
                return RedirectToAction(nameof(Developers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> EditDeveloper(string id)
        {
            var developers = await _adminUserService.GetAllDevelopersAsync();
            var developer = developers.FirstOrDefault(a => a.Id == id);
            if (developer == null) return RedirectToAction(nameof(Developers));
            var vm = new EditUserViewModel
            {
                FirstName = developer.FirstName,
                LastName = developer.LastName,
                UserName = developer.UserName,
                Cedula = developer.Cedula,
                Email = developer.Email,
                Password = string.Empty,
                ConfirmPassword = string.Empty
            };
            ViewBag.UserId = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDeveloper(string id, EditUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                return View(vm);
            }
            try
            {
                var dto = new EditUserDto
                {
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    UserName = vm.UserName,
                    Cedula = vm.Cedula,
                    Email = vm.Email,
                    Password = vm.Password,
                    ConfirmPassword = vm.ConfirmPassword
                };
                await _adminUserService.UpdateUserAsync(id, dto);
                return RedirectToAction(nameof(Developers));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.UserId = id;
                return View(vm);
            }
        }

        public async Task<IActionResult> ToggleDeveloperActive(string id)
        {
            await _adminUserService.ToggleActiveAsync(id);
            return RedirectToAction(nameof(Developers));
        }

        public async Task<IActionResult> PropertyTypes()
        {
            var types = await _propertyTypeService.GetAllAsync();
            return View(types);
        }

        public IActionResult CreatePropertyType()
        {
            return View(new CreatePropertyViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePropertyType(CreatePropertyViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                var dto = new CreatePropertyDto
                {
                    TypeName = vm.ImprovementName,
                    Description = vm.Description
                };
                await _propertyTypeService.CreateAsync(dto);
                return RedirectToAction(nameof(PropertyTypes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> EditPropertyType(string id)
        {
            var type = await _propertyTypeService.GetByIdAsync(id);
            if (type == null) return RedirectToAction(nameof(PropertyTypes));
            var vm = new EditPropetyViewModel
            {
                TypeName = type.TypeName,
                Description = type.Description
            };
            ViewBag.TypeId = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPropertyType(string id, EditPropetyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TypeId = id;
                return View(vm);
            }
            try
            {
                var dto = new EditPropertyDto
                {
                    TypeName = vm.TypeName,
                    Description = vm.Description
                };
                await _propertyTypeService.UpdateAsync(id, dto);
                return RedirectToAction(nameof(PropertyTypes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.TypeId = id;
                return View(vm);
            }
        }

        public async Task<IActionResult> DeletePropertyType(string id)
        {
            var type = await _propertyTypeService.GetByIdAsync(id);
            if (type == null) return RedirectToAction(nameof(PropertyTypes));
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePropertyTypeConfirmed(string id)
        {
            await _propertyTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(PropertyTypes));
        }

        public async Task<IActionResult> SaleTypes()
        {
            var types = await _saleTypeService.GetAllAsync();
            return View(types);
        }

        public IActionResult CreateSaleType()
        {
            return View(new CreateTypeSaleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSaleType(CreateTypeSaleViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                var dto = new CreateTypeSaleDto
                {
                    TypeName = vm.TypeName,
                    Description = vm.Description
                };
                await _saleTypeService.CreateAsync(dto);
                return RedirectToAction(nameof(SaleTypes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> EditSaleType(string id)
        {
            var type = await _saleTypeService.GetByIdAsync(id);
            if (type == null) return RedirectToAction(nameof(SaleTypes));
            var vm = new EditTypeSaleViewModel
            {
                TypeName = type.TypeName,
                Description = type.Description
            };
            ViewBag.TypeId = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSaleType(string id, EditTypeSaleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TypeId = id;
                return View(vm);
            }
            try
            {
                var dto = new EditTypeSaleDto
                {
                    TypeName = vm.TypeName,
                    Description = vm.Description
                };
                await _saleTypeService.UpdateAsync(id, dto);
                return RedirectToAction(nameof(SaleTypes));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.TypeId = id;
                return View(vm);
            }
        }

        public async Task<IActionResult> DeleteSaleType(string id)
        {
            var type = await _saleTypeService.GetByIdAsync(id);
            if (type == null) return RedirectToAction(nameof(SaleTypes));
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSaleTypeConfirmed(string id)
        {
            await _saleTypeService.DeleteAsync(id);
            return RedirectToAction(nameof(SaleTypes));
        }

        public async Task<IActionResult> Improvements()
        {
            var improvements = await _improvementService.GetAllAsync();
            return View(improvements);
        }

        public IActionResult CreateImprovement()
        {
            return View(new CreateImprovementsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImprovement(CreateImprovementsViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            try
            {
                var dto = new CreateImprovementDto
                {
                    ImprovementName = vm.ImprovementName,
                    Description = vm.Description
                };
                await _improvementService.CreateAsync(dto);
                return RedirectToAction(nameof(Improvements));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> EditImprovement(string id)
        {
            var improvement = await _improvementService.GetByIdAsync(id);
            if (improvement == null) return RedirectToAction(nameof(Improvements));
            var vm = new EditImprovementsViewModel
            {
                ImprovementName = improvement.ImprovementName,
                Description = improvement.Description
            };
            ViewBag.ImprovementId = id;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditImprovement(string id, EditImprovementsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ImprovementId = id;
                return View(vm);
            }
            try
            {
                var dto = new EditImprovementDto
                {
                    ImprovementName = vm.ImprovementName,
                    Description = vm.Description
                };
                await _improvementService.UpdateAsync(id, dto);
                return RedirectToAction(nameof(Improvements));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.ImprovementId = id;
                return View(vm);
            }
        }

        public async Task<IActionResult> DeleteImprovement(string id)
        {
            var improvement = await _improvementService.GetByIdAsync(id);
            if (improvement == null) return RedirectToAction(nameof(Improvements));
            return View(improvement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImprovementConfirmed(string id)
        {
            await _improvementService.DeleteAsync(id);
            return RedirectToAction(nameof(Improvements));
        }
    }
}
