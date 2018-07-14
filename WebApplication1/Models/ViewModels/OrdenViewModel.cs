using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.ViewModels
{
    public class OrdenViewModel
    {
        public OrdenViewModel()
        {
            LineaOrden = new LineaSolicitud();
        }
        [Display(Name = "Linea")]
        public LineaSolicitud LineaOrden { get; set; }        
    }
}