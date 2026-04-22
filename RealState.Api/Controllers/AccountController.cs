using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs.Administrator.User;
using Application.DTOs.Login;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace RealStay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);
            if (user == null)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password!, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Credenciales incorrectas" });

            var token = await GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("register-developer")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RegisterDeveloper([FromBody] CreateUserDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                IdentityNumber = model.Cedula,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Developer");  
            return Ok(new { message = "Usuario desarrollador creado exitosamente" });
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "Administrator")]  
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                IdentityNumber = model.Cedula,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Administrator"); 
            return Ok(new { message = "Usuario administrador creado exitosamente" });
        }

        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]!);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}