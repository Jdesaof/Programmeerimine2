using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    public class SaveOluCommandHandler : IRequestHandler<SaveOluCommand, OperationResult<OluDto>>
    {
        private readonly IOluRepository _repository;
        public SaveOluCommandHandler(IOluRepository repository)
        {
            _repository = repository;
        }

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
                await _repository.GetAsync(request.Id, cancellationToken);
            if (entity == null) return result;

            entity.Nimi = request.Nimi ?? string.Empty;
            entity.Kirjeldus = request.Kirjeldus ?? string.Empty;
            entity.Tuup = request.Tuup ?? string.Empty;
            entity.Alkoholiprotsent = request.Alkoholiprotsent;
            await _repository.SaveAsync(entity, cancellationToken);
            result.Value = OluDto.FromEntity(entity);
            return result;
        }
    }
}
