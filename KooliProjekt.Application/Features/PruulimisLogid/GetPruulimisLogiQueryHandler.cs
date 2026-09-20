using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class GetPruulimisLogiQueryHandler : IRequestHandler<GetPruulimisLogiQuery, OperationResult<PruulimisLogiDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetPruulimisLogiQueryHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<PruulimisLogiDto>> Handle(GetPruulimisLogiQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PruulimisLogiDto>();
            if (request.Id <= 0) return result;
            var entity = await _context.PruulimisLogid.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity != null) result.Value = PruulimisLogiDto.FromEntity(entity);
            return result;
        }
    }
}
