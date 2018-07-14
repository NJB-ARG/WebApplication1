
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Models
{
    /// <summary>
    /// The 'Product' abstract class
    // NJB: le puse public
    /// </summary>

    public abstract class Pagina
    {

        //para identificar una págin: son los datos 
        public int PaginaID { get; set; }
        public int PaginaNumero { get; set; }
        public string PaginaNombre { get; set; }
        //si es una pagina de cabecera de documento o pagina detalle de documento: 'C' o 'D'
        public string PaginaTipo { get; set; }
        public bool PaginaValidada { get; set; }
        public string PaginaTipoDocumento { get; set; }

        //foreign Key
        public int? SolicitudID { get; set; }
        //Navigation Properties - un solo objeto
        [Display(Name = "Pagina Solicitud")]
        public virtual Solicitud Pagina_Solicitud { get; set; }
        //foreign Key
        public int? OrdenID { get; set; }
        //Navigation Properties - un solo objeto
        [Display(Name = "Pagina Orden")]
        public virtual Orden Pagina_Orden { get; set; }

    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class OfertasMercado : Pagina
    {
        public OfertasMercado()
        {
            OfertasMercadoFecha = DateTime.Now;
            OfertasMercadoFinValidez = DateTime.Now;
        }

        //Consulta otros concesionarios
        public bool OfertasMercadoConsecionario { get; set; }
        public string OfertasMercadoLugar { get; set; }
        public DateTime OfertasMercadoFecha { get; set; }
        public string OfertasMercadoUnidad { get; set; }
        [DataType(DataType.Currency)]
        public double OfertasMercadoPrecio { get; set; }
        public string OfertasMercadoFinanciacion { get; set; }
        public DateTime OfertasMercadoFinValidez { get; set; }
    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class PropuestaComercial : Pagina
    {
        public PropuestaComercial()
        {
            PropuestaComercialFechaIni = DateTime.Now;
            PropuestaComercialFechaFin = DateTime.Now;
        }

        [Display(Name = "Fecha Inicio Vigencia")]
        public DateTime PropuestaComercialFechaIni { get; set; }
        [Display(Name = "Fecha Fin Vigencia")]
        public DateTime PropuestaComercialFechaFin { get; set; }
        [Display(Name = "Unidad")]
        public string PropuestaComercialUnidad { get; set; }
        [Display(Name = "Precio")]
        public double PropuestaComercialPrecio { get; set; }
        [Display(Name = "Tipo Factura")]
        public char PropuestaComercialTipoFactura { get; set; }
        [Display(Name = "Forma Pago")]
        public formasPago PropuestaComercialFormaPago { get; set; }
        //en dias
        [Display(Name = "Plazo Entrega")]
        public int PropuestaComercialPlazoEntrega { get; set; }
    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class Seguro : Pagina
    {
        public bool SeguroATM { get; set; }
        public formasPago SeguroFormaPago { get; set; }
        public string SeguroCompania { get; set; }
        public string SeguroTipo { get; set; }
    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class CuentaDeposito : Pagina
    {
        public long CuentaDepositoNumero { get; set; }
        public string CuentaDepositoTitular { get; set; }
    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class PagosRealizados : Pagina
    {

    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class DatosUnidad : Pagina
    {
        public string DatosUnidadDesc { get; set; }
        public string DatosUnidadChasis { get; set; }
        public string DatosUnidadMotor { get; set; }        
    }

    /// <summary>
    /// A 'ConcreteProduct' class
    /// </summary>
    public class DatosContacto : Pagina
    {
        [Display(Name = "Contacto Nombre")]
        public string DatosContactoNombre { get; set; }
        [Display(Name = "Contacto Mail")]
        public string DatosContactoMail { get; set; }
        [Display(Name = "Contacto Teléfono")]
        public long DatosContactoTel { get; set; }
    }

    public class DocAdjunto : Pagina
    {        
    }    

    public enum formasPago
    {
        TarjetaCredito,
        TarjetaDebito,
        Efectivo,
        Deposito,
        Cheque
    }

    public class ServicioTecnico : Pagina
    {
        public ServicioTecnico()
        {
            ServicioTecnicoFecha = DateTime.Now;
        }

        public bool ServicioTecnicoActivacion { get; set; }
        public string ServicioTecnicoResponsable { get; set; }
        public DateTime ServicioTecnicoFecha { get; set; }
        public string ServicioTecnicoObserv { get; set; }
    }

    public class Gestoria : Pagina
    {
        public bool GestoriaIncripcion { get; set; }
        public string GestoriaPatente { get; set; }
        public string GestoriaFormulario { get; set; }
        public bool GestoriaUnidadCancelada { get; set; }
    }

    public class Comentario : Pagina
    {
        public Comentario()
        {
            ComentarioFecha = DateTime.Now;
        }

        [Display(Name = "Número")]
        public int ComentarioNum { get; set; }
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ComentarioFecha { get; set; }
        [Display(Name = "Usuario")]
        public string ComentarioPersona { get; set; }
        [Display(Name = "Comentario")]
        [DataType(DataType.MultilineText)]
        public string ComentarioTexto { get; set; }
    }
}