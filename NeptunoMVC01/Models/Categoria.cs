using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeptunoMVC01.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name="Categoría")]
        [StringLength(100,ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres",MinimumLength = 3)]
        public string NombreCategoria { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(255,ErrorMessage = "El campo {0} debe contener no más de {1} caracteres")]
        public string Descripcion { get; set; }
    }
}