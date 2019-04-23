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
using System.Text;

namespace AnkiSyncServer.Controllers
{

    [Route("sync/hostKey")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        //private readonly IUserManager _userManager;

        public TokenController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] AuthRequest authUserRequest)
        {
            // Assume user is valid for testing
            if (authUserRequest.Username.Equals("foo", StringComparison.Ordinal))
            {
                // Check PWD
                if (authUserRequest.Password.Equals("bar", StringComparison.Ordinal))
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, authUserRequest.Username),
                        new Claim(JwtRegisteredClaimNames.Jti, "1")
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                        _config["Tokens:Issuer"],
                        claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                    return Ok(new
                    {
                        key = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
            }

            return BadRequest("Could not create token");
        }
    }
}