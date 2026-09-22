using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class ListPruulimisLogidQueryHandler
        : IRequestHandler<ListPruulimisLogidQuery, OperationResult<PagedResult<PruulimisLogi>>>
    {
        private readonly ApplicationDbContext _context;

        public ListPruulimisLogidQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PagedResult<PruulimisLogi>>> Handle(
            ListPruulimisLogidQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var query = _context.PruulimisLogid.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Kasutaja))
            {
                var text = request.Kasutaja.Trim();
                query = query.Where(x => x.Kasutaja.Contains(text));
            }
            if (request.PartiiId.HasValue)
                query = query.Where(x => x.PartiiId == request.PartiiId.Value);

            var result = await query
                .OrderBy(x => x.Id)
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return new OperationResult<PagedResult<PruulimisLogi>>(result);
        }
    }
}
