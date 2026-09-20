using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class SavePartiiFotoCommandHandler : IRequestHandler<SavePartiiFotoCommand, OperationResult<PartiiFotoDto>>
    {
        private readonly IPartiiFotoRepository _repository;
        private readonly IPartiiRepository _parentRepository;
        public SavePartiiFotoCommandHandler(IPartiiFotoRepository repository, IPartiiRepository parentRepository)
        {
            _repository = repository;
            _parentRepository = parentRepository;
        }

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
                await _repository.GetAsync(request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _parentRepository.ExistsAsync(request.PartiiId, cancellationToken))
                return result.AddPropertyError("PartiiId", "Referenced record does not exist.");

            entity.PartiiId = request.PartiiId;
            entity.FailiTee = request.FailiTee ?? string.Empty;
            await _repository.SaveAsync(entity, cancellationToken);
            result.Value = PartiiFotoDto.FromEntity(entity);
            return result;
        }
    }
}
