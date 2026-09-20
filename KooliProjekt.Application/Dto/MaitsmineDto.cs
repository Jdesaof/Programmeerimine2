using System;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Dto
{
    public class MaitsmineDto
    {
        public int Id { get; set; }
        public int PartiiId { get; set; }
        public DateTime Kuupaev { get; set; }
        public string Degusteerija { get; set; } = string.Empty;
        public int Hinne { get; set; }
        public string Kommentaar { get; set; } = string.Empty;

        public static MaitsmineDto FromEntity(Maitsmine entity)
        {
            return new MaitsmineDto
            {
                Id = entity.Id,
                PartiiId = entity.PartiiId,
                Kuupaev = entity.Kuupaev,
                Degusteerija = entity.Degusteerija,
                Hinne = entity.Hinne,
                Kommentaar = entity.Kommentaar,
            };
        }
    }
}
