using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Maitsmised;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class MaitsmisedController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public MaitsmisedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ListMaitsmisedQuery(),
                cancellationToken);

            return Result(result);
        }
    }
}