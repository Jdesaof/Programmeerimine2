using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class GetMaitsmineQuery : IRequest<OperationResult<MaitsmineDto>>
    {
        public int Id { get; set; }
    }
}
