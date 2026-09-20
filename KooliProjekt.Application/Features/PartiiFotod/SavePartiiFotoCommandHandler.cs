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
    public class SavePartiiFotoCommandHandler : IRequestHandler<SavePartiiFotoCommand, OperationResult<PartiiFotoDto>>
    {
        private readonly ApplicationDbContext _context;
        public SavePartiiFotoCommandHandler(ApplicationDbContext context) { _context = context; }

        public async Task<OperationResult<PartiiFotoDto>> Handle(SavePartiiFotoCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PartiiFotoDto>();
            var validation = await new SavePartiiFotoCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    result.AddPropertyError(error.PropertyName, error.ErrorMessage);
                return result;
            }

            var entity = request.Id == 0 ? new PartiiFoto() :
                await _context.PartiiFotod.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _context.Partiid.AnyAsync(x => x.Id == request.PartiiId, cancellationToken))
                return result.AddPropertyError("PartiiId", "Referenced record does not exist.");

            entity.PartiiId = request.PartiiId;
            entity.FailiTee = request.FailiTee ?? string.Empty;
            if (request.Id == 0) _context.PartiiFotod.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            result.Value = PartiiFotoDto.FromEntity(entity);
            return result;
        }
    }
}
