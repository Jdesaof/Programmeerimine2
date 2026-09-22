using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PruulimisLogid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SavePruulimisLogiCommand : IRequest<OperationResult<PruulimisLogiDto>>, ITransactional
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public DateTime Kuupaev { get; set; }
        public string Kasutaja { get; set; } = string.Empty;
        public string Kirjeldus { get; set; } = string.Empty;
    }
}
