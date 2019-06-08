using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AnkiSyncServer.Model;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using AnkiSyncServer.Models;
using AnkiSyncServer.ViewModels;
using System.Text;

namespace AnkiSyncServer.Controllers
{

    [Route("sync/[controller]")]
    [ApiController]
    public class HostKeyController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public HostKeyController(
            IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
        )
        {
            config = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel authUserRequest)
        {
            ApplicationUser user = await userManager.FindByNameAsync(authUserRequest.Username);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, authUserRequest.Password, isPersistent: false, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
                    SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    JwtSecurityToken token = new JwtSecurityToken(config["Tokens:Issuer"],
                        config["Tokens:Issuer"],
                        claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                    return Ok(new
                    {
                        key = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }

            return BadRequest("Invalid username or password.");
        }
    }
}