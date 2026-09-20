using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    public class GetPartiiFotoQuery : IRequest<OperationResult<PartiiFotoDto>>
    {
        public int Id { get; set; }
    }
}
