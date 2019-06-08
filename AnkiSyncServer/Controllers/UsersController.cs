using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AnkiSyncServer.Models;
using AnkiSyncServer.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AnkiSyncServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController (UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result);
        }
    }
}