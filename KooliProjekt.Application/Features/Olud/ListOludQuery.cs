using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ListOludQuery
        : IRequest<OperationResult<PagedResult<Olu>>>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
        public string Nimi { get; set; }
        public string Tuup { get; set; }

    }
}