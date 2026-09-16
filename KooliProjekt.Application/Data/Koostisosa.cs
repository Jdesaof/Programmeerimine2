using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Koostisosa
    {
        public int Id { get; set; }

        public int PartiiId { get; set; }

        public Partii Partii { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nimetus { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Uhik { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,4)")]
        public decimal Hind { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Kogus { get; set; }

        [NotMapped]
        public decimal Summa => Hind * Kogus;

        [MaxLength(255)]
        public string Kirjeldus { get; set; } = string.Empty;
    }
}