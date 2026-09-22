using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class SaveMaitsmineCommandHandler : IRequestHandler<SaveMaitsmineCommand, OperationResult<MaitsmineDto>>
    {
        private readonly IMaitsmineRepository _repository;
        private readonly IPartiiRepository _parentRepository;
        public SaveMaitsmineCommandHandler(IMaitsmineRepository repository, IPartiiRepository parentRepository)
        {
            _repository = repository;
            _parentRepository = parentRepository;
        }

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
                await _repository.GetAsync(request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _parentRepository.ExistsAsync(request.PartiiId, cancellationToken))
                return result.AddPropertyError("PartiiId", "Referenced record does not exist.");

            entity.PartiiId = request.PartiiId;
            entity.Kuupaev = request.Kuupaev;
            entity.Degusteerija = request.Degusteerija;
            entity.Hinne = request.Hinne;
            entity.Kommentaar = request.Kommentaar ?? string.Empty;
            await _repository.SaveAsync(entity, cancellationToken);
            result.Value = MaitsmineDto.FromEntity(entity);
            return result;
        }
    }
}
