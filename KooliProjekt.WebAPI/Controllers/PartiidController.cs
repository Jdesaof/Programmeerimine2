using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Partiid;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class PartiidController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PartiidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ListPartiidQuery(),
                cancellationToken);

            return Result(result);
        }
    }
}