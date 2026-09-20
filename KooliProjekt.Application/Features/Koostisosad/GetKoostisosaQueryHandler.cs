using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class GetKoostisosaQueryHandler : IRequestHandler<GetKoostisosaQuery, OperationResult<KoostisosaDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetKoostisosaQueryHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<KoostisosaDto>> Handle(GetKoostisosaQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<KoostisosaDto>();
            if (request.Id <= 0) return result;
            var entity = await _context.Koostisosad.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity != null) result.Value = KoostisosaDto.FromEntity(entity);
            return result;
        }
    }
}
