using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Common
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator Mediator;
        protected IMediator _mediator => Mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}