using API.Controllers.Common;
using Application.Features.Dashboard.Requests.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : BaseApiController
    {
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            return Ok(await _mediator.Send(new GetDashboardSummaryRequest()));
        }
    }
}
