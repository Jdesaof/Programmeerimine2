using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Features.PruulimisLogid;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class PruulimisLogidController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PruulimisLogidController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List(
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ListPruulimisLogidQuery(),
                cancellationToken);

            return Result(result);
        }
    }
}