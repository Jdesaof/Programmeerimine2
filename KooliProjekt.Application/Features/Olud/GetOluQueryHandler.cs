using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    public class GetOluQueryHandler : IRequestHandler<GetOluQuery, OperationResult<OluDto>>
    {
        private readonly IOluRepository _repository;
        public GetOluQueryHandler(IOluRepository repository) { _repository = repository; }

        public async Task<OperationResult<OluDto>> Handle(GetOluQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<OluDto>();
            if (request.Id <= 0) return result;
            var entity = await _repository.GetAsync(request.Id, cancellationToken);
            if (entity != null) result.Value = OluDto.FromEntity(entity);
            return result;
        }
    }
}
