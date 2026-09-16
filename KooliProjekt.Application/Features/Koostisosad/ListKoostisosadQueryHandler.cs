using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class ListKoostisosadQueryHandler
        : IRequestHandler<ListKoostisosadQuery,
            OperationResult<List<Koostisosa>>>
    {
        private readonly ApplicationDbContext _context;

        public ListKoostisosadQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Koostisosa>>> Handle(
            ListKoostisosadQuery request,
            CancellationToken cancellationToken)
        {
            var koostisosad = await _context.Koostisosad
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return new OperationResult<List<Koostisosa>>(koostisosad);
        }
    }
}