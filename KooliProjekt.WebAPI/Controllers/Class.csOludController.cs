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
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ListOludQuery(),
                cancellationToken);

            return Result(result);
        }
    }
}