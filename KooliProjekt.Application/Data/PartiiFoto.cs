using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class PartiiFoto : Entity
    {

        public int PartiiId { get; set; }

        public Partii Partii { get; set; }

        [Required]
        [MaxLength(500)]
        public string FailiTee { get; set; } = string.Empty;
    }
}