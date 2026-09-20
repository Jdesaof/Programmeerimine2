using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Partiid
{
    public class GetPartiiQueryHandler : IRequestHandler<GetPartiiQuery, OperationResult<PartiiDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetPartiiQueryHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<PartiiDto>> Handle(GetPartiiQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PartiiDto>();
            if (request.Id <= 0) return result;
            var entity = await _context.Partiid.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity != null) result.Value = PartiiDto.FromEntity(entity);
            return result;
        }
    }
}
