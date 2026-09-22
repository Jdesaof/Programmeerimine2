using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetKoostisosaQuery : IRequest<OperationResult<KoostisosaDto>>
    {
        public int Id { get; set; }
    }
}
