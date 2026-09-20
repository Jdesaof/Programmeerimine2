using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class SavePruulimisLogiCommandHandler : IRequestHandler<SavePruulimisLogiCommand, OperationResult<PruulimisLogiDto>>
    {
        private readonly IPruulimisLogiRepository _repository;
        private readonly IPartiiRepository _parentRepository;
        public SavePruulimisLogiCommandHandler(IPruulimisLogiRepository repository, IPartiiRepository parentRepository)
        {
            _repository = repository;
            _parentRepository = parentRepository;
        }

        public async Task<OperationResult<PruulimisLogiDto>> Handle(SavePruulimisLogiCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PruulimisLogiDto>();
            var validation = await new SavePruulimisLogiCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    result.AddPropertyError(error.PropertyName, error.ErrorMessage);
                return result;
            }

            var entity = request.Id == 0 ? new PruulimisLogi() :
                await _repository.GetAsync(request.Id, cancellationToken);
            if (entity == null) return result;

            if (!await _parentRepository.ExistsAsync(request.PartiiId, cancellationToken))
                return result.AddPropertyError("PartiiId", "Referenced record does not exist.");

            entity.PartiiId = request.PartiiId;
            entity.Kuupaev = request.Kuupaev;
            entity.Kasutaja = request.Kasutaja ?? string.Empty;
            entity.Kirjeldus = request.Kirjeldus ?? string.Empty;
            await _repository.SaveAsync(entity, cancellationToken);
            result.Value = PruulimisLogiDto.FromEntity(entity);
            return result;
        }
    }
}
