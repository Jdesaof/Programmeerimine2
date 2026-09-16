using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Olu
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nimi { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Kirjeldus { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Tuup { get; set; } = string.Empty;

        public decimal Alkoholiprotsent { get; set; }
    }
}