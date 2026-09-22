using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetPartiiFotoQuery : IRequest<OperationResult<PartiiFotoDto>>
    {
        public int Id { get; set; }
    }
}
