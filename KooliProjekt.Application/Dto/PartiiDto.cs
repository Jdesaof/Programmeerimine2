using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class PartiiDto
    {
        public int Id { get; set; }
        public int OluId { get; set; }
        public string Kood { get; set; } = string.Empty;
        public DateTime Kuupaev { get; set; }
        public string Kirjeldus { get; set; } = string.Empty;
        public string Tulemus { get; set; } = string.Empty;

        public static PartiiDto FromEntity(Partii entity)
        {
            return new PartiiDto
            {
                Id = entity.Id,
                OluId = entity.OluId,
                Kood = entity.Kood,
                Kuupaev = entity.Kuupaev,
                Kirjeldus = entity.Kirjeldus,
                Tulemus = entity.Tulemus,
            };
        }
    }
}
