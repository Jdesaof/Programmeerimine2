using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Partiid
{
    public class ListPartiidQueryHandler
        : IRequestHandler<ListPartiidQuery,
            OperationResult<PagedResult<Partii>>>
    {
        private readonly ApplicationDbContext _context;

        public ListPartiidQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PagedResult<Partii>>> Handle(
            ListPartiidQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var result = await _context.Partiid
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .GetPagedAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken);

            return new OperationResult<PagedResult<Partii>>(result);
        }
    }
}