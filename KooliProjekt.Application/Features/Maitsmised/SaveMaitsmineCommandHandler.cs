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
    public class SaveMaitsmineCommandHandler : IRequestHandler<SaveMaitsmineCommand, OperationResult<MaitsmineDto>>
    {
        private readonly ApplicationDbContext _context;
        public SaveMaitsmineCommandHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<MaitsmineDto>> Handle(SaveMaitsmineCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<MaitsmineDto>();
            var validation = await new SaveMaitsmineCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    result.AddPropertyError(error.PropertyName, error.ErrorMessage);
                return result;
            }

            var entity = request.Id == 0 ? new Maitsmine() :
                await _context.Maitsmised.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _context.Partiid.AnyAsync(x => x.Id == request.PartiiId, cancellationToken))
                return result.AddPropertyError("PartiiId", "Referenced record does not exist.");

            entity.PartiiId = request.PartiiId;
            entity.Kuupaev = request.Kuupaev;
            entity.Degusteerija = request.Degusteerija ?? string.Empty;
            entity.Hinne = request.Hinne;
            entity.Kommentaar = request.Kommentaar ?? string.Empty;
            if (request.Id == 0) _context.Maitsmised.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            result.Value = MaitsmineDto.FromEntity(entity);
            return result;
        }
    }
}
