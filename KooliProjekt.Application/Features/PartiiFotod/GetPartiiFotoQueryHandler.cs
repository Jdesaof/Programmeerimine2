using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class GetPartiiFotoQueryHandler : IRequestHandler<GetPartiiFotoQuery, OperationResult<PartiiFotoDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetPartiiFotoQueryHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<PartiiFotoDto>> Handle(GetPartiiFotoQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PartiiFotoDto>();
            if (request.Id <= 0) return result;
            var entity = await _context.PartiiFotod.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity != null) result.Value = PartiiFotoDto.FromEntity(entity);
            return result;
        }
    }
}
