using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class SaveKoostisosaCommandHandler : IRequestHandler<SaveKoostisosaCommand, OperationResult<KoostisosaDto>>
    {
        private readonly IKoostisosaRepository _repository;
        private readonly IPartiiRepository _parentRepository;
        public SaveKoostisosaCommandHandler(IKoostisosaRepository repository, IPartiiRepository parentRepository)
        {
            _repository = repository;
            _parentRepository = parentRepository;
        }

        public async Task<OperationResult<KoostisosaDto>> Handle(SaveKoostisosaCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<KoostisosaDto>();
            var validation = await new SaveKoostisosaCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    result.AddPropertyError(error.PropertyName, error.ErrorMessage);
                return result;
            }

            var entity = request.Id == 0 ? new Koostisosa() :
                await _repository.GetAsync(request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _parentRepository.ExistsAsync(request.PartiiId, cancellationToken))
                return result.AddPropertyError("PartiiId", "Referenced record does not exist.");

            entity.PartiiId = request.PartiiId;
            entity.Nimetus = request.Nimetus ?? string.Empty;
            entity.Uhik = request.Uhik ?? string.Empty;
            entity.Hind = request.Hind;
            entity.Kogus = request.Kogus;
            entity.Kirjeldus = request.Kirjeldus ?? string.Empty;
            await _repository.SaveAsync(entity, cancellationToken);
            result.Value = KoostisosaDto.FromEntity(entity);
            return result;
        }
    }
}
