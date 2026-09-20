using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class GetMaitsmineQueryHandler : IRequestHandler<GetMaitsmineQuery, OperationResult<MaitsmineDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetMaitsmineQueryHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<MaitsmineDto>> Handle(GetMaitsmineQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<MaitsmineDto>();
            if (request.Id <= 0) return result;
            var entity = await _context.Maitsmised.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity != null) result.Value = MaitsmineDto.FromEntity(entity);
            return result;
        }
    }
}
