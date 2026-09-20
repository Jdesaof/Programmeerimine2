using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    public class GetPruulimisLogiQueryHandler : IRequestHandler<GetPruulimisLogiQuery, OperationResult<PruulimisLogiDto>>
    {
        private readonly IPruulimisLogiRepository _repository;
        public GetPruulimisLogiQueryHandler(IPruulimisLogiRepository repository) { _repository = repository; }

        public async Task<OperationResult<PruulimisLogiDto>> Handle(GetPruulimisLogiQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PruulimisLogiDto>();
            if (request.Id <= 0) return result;
            var entity = await _repository.GetAsync(request.Id, cancellationToken);
            if (entity != null) result.Value = PruulimisLogiDto.FromEntity(entity);
            return result;
        }
    }
}
