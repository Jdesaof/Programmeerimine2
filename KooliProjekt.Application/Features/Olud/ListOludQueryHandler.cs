using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Olud
{
    public class ListOludQueryHandler
        : IRequestHandler<ListOludQuery,
            OperationResult<PagedResult<Olu>>>
    {
        private readonly ApplicationDbContext _context;

        public ListOludQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PagedResult<Olu>>> Handle(
            ListOludQuery request,
            CancellationToken cancellationToken)
        {
            System.ArgumentNullException.ThrowIfNull(request);

            var query = _context.Olud.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Nimi))
            {
                var text = request.Nimi.Trim();
                query = query.Where(x => x.Nimi.Contains(text));
            }
            if (!string.IsNullOrWhiteSpace(request.Tuup))
            {
                var value = request.Tuup.Trim();
                query = query.Where(x => x.Tuup == value);
            }

            var result = await query
                .OrderBy(x => x.Id)
                .GetPagedAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken);

            return new OperationResult<PagedResult<Olu>>(result);
        }
    }
}