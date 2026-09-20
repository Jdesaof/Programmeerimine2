using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.Olud;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class OludController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public OludController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] ListOludQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                query,
                cancellationToken);

            return Result(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOluQuery { Id = id }, cancellationToken);
            return Result(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveOluCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Result(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteOluCommand { Id = id }, cancellationToken);
            return Result(result);
        }
    }
}
