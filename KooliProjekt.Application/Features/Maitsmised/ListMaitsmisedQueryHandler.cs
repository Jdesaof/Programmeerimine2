using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class ListMaitsmisedQueryHandler
        : IRequestHandler<ListMaitsmisedQuery,
            OperationResult<List<Maitsmine>>>
    {
        private readonly ApplicationDbContext _context;

        public ListMaitsmisedQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Maitsmine>>> Handle(
            ListMaitsmisedQuery request,
            CancellationToken cancellationToken)
        {
            var maitsmised = await _context.Maitsmised
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return new OperationResult<List<Maitsmine>>(maitsmised);
        }
    }
}