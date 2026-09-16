using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Partiid
{
    public class ListPartiidQueryHandler
        : IRequestHandler<ListPartiidQuery, OperationResult<List<Partii>>>
    {
        private readonly ApplicationDbContext _context;

        public ListPartiidQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Partii>>> Handle(
            ListPartiidQuery request,
            CancellationToken cancellationToken)
        {
            var partiid = await _context.Partiid
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return new OperationResult<List<Partii>>(partiid);
        }
    }
}