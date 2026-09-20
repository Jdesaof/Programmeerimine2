using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class GetPartiiFotoQueryHandler : IRequestHandler<GetPartiiFotoQuery, OperationResult<PartiiFotoDto>>
    {
        private readonly IPartiiFotoRepository _repository;
        public GetPartiiFotoQueryHandler(IPartiiFotoRepository repository) { _repository = repository; }

        public async Task<OperationResult<PartiiFotoDto>> Handle(GetPartiiFotoQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PartiiFotoDto>();
            if (request.Id <= 0) return result;
            var entity = await _repository.GetAsync(request.Id, cancellationToken);
            if (entity != null) result.Value = PartiiFotoDto.FromEntity(entity);
            return result;
        }
    }
}
