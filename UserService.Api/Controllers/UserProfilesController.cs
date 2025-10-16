using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Application.Dtos;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController(IUserService userService) : ControllerBase
    {
        // GET: api/userprofiles
        [Authorize(Roles = "Admin")]
        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<UserProfileDto>>> GetAllUserProfiles()
        {
            var users = await userService.GetAllUserProfileAsync();
            return Ok(users);
        }

        // GET: api/userprofiles/getbyemail?email={email}

        [Authorize]
        [HttpGet("getbyemail")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileByEmail()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
                return Unauthorized();

            var user = await userService.GetUserProfileByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found");

            return Ok(user);
        }

        // GET: api/userprofiles/getbyid/{userId}
        [Authorize]
        [HttpGet("getbyid")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfileById()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || !Guid.TryParse(userId, out var userGuid))
                return Unauthorized();

            var user = await userService.GetUserProfileByGuidAsync(userGuid);
            if (user == null)
                return BadRequest("User not found!");

            return Ok(user);
        }

        // PUT: api/userprofiles/update-display-name
        [Authorize]
        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateUserProfileName([FromBody]UserProfileDto updateDto)
        {
            var result = await userService.UpdateUserProfileAsync(updateDto);
            if(!result)
                return BadRequest("Failed to update prifle");

            return Ok("User Profile updated successfully");
        }
    }
}
