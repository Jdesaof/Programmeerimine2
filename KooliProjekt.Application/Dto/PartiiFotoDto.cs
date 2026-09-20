using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class PartiiFotoDto
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public string FailiTee { get; set; } = string.Empty;

        public static PartiiFotoDto FromEntity(PartiiFoto entity)
        {
            return new PartiiFotoDto
            {
                Id = entity.Id,
                PartiiId = entity.PartiiId,
                FailiTee = entity.FailiTee,
            };
        }
    }
}
