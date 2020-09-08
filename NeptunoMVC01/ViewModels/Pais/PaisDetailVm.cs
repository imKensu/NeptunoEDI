using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NeptunoMVC01.Models;

namespace NeptunoMVC01.ViewModels.Pais
{
    public class PaisDetailVm
    {
        public int PaisId { get; set; }

        [Display(Name = "País")]
        public string NombrePais { get; set; }
        public List<Ciudad> Ciudades { get; set; }
    }
}