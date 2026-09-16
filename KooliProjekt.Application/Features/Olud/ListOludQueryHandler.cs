using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Olud
{
    public class ListOludQueryHandler
        : IRequestHandler<ListOludQuery, OperationResult<List<Olu>>>
    {
        private readonly ApplicationDbContext _context;

        public ListOludQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Olu>>> Handle(
            ListOludQuery request,
            CancellationToken cancellationToken)
        {
            var olud = await _context.Olud
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return new OperationResult<List<Olu>>(olud);
        }
    }
}