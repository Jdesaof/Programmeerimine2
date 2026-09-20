using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class KoostisosaDto
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public string Nimetus { get; set; } = string.Empty;
        public string Uhik { get; set; } = string.Empty;
        public decimal Hind { get; set; }
        public decimal Kogus { get; set; }
        public string Kirjeldus { get; set; } = string.Empty;
        public decimal Summa => Hind * Kogus;

        public static KoostisosaDto FromEntity(Koostisosa entity)
        {
            return new KoostisosaDto
            {
                Id = entity.Id,
                PartiiId = entity.PartiiId,
                Nimetus = entity.Nimetus,
                Uhik = entity.Uhik,
                Hind = entity.Hind,
                Kogus = entity.Kogus,
                Kirjeldus = entity.Kirjeldus,
            };
        }
    }
}
