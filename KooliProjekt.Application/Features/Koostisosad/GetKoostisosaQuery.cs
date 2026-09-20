using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    public class GetKoostisosaQuery : IRequest<OperationResult<KoostisosaDto>>
    {
        public int Id { get; set; }
    }
}
