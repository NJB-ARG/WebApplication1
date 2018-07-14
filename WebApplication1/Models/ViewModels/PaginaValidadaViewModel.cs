using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class PaginaValidadaViewModel
    {
        public int PaginaID { get; set; }
        public int PaginaNumero { get; set; }        
        public string Nombre { get; set; }
        public bool Validada { get; set; }
    }
}