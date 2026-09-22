using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ListPartiidQuery
        : IRequest<OperationResult<PagedResult<Partii>>>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
        public string Kood { get; set; }
        public int? OluId { get; set; }

    }
}