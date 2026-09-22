using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class GetOluQuery : IRequest<OperationResult<OluDto>>
    {
        public int Id { get; set; }
    }
}
