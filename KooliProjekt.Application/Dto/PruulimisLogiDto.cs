using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class PruulimisLogiDto
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public DateTime Kuupaev { get; set; }
        public string Kasutaja { get; set; } = string.Empty;
        public string Kirjeldus { get; set; } = string.Empty;

        public static PruulimisLogiDto FromEntity(PruulimisLogi entity)
        {
            return new PruulimisLogiDto
            {
                Id = entity.Id,
                PartiiId = entity.PartiiId,
                Kuupaev = entity.Kuupaev,
                Kasutaja = entity.Kasutaja,
                Kirjeldus = entity.Kirjeldus,
            };
        }
    }
}
