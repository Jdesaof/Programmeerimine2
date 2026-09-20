using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Olud
{
    public class GetOluQueryHandler : IRequestHandler<GetOluQuery, OperationResult<OluDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetOluQueryHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<OluDto>> Handle(GetOluQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<OluDto>();
            if (request.Id <= 0) return result;
            var entity = await _context.Olud.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity != null) result.Value = OluDto.FromEntity(entity);
            return result;
        }
    }
}
