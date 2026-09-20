using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class ListMaitsmisedQueryHandler
        : IRequestHandler<ListMaitsmisedQuery, OperationResult<PagedResult<Maitsmine>>>
    {
        private readonly ApplicationDbContext _context;

        public ListMaitsmisedQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PagedResult<Maitsmine>>> Handle(
            ListMaitsmisedQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var result = await _context.Maitsmised
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return new OperationResult<PagedResult<Maitsmine>>(result);
        }
    }
}
