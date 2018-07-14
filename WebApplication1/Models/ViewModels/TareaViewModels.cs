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
        [Display(Name = "Tarea ID")]
        public int TareaID { get; set; }
        [Display(Name = "Descripción")]
        public DescripcionTarea TareaDescripcion { get; set; }

        public TipoTarea TareaTipo { get; set; }

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
        public DateTime TareaFechaIniReal { get; set; }

        [Display(Name = "Fecha Real Fin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TareaFechaFinReal { get; set; }

        [Display(Name = "Fecha Aprobación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TareaAprobacion { get; set; }

        public int TareaResponsableID { get; set; }

        public int TareaAprobadorID { get; set; }

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
        EnAprobación,
        //*implica que la tarea fue aprobada por el supervisor
        Finalizada,
    }
}