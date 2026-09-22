using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetMaitsmineQuery : IRequest<OperationResult<MaitsmineDto>>
    {
        public int Id { get; set; }
    }
}
