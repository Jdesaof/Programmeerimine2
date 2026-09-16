using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Koostisosad;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class KoostisosadController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public KoostisosadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ListKoostisosadQuery(),
                cancellationToken);

            return Result(result);
        }
    }
}