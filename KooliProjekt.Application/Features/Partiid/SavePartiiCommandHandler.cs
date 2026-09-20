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
    public class SavePartiiCommandHandler : IRequestHandler<SavePartiiCommand, OperationResult<PartiiDto>>
    {
        private readonly ApplicationDbContext _context;
        public SavePartiiCommandHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<PartiiDto>> Handle(SavePartiiCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PartiiDto>();
            var validation = await new SavePartiiCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    result.AddPropertyError(error.PropertyName, error.ErrorMessage);
                return result;
            }

            var entity = request.Id == 0 ? new Partii() :
                await _context.Partiid.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _context.Olud.AnyAsync(x => x.Id == request.OluId, cancellationToken))
                return result.AddPropertyError("OluId", "Referenced record does not exist.");

            entity.OluId = request.OluId;
            entity.Kood = request.Kood ?? string.Empty;
            entity.Kuupaev = request.Kuupaev;
            entity.Kirjeldus = request.Kirjeldus ?? string.Empty;
            entity.Tulemus = request.Tulemus ?? string.Empty;
            if (request.Id == 0) _context.Partiid.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            result.Value = PartiiDto.FromEntity(entity);
            return result;
        }
    }
}
