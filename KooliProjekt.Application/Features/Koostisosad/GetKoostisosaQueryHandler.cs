using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class GetKoostisosaQueryHandler : IRequestHandler<GetKoostisosaQuery, OperationResult<KoostisosaDto>>
    {
        private readonly IKoostisosaRepository _repository;
        public GetKoostisosaQueryHandler(IKoostisosaRepository repository) { _repository = repository; }

        public async Task<OperationResult<KoostisosaDto>> Handle(GetKoostisosaQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<KoostisosaDto>();
            if (request.Id <= 0) return result;
            var entity = await _repository.GetAsync(request.Id, cancellationToken);
            if (entity != null) result.Value = KoostisosaDto.FromEntity(entity);
            return result;
        }
    }
}
