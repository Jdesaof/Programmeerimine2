using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Partiid
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SavePartiiCommand : IRequest<OperationResult<PartiiDto>>, ITransactional
    {
        public int Id { get; set; }
        public int OluId { get; set; }
        public string Kood { get; set; } = string.Empty;
        public DateTime Kuupaev { get; set; }
        public string Kirjeldus { get; set; } = string.Empty;
        public string Tulemus { get; set; } = string.Empty;
    }
}
