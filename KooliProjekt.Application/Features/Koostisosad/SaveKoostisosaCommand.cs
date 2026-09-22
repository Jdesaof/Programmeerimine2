using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Koostisosad
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SaveKoostisosaCommand : IRequest<OperationResult<KoostisosaDto>>, ITransactional
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public string Nimetus { get; set; } = string.Empty;
        public string Uhik { get; set; } = string.Empty;
        public decimal Hind { get; set; }
        public decimal Kogus { get; set; }
        public string Kirjeldus { get; set; } = string.Empty;
    }
}
