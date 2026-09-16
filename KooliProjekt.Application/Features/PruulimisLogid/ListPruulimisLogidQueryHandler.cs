using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class ListPruulimisLogidQueryHandler
        : IRequestHandler<ListPruulimisLogidQuery,
            OperationResult<List<PruulimisLogi>>>
    {
        private readonly ApplicationDbContext _context;

        public ListPruulimisLogidQueryHandler(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<PruulimisLogi>>> Handle(
            ListPruulimisLogidQuery request,
            CancellationToken cancellationToken)
        {
            var logid = await _context.PruulimisLogid
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return new OperationResult<List<PruulimisLogi>>(logid);
        }
    }
}