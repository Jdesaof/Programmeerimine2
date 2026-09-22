using System;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.PartiiFotod
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SavePartiiFotoCommand : IRequest<OperationResult<PartiiFotoDto>>, ITransactional
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public string FailiTee { get; set; } = string.Empty;
    }
}
