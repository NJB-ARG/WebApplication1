using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class HojaProspectoViewModel
    {
        public int HojaProspectoViewModelID { get; set; }
        
        [Display(Name = "Fecha Creación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HojaProspectoFecCreacion { get; set; }

        [Display(Name = "Fecha Creación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HojaProspectoFecUltMod { get; set; }

        //Datos prospecto
        public Prospecto HojaProspectoProspecto { get; set; }

        //Ofertas de consecionarios
        //public virtual List<OfertasMercadoPagina> HojaProspectoOfertasPosibles { get; set; }

        public string HojaProspectoComentario { get; set; }
    }
}