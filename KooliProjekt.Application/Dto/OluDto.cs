using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class OluDto
    {
        public int Id { get; set; }
        public string Nimi { get; set; } = string.Empty;
        public string Kirjeldus { get; set; } = string.Empty;
        public string Tuup { get; set; } = string.Empty;
        public decimal Alkoholiprotsent { get; set; }

        public static OluDto FromEntity(Olu entity)
        {
            return new OluDto
            {
                Id = entity.Id,
                Nimi = entity.Nimi,
                Kirjeldus = entity.Kirjeldus,
                Tuup = entity.Tuup,
                Alkoholiprotsent = entity.Alkoholiprotsent,
            };
        }
    }
}
