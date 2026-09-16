using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Partii
    {
        public int Id { get; set; }

        public int OluId { get; set; }
        public Olu Olu { get; set; }

        [Required]
        [MaxLength(50)]
        public string Kood { get; set; } = string.Empty;

        public DateTime Kuupaev { get; set; }

        [MaxLength(255)]
        public string Kirjeldus { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Tulemus { get; set; } = string.Empty;
    }
}