using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Olud
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SaveOluCommand : IRequest<OperationResult<OluDto>>, ITransactional
    {
        public int Id { get; set; }
        public string Nimi { get; set; } = string.Empty;
        public string Kirjeldus { get; set; } = string.Empty;
        public string Tuup { get; set; } = string.Empty;
        public decimal Alkoholiprotsent { get; set; }
    }
}
