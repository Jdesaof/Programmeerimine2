using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    public class ListOludQuery
        : IRequest<OperationResult<PagedResult<Olu>>>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}