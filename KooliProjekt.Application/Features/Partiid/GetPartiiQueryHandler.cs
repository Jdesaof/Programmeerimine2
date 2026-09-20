using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    public class GetPartiiQueryHandler : IRequestHandler<GetPartiiQuery, OperationResult<PartiiDto>>
    {
        private readonly IPartiiRepository _repository;
        public GetPartiiQueryHandler(IPartiiRepository repository) { _repository = repository; }

        public async Task<OperationResult<PartiiDto>> Handle(GetPartiiQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            var result = new OperationResult<PartiiDto>();
            if (request.Id <= 0) return result;
            var entity = await _repository.GetAsync(request.Id, cancellationToken);
            if (entity != null) result.Value = PartiiDto.FromEntity(entity);
            return result;
        }
    }
}
