using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookStore_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        readonly SignInManager<IdentityUser> _signInManager;
        readonly UserManager<IdentityUser> _userManager;
        readonly IConfiguration _configuration;
        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn([FromBody] UserDTO userDTO)
        {
            try
            {
                var userName = userDTO.EmailAddress;
                var password = userDTO.Password;
                var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
                if (result != null && result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(userName);
                    var tokenstring = await GenerateJsonWebToken(user);
                    return Ok(new { token = tokenstring });
                }
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message + "-" + ex.InnerException);
            }
            return Unauthorized(userDTO);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                var userName = userDTO.EmailAddress;
                var password = userDTO.Password;
                var user = new IdentityUser { Email = userName, UserName = userName };
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return InternalError("User creation failed");
                }
                await _userManager.AddToRoleAsync(user, "Customer");
                return Created("login",new { result.Succeeded });
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message + "-" + ex.InnerException);
            }
        }
        private async Task<string> GenerateJsonWebToken(IdentityUser identityUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credintial = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,identityUser.Email),
                new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier,identityUser.Id)
            };
            var roles = await _userManager.GetRolesAsync(identityUser);
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"], claims, null, expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credintial);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private IActionResult InternalError(string message)
        {
            return StatusCode(500, "Something wrong. Please contact the Admin");
        }
    }
}
