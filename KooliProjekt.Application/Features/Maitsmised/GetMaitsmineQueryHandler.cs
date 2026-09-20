using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class GetMaitsmineQueryHandler : IRequestHandler<GetMaitsmineQuery, OperationResult<MaitsmineDto>>
    {
        private readonly IMaitsmineRepository _repository;
        public GetMaitsmineQueryHandler(IMaitsmineRepository repository) { _repository = repository; }

        public async Task<OperationResult<MaitsmineDto>> Handle(GetMaitsmineQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<MaitsmineDto>();
            if (request.Id <= 0) return result;
            var entity = await _repository.GetAsync(request.Id, cancellationToken);
            if (entity != null) result.Value = MaitsmineDto.FromEntity(entity);
            return result;
        }
    }
}
