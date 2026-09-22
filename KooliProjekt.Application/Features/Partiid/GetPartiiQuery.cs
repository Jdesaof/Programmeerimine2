using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetPartiiQuery : IRequest<OperationResult<PartiiDto>>
    {
        public int Id { get; set; }
    }
}
