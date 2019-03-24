using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Model;
using FASTCapstonePortal.RequestModels;
using FASTCapstonePortal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FASTCapstonePortal.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {

        private readonly UserManager<CapstoneUser> _userManager;
        private readonly IStudent _studentService;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<CapstoneUser> userManager, IStudent studentService, ITokenService tokenService)
        {
            _userManager = userManager;
            _studentService = studentService;
            _tokenService = tokenService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody][Required] RegisterUser registerUser)
        {
            if (registerUser.Admin) ModelState.Remove("Program");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (_userManager.Users.Any(u => u.UserName == registerUser.UserName)) return BadRequest("Username not available");
            if (_userManager.Users.Any(u => u.Email == registerUser.Email)) return BadRequest("E-mail not available");
            CapstoneUser user = new CapstoneUser { UserName = registerUser.UserName, Admin = registerUser.Admin, Email = registerUser.Email};
                IdentityResult result = await _userManager.CreateAsync(user, registerUser.Password);

                if (result.Succeeded)
                {
                    if (!registerUser.Admin)
                    {
                        await _studentService.CreateAsync(new Student { User = user, FirstName = registerUser.FirstName, LastName = registerUser.LastName, Program = registerUser.Program});
                    }
                    return Ok(new
                    {
                        id = user.Id
                    });
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("User Create Error", error.Description);
                    }
                return BadRequest(ModelState);
                }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody][Required] LoginUser loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            CapstoneUser user = await _userManager.FindByNameAsync(loginUser.UserName);
            if (user == null) return BadRequest("User not found");
            bool result = await _userManager.CheckPasswordAsync(user, loginUser.Password);
            if (!result) return Unauthorized();
            JwtSecurityToken token = _tokenService.GenerateAccessToken(user);
            string refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                refresh_token = refreshToken,
                expiration = token.ValidTo,
                id = user.Id,
                roles = token.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()

            });
        }

        
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody][Required] RefreshToken tokens)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            ClaimsPrincipal principal = new ClaimsPrincipal();
            //To prevent 500, in case a non-JWT string is provided in access_token
            try
            {
                principal = _tokenService.GetPrincipalFromExpiredToken(tokens.Access_Token);
            }
            catch
            {
                return Unauthorized();
            }
            string userid = principal.Identity.Name;
            CapstoneUser user = await _userManager.FindByIdAsync(userid);
            if (user == null || user.RefreshToken != tokens.Refresh_Token) return Unauthorized();
            JwtSecurityToken newToken = _tokenService.GenerateAccessToken(user);
            string newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return Ok(new
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(newToken),
                refresh_token = newRefreshToken,
                expiration = newToken.ValidTo,
                id = user.Id,
                roles = newToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
            });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Revoke()
        {
            CapstoneUser user = await _userManager.FindByIdAsync(User.Identity.Name);
            if (user == null) return BadRequest();
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return Ok("Revoked");
        }
    }
}
