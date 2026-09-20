using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Maitsmised
{
    public class SaveMaitsmineCommand : IRequest<OperationResult<MaitsmineDto>>, ITransactional
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public DateTime Kuupaev { get; set; }
        public string Degusteerija { get; set; } = string.Empty;
        public int Hinne { get; set; }
        public string Kommentaar { get; set; } = string.Empty;
    }
}
