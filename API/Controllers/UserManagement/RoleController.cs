using API.Controllers.Common;
using Application.Dtos.UserManagement.Roles;
using Application.Features.UserManagement.Role.Requests.Commands;
using Application.Features.UserManagement.Role.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserManagement
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseApiController
    {
        [HttpGet("role_list")]
        public async Task<IActionResult> GetAllRoles(int page, int perPage, string? search, string? sortBy, string? sortDirection)
        {
            var roles = await _mediator.Send(new GetListOfAllRolesWithParamRequest
            {
                Page = page,
                PerPage = perPage,
                Search = search,
                SortBy = sortBy,
                SortDirection = sortDirection
            });
            return Ok(roles);
        }

        [HttpGet("all-permissions")]
        public async Task<IActionResult> AllPermissions()
        {
            var permissions = await _mediator.Send(new GetAllPermissionListRequest());
            return Ok(permissions);
        }

        [HttpPost("add_role")]
        public async Task<IActionResult> AddRole(AddRoleDto roles)
        {
            await _mediator.Send(new AddRoleCommand{Roles = roles});
            return Ok();
        }

        [HttpPut("update_role")]
        public async Task<IActionResult> UpdateRole(UpdateRoleDto role)
        {
            await _mediator.Send(new UpdateRoleCommand{Role = role});
            return Ok();
        }

        [HttpPost("assign_permissio_to_role")]
        public async Task<IActionResult> AssignPermissionToRole(AssignPermissionsToRoleDto permissions)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new AssignPermissionsToRoleCommand{Permissions = permissions});
                return Ok();
            }
            return BadRequest();
            
        }
        
    }
}