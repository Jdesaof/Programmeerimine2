using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class ListKoostisosadQueryHandler
        : IRequestHandler<ListKoostisosadQuery, OperationResult<PagedResult<Koostisosa>>>
    {
        private readonly ApplicationDbContext _context;

        public ListKoostisosadQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PagedResult<Koostisosa>>> Handle(
            ListKoostisosadQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var result = await _context.Koostisosad
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return new OperationResult<PagedResult<Koostisosa>>(result);
        }
    }
}
