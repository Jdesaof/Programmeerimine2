using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class ListPartiiFotodQueryHandler
        : IRequestHandler<ListPartiiFotodQuery, OperationResult<PagedResult<PartiiFoto>>>
    {
        private readonly ApplicationDbContext _context;

        public ListPartiiFotodQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<PagedResult<PartiiFoto>>> Handle(
            ListPartiiFotodQuery request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var query = _context.PartiiFotod.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.FailiTee))
            {
                var text = request.FailiTee.Trim();
                query = query.Where(x => x.FailiTee.Contains(text));
            }
            if (request.PartiiId.HasValue)
                query = query.Where(x => x.PartiiId == request.PartiiId.Value);

            var result = await query
                .OrderBy(x => x.Id)
                .GetPagedAsync(request.Page, request.PageSize, cancellationToken);

            return new OperationResult<PagedResult<PartiiFoto>>(result);
        }
    }
}
