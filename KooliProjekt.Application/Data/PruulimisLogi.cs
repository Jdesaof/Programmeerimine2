using System;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class PruulimisLogi
    {
        public int Id { get; set; }

        public int PartiiId { get; set; }

        public Partii Partii { get; set; }

        public DateTime Kuupaev { get; set; }

        [Required]
        [MaxLength(100)]
        public string Kasutaja { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Kirjeldus { get; set; } = string.Empty;
    }
}