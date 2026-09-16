using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class PartiiFoto
    {
        public int Id { get; set; }

        public int PartiiId { get; set; }

        public Partii Partii { get; set; }

        [Required]
        [MaxLength(500)]
        public string FailiTee { get; set; } = string.Empty;
    }
}