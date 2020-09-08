using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeptunoMVC01.Models
{
    [Table("Ciudades")]
    public class Ciudad
    {
        public int CiudadId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(120,ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres",MinimumLength = 3)]
        [Display(Name = "Ciudad")]
        public string NombreCiudad { get; set; }
        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1,int.MaxValue,ErrorMessage = "Debe seleccionar un país")]
        public int PaisId { get; set; }
        public virtual Pais Pais { get; set; }
    }
}