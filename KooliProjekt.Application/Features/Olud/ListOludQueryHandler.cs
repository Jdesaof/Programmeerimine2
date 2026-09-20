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

            var result = await _context.Olud
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .GetPagedAsync(
                    request.Page,
                    request.PageSize,
                    cancellationToken);

            return new OperationResult<PagedResult<Olu>>(result);
        }
    }
}