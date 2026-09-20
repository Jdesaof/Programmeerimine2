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
    public class SaveOluCommandHandler : IRequestHandler<SaveOluCommand, OperationResult<OluDto>>
    {
        private readonly ApplicationDbContext _context;
        public SaveOluCommandHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<OluDto>> Handle(SaveOluCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<OluDto>();
            var validation = await new SaveOluCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    result.AddPropertyError(error.PropertyName, error.ErrorMessage);
                return result;
            }

            var entity = request.Id == 0 ? new Olu() :
                await _context.Olud.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null) return result;

            entity.Nimi = request.Nimi ?? string.Empty;
            entity.Kirjeldus = request.Kirjeldus ?? string.Empty;
            entity.Tuup = request.Tuup ?? string.Empty;
            entity.Alkoholiprotsent = request.Alkoholiprotsent;
            if (request.Id == 0) _context.Olud.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            result.Value = OluDto.FromEntity(entity);
            return result;
        }
    }
}
