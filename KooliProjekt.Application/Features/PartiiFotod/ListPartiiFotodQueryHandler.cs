using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class ListPartiiFotodQueryHandler
        : IRequestHandler<ListPartiiFotodQuery,
            OperationResult<List<PartiiFoto>>>
    {
        private readonly ApplicationDbContext _context;

        public ListPartiiFotodQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<PartiiFoto>>> Handle(
            ListPartiiFotodQuery request,
            CancellationToken cancellationToken)
        {
            var fotod = await _context.PartiiFotod
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            return new OperationResult<List<PartiiFoto>>(fotod);
        }
    }
}