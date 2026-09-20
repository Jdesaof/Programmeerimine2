using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    public class GetPartiiQuery : IRequest<OperationResult<PartiiDto>>
    {
        public int Id { get; set; }
    }
}
