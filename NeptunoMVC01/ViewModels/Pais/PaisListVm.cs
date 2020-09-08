using System.ComponentModel.DataAnnotations;

namespace NeptunoMVC01.ViewModels.Pais
{
    public class PaisListVm
    {
        public int PaisId { get; set; }

        [Display(Name = "País")]
        public string NombrePais { get; set; }
        public int  CantidadDeCiudades { get; set; }
    }
}