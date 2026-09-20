using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class ListMaitsmisedQuery
        : IRequest<OperationResult<PagedResult<Maitsmine>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
