using Application.ViewModels.Login;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.IO;

namespace RealStay.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _env;

        public AccountController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IWebHostEnvironment env)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _env = env;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToRoleHome();

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userExist = await _userManager.FindByNameAsync(vm.UserName);
            if (userExist != null)
            {
                ModelState.AddModelError(string.Empty, "El nombre de usuario ya existe.");
                return View(vm);
            }

            var emailExist = await _userManager.FindByEmailAsync(vm.Email);
            if (emailExist != null)
            {
                ModelState.AddModelError(string.Empty, "El correo electrónico ya existe.");
                return View(vm);
            }

            var user = new AppUser
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                UserName = vm.UserName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                IsActive = false // Empieza inactivo hasta que confirme el correo
            };

            // Guardar foto de perfil
            if (vm.ProfilePicture != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.ProfilePicture.FileName)}";
                var folderPath = Path.Combine(_env.WebRootPath, "uploads", "users");
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.ProfilePicture.CopyToAsync(stream);
                }
                user.PathImg = $"/uploads/users/{fileName}";
            }

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, vm.Role);

                // Enviar correo de confirmación
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                
                await _emailService.SendEmailAsync(
                    user.Email!,
                    "Confirmación de Cuenta - RealStay",
                    $"<h1>Bienvenido a RealStay</h1><p>Por favor confirma tu cuenta haciendo clic en el siguiente enlace:</p><a href='{confirmationLink}'>Activar Cuenta</a>"
                );

                TempData["Success"] = "Usuario registrado exitosamente. Por favor revisa tu correo para activar tu cuenta.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null) return RedirectToAction("Index");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
                TempData["Success"] = "Cuenta activada exitosamente. Ya puedes iniciar sesión.";
            }
            else
            {
                TempData["Error"] = "Error al activar la cuenta.";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Index(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToRoleHome();

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByNameAsync(vm.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(vm);
            }

            if (!user.IsActive || (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now))
            {
                ModelState.AddModelError(string.Empty,
                    "Tu cuenta está inactiva. Debes activarla mediante el enlace " +
                    "enviado a tu correo electrónico para poder acceder al sistema.");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(
                vm.UserName, vm.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(vm);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Client"))
                return RedirectToAction("Index", "Client");
            if (roles.Contains("Agent"))
                return RedirectToAction("Index", "Agent");
            if (roles.Contains("Administrator"))
                return RedirectToAction("Index", "Administrator");

            await _signInManager.SignOutAsync();
            ModelState.AddModelError(string.Empty, "Tu usuario no tiene un rol asignado. Contacta al administrador.");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Welcome", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToRoleHome()
        {
            if (User.IsInRole("Client"))
                return RedirectToAction("Index", "Client");
            if (User.IsInRole("Agent"))
                return RedirectToAction("Index", "Agent");
            if (User.IsInRole("Administrator"))
                return RedirectToAction("Index", "Administrator");

            return RedirectToAction("Index");
        }

    }
}
