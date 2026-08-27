using API.Controllers.Common;
using Application.Dtos.UserManagement.User;
using Application.Features.UserManagement.Requests.Queries;
using Application.Features.UserManagement.User.Requests.Commands;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        [HttpGet("all_users")]
        public async Task<IActionResult> GetUserList(int page, int perPage, string? search, string? sortBy, string? sortDirection)
        {
            var users = await _mediator.Send(new GetListOfAllUsersWithParamRequest
            {
                Page = page,
                PerPage = perPage,
                Search = search,
                SortBy = sortBy,
                SortDirection = sortDirection
            });
            return Ok(users);
        }
        [HttpPost("add_user")]
        public async Task<IActionResult> AddUser(AddUserDto user)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AddUserCommand { User = user });
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("update_user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new UpdateUserCommand { User = user });
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("assign_role_to_user")]
        public async Task<IActionResult> AssignRoleToUser(AssignRolesToUserDto roles)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AssignRolesToUserCommand{Roles = roles});
                return Ok();
            }
            return BadRequest();
            
        }
    }
}