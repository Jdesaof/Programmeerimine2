using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.PartiiFotod;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class PartiiFotodController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PartiiFotodController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ListPartiiFotodQuery(),
                cancellationToken);

            return Result(result);
        }
    }
}