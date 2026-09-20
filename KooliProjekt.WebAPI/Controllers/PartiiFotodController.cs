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
            [FromQuery] ListPartiiFotodQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Result(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPartiiFotoQuery { Id = id }, cancellationToken);
            return Result(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SavePartiiFotoCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Result(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeletePartiiFotoCommand { Id = id }, cancellationToken);
            return Result(result);
        }
    }
}
