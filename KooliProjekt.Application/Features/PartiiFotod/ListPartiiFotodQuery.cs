using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ListPartiiFotodQuery
        : IRequest<OperationResult<PagedResult<PartiiFoto>>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string FailiTee { get; set; }
        public int? PartiiId { get; set; }

    }
}
