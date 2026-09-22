using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ListKoostisosadQuery
        : IRequest<OperationResult<PagedResult<Koostisosa>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string Nimetus { get; set; }
        public int? PartiiId { get; set; }

    }
}
