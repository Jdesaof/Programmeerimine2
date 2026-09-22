using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetPruulimisLogiQuery : IRequest<OperationResult<PruulimisLogiDto>>
    {
        public int Id { get; set; }
    }
}
