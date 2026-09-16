using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Maitsmine
    {
        public int Id { get; set; }

        public int PartiiId { get; set; }
        public Partii Partii { get; set; }

        public DateTime Kuupaev { get; set; }

        [Required]
        [MaxLength(100)]
        public string Degusteerija { get; set; } = string.Empty;

        public int Hinne { get; set; }

        [MaxLength(255)]
        public string Kommentaar { get; set; } = string.Empty;
    }
}