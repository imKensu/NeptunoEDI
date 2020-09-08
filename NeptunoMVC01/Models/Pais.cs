using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeptunoMVC01.Models
{
    [Table("Paises")]
    public class Pais
    {
        public int PaisId { get; set; }
        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(100,ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres",MinimumLength = 3)]
        public string NombrePais { get; set; }
    }
}