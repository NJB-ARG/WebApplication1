using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Models
{

    public class Contacto
    {

        [Display(Name = "Contacto ID")]
        public int ContactoID { get; set; }
        [Display(Name = "Nombre Evento")]
        public string ContactoNombreEvento { get; set; }
        [Display(Name = "Tipo Contacto")]
        public TipoContacto ContactoTipoContacto { get; set; }

        [Display(Name = "Fecha Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ContactoFechaIni { get; set; }

        [Display(Name = "Fecha Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ContactoFechaFin { get; set; }

        [Display(Name = "Hora Inicio")]
        [DataType(DataType.Time)]        
        public string ContactoHoraIni { get; set; }

        [Display(Name = "Hora Fin")]
        [DataType(DataType.Time)]
        public string ContactoHoraFin { get; set; }

        [Display(Name = "Empleado Responsable")]
        public int? EmpleadoID { get; set; }
        [Display(Name = "Empleado")]
        //[ForeignKey("EmpleadoID")]
        public virtual Empleado Contacto_Empleado { get; set; }

        [Display(Name = "Prospecto ID")]
        public int? ProspectoID { get; set; }
        [Display(Name = "Prospecto")]
        public virtual Prospecto Contacto_Prospecto { get; set; }

        //Navigation prop - Security
        [Display(Name = "Contacto Owner")]
        public virtual ApplicationUser Contacto_AppUser { get; set; }

        [Display(Name = "Contacto Estado")]
        public EstadoContacto ContactoEstado { get; set; }

    }

    public enum TipoContacto
    {
        Redes,
        Llamada,
        Personal,
        Mail
    }

    public enum EstadoContacto
    {
        Pendiente,
        Cancelado,
        Finalizado
    }
}