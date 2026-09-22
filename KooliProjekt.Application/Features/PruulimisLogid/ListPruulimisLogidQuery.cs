using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ListPruulimisLogidQuery
        : IRequest<OperationResult<PagedResult<PruulimisLogi>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string Kasutaja { get; set; }
        public int? PartiiId { get; set; }

    }
}
