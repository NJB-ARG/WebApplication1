using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class Tarea
    {
        public Tarea()
        {
            TareaFechaIni = DateTime.Now;
            TareaFechaFin = DateTime.Now;
            //TareaFechaIniReal = DateTime.Now;
            //TareaFechaFinReal = DateTime.Now;
        }

        [Display(Name = "Tarea ID")]
        public int TareaID { get; set; }
        [Display(Name = "Título")]
        public DescripcionTarea TareaDescripcion { get; set; }
        [Display(Name = "Descripción")]
        public string TareaDescripcion2 { get; set; }
        [Display(Name = "Tipo")]
        public TipoTarea TareaTipo { get; set; }
        [Display(Name = "Módulo")]
        public ModuloTarea TareaModulo { get; set; }

        [Display(Name = "Fecha Estimada Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TareaFechaIni { get; set; }

        [Display(Name = "Fecha Estimada Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TareaFechaFin { get; set; }

        [Display(Name = "Fecha Real Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TareaFechaIniReal { get; set; }

        [Display(Name = "Fecha Real Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? TareaFechaFinReal { get; set; }

        //[Display(Name = "Fecha Aprobación")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public DateTime TareaAprobacion { get; set; }

        [Display(Name = "Finalizada")]
        public bool TareaIsDone { get; set; }

        [Display(Name = "Empleado Responsable")]
        public int EmpleadoID { get; set; }
        [Display(Name = "Empleado")]
        [ForeignKey("EmpleadoID")]
        public virtual Empleado Tarea_Empleado { get; set; }

        //Navigation prop - Security
        [Display(Name = "Tarea Owner")]
        public virtual ApplicationUser Tarea_AppUser { get; set; }

        //foreign Key
        public int? SolicitudID { get; set; }
        //Navigation Properties - un solo objeto
        [Display(Name = "Tarea Solicitud")]
        public virtual Solicitud Tarea_Solicitud { get; set; }
        //foreign Key
        public int? OrdenID { get; set; }
        //Navigation Properties - un solo objeto
        [Display(Name = "Tarea Orden")]
        public virtual Orden Tarea_Orden { get; set; }

        [Display(Name = "Tarea Estado")]
        public EstadoTarea TareaEstado { get; set; }        
    }

    public enum DescripcionTarea
    {
        [Display(Name = "Enviar Propuesta Comercial")]
        EnviarPropuestaComercial,
        [Display(Name = "Completar Legajo")]
        CompletarLegajo,        
        //*Todos los documentos que figuran en Archivostipo del VentaViewModel
        ValidarDocumento,
        ActivacionUnidad,
        CrearAlerta,
        BuscarCertificadoImportacion,
        CrearBoletoCompraVenta,
        CrearCertificadoNoRodamiento,
        CrearRemito,
        ArmarLegajoGestoria,
        RealizarFormulariosInscripcion,
        EnviarUnidad,
        EnviarDocumentacionCliente,
        EnviarDocumentacionCorreo,
        TareaManual,
        //* Lo debe hacer el Gerente de Ventas
        [Display(Name = "Aprobar Solicitud")]
        AprobarSolicitud     
    }

    public enum ModuloTarea
    {
        [Display(Name = "Módulo Ventas")]
        TareaModuloVentas,
        [Display(Name = "Módulo CRM")]
        TareaModuloCRM,        
    }

    public enum TipoTarea
    {
        [Display(Name = "Manual")]
        Manual,
        [Display(Name = "Automática")]
        Automatica
    }

    public enum EstadoTarea
    {
        Asignada,
        EnCurso,
        //EnAprobación,
        //*implica que la tarea fue aprobada por el supervisor
        Finalizada
    }
}