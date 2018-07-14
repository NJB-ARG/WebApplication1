using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel;
//using System.Linq;
//using System.Web;


namespace WebApplication1.Models.ViewModels
{
    //REVISAR:  Analizar la jerarquía de dependencias entre ventas, solicitud y orden
    public class Solicitud
    {

        public Solicitud()
        {
            Solicitud_Paginas = new List<Pagina>();
            Solicitud_LineasSolicitud = new List<LineaSolicitud>();
            Solicitud_Ordenes = new List<Orden>();
            Solicitud_Pagos = new List<Pago>();
            Solicitud_DocumentosAdjuntos = new List<DocumentoAdjunto>();
            Solicitud_Facturas = new List<FacturaCliente>();
    }

        //[Key, ForeignKey("SolicitudVenta")]
        [Display(Name = "ID")]
        public int SolicitudID { get; set; }

        //NJB-INI - user ID from AspNetUser table - (Security)
        public string SolicitudOwnerID { get; set; }

        [Display(Name = "Numero")]
        public int SolicitudNum { get; set; }

        [Display(Name = "Descripción")]
        public string SolicitudDescripcion { get; set; }

        //[Required]
        [Display(Name = "Tipo Solicitante")]
        public string SolicitudTipoSolicitante { get; set; }
        //*para dropdownlist*
        //public IEnumerable<SelectListItem> TipoSolicitantes { set; get; }

        [Display(Name = "Fecha Creación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime SolicitudFecCreacion { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SolicitudFecVencimiento { get; set; }

        [Display(Name = "Estado")]
        [UIHint("Enum")]
        [DataType(DataType.Text)]
        public SolicitudStatus SolicitudEstado { get; set; }

        [Display(Name = "Monto Total")]
        [DataType(DataType.Currency)]
        public double SolicitudMontoTotal { get; set; }

        //**FOREIGN KEYS-ini
        [Display(Name = "Sucursal")]
        public int SucursalID { get; set; }
        [Display(Name = "Empleado Responsable")]
        public int? EmpleadoID { get; set; }
        //Puede ser un prospecto de la agenda de contactos o un cliente
        [Display(Name = "Prospecto")]
        public int? ProspectoID { get; set; }
        [Display(Name = "Cliente")]
        public int? ClienteID { get; set; }
        //**FOREIGN KEYS-fin

        //NAVIGATION PROP - ini
        //Lista de paginas de DATOS que componen la solicitud (Asociación Polimorfica) - Se muestran por medio de -READING RELATED DATA-
        public virtual List<Pagina> Solicitud_Paginas { get; set; }

        //Navigation Properties - Colección de objetos - una solicitud puede tener más de una orden
        //UNA ORDEN POR PROVEEDOR
        //UN PROVEEDOR ESTA ASOCIADO A UN PRODUCTO
        public virtual List<Orden> Solicitud_Ordenes { get; set; }

        //Navigation Properties - Listas - CADA LINEA PUEDE TENR UN PROVEEDOR DIFERENTE DEPENDIENDO DEL PRODUCTO CARGADO
        public virtual List<LineaSolicitud> Solicitud_LineasSolicitud { get; set; }

        //Una solicitud puede tener varios
        public virtual List<Pago> Solicitud_Pagos { get; set; }
        public virtual List<DocumentoAdjunto> Solicitud_DocumentosAdjuntos { get; set; }
        public virtual List<FacturaCliente> Solicitud_Facturas { get; set; }

        //prospecto puede o no existir
        [Display(Name = "Prospecto")]
        [ForeignKey("ProspectoID")]
        public virtual Prospecto Solicitud_Prospecto { get; set; }
        //cliente puede o no existir
        [Display(Name = "Cliente")]        
        [ForeignKey("ClienteID")]
        public virtual Cliente Solicitud_Cliente { get; set; }
        //empleado puede o no existir
        [Display(Name = "Empleado")]
        [ForeignKey("EmpleadoID")]        
        public virtual Empleado Solicitud_Empleado { get; set; }

        [Display(Name = "Sucursal")]
        [ForeignKey("SucursalID")]
        public Sucursal Solicitud_Sucursal { get; set; }
        //NAVIGATION PROP - fin
    }

    //el estado "Orden Enviada" inicia el proceso de la creación de la orden y se continua la gestión desde la orden
    public enum SolicitudStatus
    {        
        [Display(Name = "En Borrador")]
        En_Borrador,
        Iniciada,        
        [Display(Name = "En Aprobación")]
        En_Aprobacion,
        Approbada,        
        [Display(Name = "Orden Enviada")]
        Orden_Enviada,
        Cancelada,
        Rechazada
    }

    public class custom_Dropdownlist
    {
        //*para dropdownlist*
        public IEnumerable<SelectListItem> dropdownlist { set; get; }
    }

    public class LineaSolicitud
    {
        public int LineaSolicitudID { get; set; }

        //foreign key
        [Display(Name = "Solicitud")]
        public int SolicitudID { get; set; }
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }

        [Display(Name = "Linea #")]
        public int LineaSolicitudNum { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "PU Producto")]
        public Double LineaSolicitudPUprod { get; set; }
        [Display(Name = "Cantidad")]
        public int LineaSolicitudCantidad { get; set; }
        [Display(Name = "Moneda")]
        public String LineaSolicitudMoneda { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Monto Línea")]
        public double LineaSolicitudMonto { get; set; }
        //Para la ORDEN:
        [Display(Name = "Proveedor ID")]
        public int? SolicitudProveedorID { get; set; }
        [Display(Name = "Proveedor Nombre")]
        public string SolicitudProveedorNombre { get; set; }

        //navigation properties
        public virtual Solicitud LineaSolicitud_Solicitud { get; set; }
        public virtual Producto LineaSolicitud_Producto { get; set; }
    }


    public class Producto
    {
        public Producto()
        {
            Producto_LineasSolicitud = new List<LineaSolicitud>();
            Producto_Proveedores = new List<Proveedor>();
        }
        [Display(Name = "Producto")]
        public int ProductoID { get; set; }
        [Display(Name = "Descripción")]
        public string ProductoDesc { get; set; }
        [Display(Name = "Moneda")]
        public string ProductoMoneda { get; set; }
        [Display(Name = "Precio")]
        [DataType(DataType.Currency)]
        public double ProductoPrecio { get; set; }
        //SI o NO
        [Display(Name = "Estado")]
        public bool ProductoEstadoActivo { get; set; }

        //Navigation Properties - Listas
        public virtual List<LineaSolicitud> Producto_LineasSolicitud { get; set; }
        public virtual List<Proveedor> Producto_Proveedores { get; set; }
    }

    public class Proveedor
    {
        public Proveedor()
        {
            Proveedor_Productos = new List<Producto>();
            Proveedor_Facturas = new List<FacturaProveedor>();
        }

        public int ProveedorID { get; set; }

        public string ProveedorNombre { get; set; }

        public string ProveedorDesc { get; set; }

        public string ProveedorDireccion { get; set; }

        public string ProveedorContacto { get; set; }

        [Display(Name = "CUIT")]
        public Int64 ProveedorCUIT { get; set; }

        //SI o NO
        public bool ProveedorEstadoActivo { get; set; }

        //Navigation Properties - Listas
        public virtual List<Producto> Proveedor_Productos { get; set; }
        public virtual List<FacturaProveedor> Proveedor_Facturas { get; set; }
    }

    public class Orden
    {
        public Orden()
        {
            Orden_Paginas = new List<Pagina>();
            Orden_DocumentosAdjuntos = new List<DocumentoAdjunto>();
            Orden_Facturas = new List<FacturaProveedor>();
        }

        //[Key, ForeignKey("OrdenVenta")]
        public int OrdenID { get; set; }

        //Lista de paginas que componen la orden (Asociación Polimorfica)
        public virtual List<Pagina> Orden_Paginas { get; set; }
        public virtual List<DocumentoAdjunto> Orden_DocumentosAdjuntos { get; set; }
        public virtual List<FacturaProveedor> Orden_Facturas { get; set; }

        //Foreign key
        public int SolicitudID { get; set; }
        //Navigation Properties - un solo objeto
        public virtual Solicitud Orden_Solicitud { get; set; }        
    }

    public abstract class Persona
    {
        [Display(Name = "ID")]
        public int PersonaID { get; set; }

        [Display(Name = "Nombre")]
        public string PersonaNombre { get; set; }

        [Display(Name = "Apellido")]
        public string PersonaApellido { get; set; }

        [Display(Name = "Telefono")]
        public Int64 PersonaTelefono { get; set; }

        [Display(Name = "Dirección")]
        public string PersonaDireccion { get; set; }

        [Display(Name = "CUIL")]
        public Int64 PersonaCUIL { get; set; }

        [Display(Name = "DNI")]
        public Int64 PersonaDni { get; set; }

        [Display(Name = "Mail")]
        public string PersonaMail { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        public DateTime PersonaFechaNacimiento { get; set; }

        [Display(Name = "Sexo")]
        public string PersonaSexo { get; set; }

        [Display(Name = "Nacionalidad")]
        public string PersonaNacionalidad { get; set; }

        [Display(Name = "Localidad")]
        public string PersonaLocalidad { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName
        {
            get
            {
                return PersonaApellido + ", " + PersonaNombre;
            }
        }
    }

    //[Table("Empleados")]
    public class Empleado : Persona
    {
        //public int EmpleadoID { get; set; }
        public Empleadosector EmpleadoSector { get; set; }

        //1,2,3,4
        public Empleadotipo EmpleadoTipo { get; set; }

        //N1,N2,N3
        public string EmpleadoNivel { get; set; }

        //Navigation Properties - Listas
        public virtual List<Solicitud> Empleado_Solicitudes { get; set; }

        //Navigation prop - Security
        public virtual ApplicationUser Empleado_AppUser { get; set; }
    }

    public enum Empleadotipo
    {
        Operario,
        Encargado,
        Gerente,
        Socio
    }

    public enum Empleadosector
    {
        Ventas,
        Taller,
        Repuestos,
        Compras,
        Finanzas,
        Contabilidad,
        Gestoria,
        Tesoreria,
        Empresa
    }

    //[Table("Clientes")]
    public class Cliente : Persona
    {
        //public int ClienteID { get; set; }

        //Cliente Individual, Cliente Empresa
        public Clientetipo ClienteTipo { get; set; }

        //1,2,3,4
        public Clienteclase ClienteClase { get; set; }

        //Navigation Properties - Listas
        public virtual List<Solicitud> Cliente_Solicitudes { get; set; }
        public virtual List<FacturaCliente> Cliente_Facturas { get; set; }

        //Navigation prop - Security
        public virtual ApplicationUser Cliente_AppUser { get; set; }
    }

    public enum Clientetipo
    {
        Individual,
        Empresa,
        Individual_Premium,
        Empresa_Premium
    }

    public enum Clienteclase
    {
        Standard,
        Platinium,
        Gold,
        Black
    }

    //[Table("Prospecto")]
    public class Prospecto : Persona
    {
        public string ProspectoUnidadBuscada { get; set; }
        public string ProspectoProfesion { get; set; }
        [DataType(DataType.EmailAddress)]
        public string ProspectoMail2 { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string ProspectoTelefono2 { get; set; }
        public string ProspectoFacebook { get; set; }
        public string ProspectoTwiter { get; set; }
        [DataType(DataType.Time)]
        public string ProspectoHorarioContacto { get; set; }

        //Datos demanda prospecto
        public string ProspectoCanalInicial { get; set; }
        public bool ProspectoUsuarioDeMoto { get; set; }
        public string ProspectoUnidadActual { get; set; }
        public bool ProspectoEntregaUnidad { get; set; }
        public string ProspectoAnioUnidad { get; set; }
        public string ProspectoKmUnidad { get; set; }
        public bool ProspectoConocimientoTecnico { get; set; }
        public string ProspectoTipoUsoUnidad { get; set; }

        //Navigation Properties - Listas
        [ForeignKey("ProspectoID")]
        public virtual List<Solicitud> Prospecto_Solicitudes { get; set; }

        //Ofertas de consecionarios
        public virtual List<OfertasMercado> Prospecto_OfertasMercadoPagina { get; set; }
    }

    //public class SolicitudesPersonas
    //{
    //    //[Key, ForeignKey("OrdenVenta")]
    //    public int SolicitudesPersonasID { get; set; }

    //    //Foreign key
    //    public int SolicitudID { get; set; }
    //    public int PersonaID { get; set; }

    //    //
    //    public int SolicitudesPersonasNumero { get; set; }

    //    //Navigation Properties - un solo objeto
    //    public virtual Solicitud SolicitudesPersonas_Solicitud { get; set; }
    //    public virtual Persona SolicitudesPersonas_Persona { get; set; }

    //}


    //Un pago se realiza sobre una cuenta y debe, obligatoriamente, ser asociado a una solicitud o una orden
    public class Pago
    {
        public Pago()
        {
            PagoFecha = DateTime.Now;            
        }

        public int PagoID { get; set; }

        [Display(Name = "Fecha Pago")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PagoFecha { get; set; }

        public string PagoDescripcion { get; set; }

        [Display(Name = "Monto Pago")]
        [DataType(DataType.Currency)]
        public double PagoMonto { get; set; }

        //Foreign keys
        public int SolicitudID { get; set; }        
        public int CuentaID { get; set; }

        //Navigation Properties - un solo objeto
        public virtual Solicitud Pago_Solicitud { get; set; }
        public virtual Cuenta Pago_Cuenta { get; set; }
    }

    public class Cuenta
    {
        public int CuentaID { get; set; }
        [Display(Name = "Cuenta Número")]
        public long CuentaNumero { get; set; }
        [Display(Name = "Cuenta Titular")]
        public string CuentaTitular { get; set; }
        [Display(Name = "Cuenta Descripción")]
        public string CuentaDesc { get; set; }

        //Navigation Properties - lista
        public virtual List<Pago> Cuenta_Pagos { get; set; }
    }

    public class TipoDepartamento
    {
        public TipoDepartamento()
        {
            TipoDepartamento_Sucursales = new List<Sucursal>();
        }

        public int TipoDepartamentoID { get; set; }

        public string TipoDepartamentoNombre { get; set; }

        public string TipoDepartamentoDesc { get; set; }

        //Navigation Properties - lista
        public virtual List<Sucursal> TipoDepartamento_Sucursales { get; set; }
    }

    public class Sucursal
    {
        public Sucursal()
        {
            Sucursal_TipoDepartamentos = new List<TipoDepartamento>();
        }

        public int SucursalID { get; set; }
        [Display(Name = "Sucursal Nombre")]
        public string SucursalNombre { get; set; }

        //Por ahora dirección completa
        [Display(Name = "Sucursal Dirección")]
        public string SucursalDireccion { get; set; }

        //Foreign key
        public int EmpresaID { get; set; }

        //Navigation Properties - un solo objeto
        public virtual Empresa Sucursal_Empresa { get; set; }
        public virtual List<TipoDepartamento> Sucursal_TipoDepartamentos { get; set; }
    }

    public class Empresa
    {
        public int EmpresaID { get; set; }
        [Display(Name = "Empresa Nombre")]
        public string EmpresaNombre { get; set; }
        [Display(Name = "Empresa Descripción")]
        public string EmpresaDesc { get; set; }

        //Navigation Properties - lista
        public virtual List<Sucursal> Empresa_Sucursales { get; set; }
    }

    //public class Enumerador
    //{
    //    public int EnumeradorID { get; set; }

    //    public SolicitudStatus SolicitudEstado { get; set; }

    //}

    public class DocumentoAdjunto
    {
        public DocumentoAdjunto()
        {
            DocumentoAdjuntoFechaCreacion = DateTime.Now;
            DocumentoAdjuntoFechaAprobacion = DateTime.Now.AddDays(30);
        }

        public int DocumentoAdjuntoID { get; set; }
        [Display(Name = "Fecha Creación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DocumentoAdjuntoFechaCreacion { get; set; }
        [Display(Name = "Fecha Aprobación")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DocumentoAdjuntoFechaAprobacion { get; set; }
        //para validaciones
        [Display(Name = "Cargado")]
        public bool DocumentoAdjuntoCargado { get; set; }
        [Display(Name = "Aprobado")]
        public bool DocumentoAdjuntoAprobado { get; set; }
        //
        [Display(Name = "Comentario")]
        public string DocumentoAdjuntoComentario { get; set; }
        [Display(Name = "Tipo")]
        public Archivostipo DocumentoAdjuntoTipo { get; set; }

        //foreign Key
        public int? SolicitudID { get; set; }
        //Navigation Properties - un solo objeto        
        public virtual Solicitud DocumentoAdjunto_Solicitud { get; set; }
        //foreign Key
        public int? OrdenID { get; set; }
        //Navigation Properties - un solo objeto        
        public virtual Orden DocumentoAdjunto_Orden { get; set; }
    }

    public enum Archivostipo
    {
        [Display(Name = "Comprobante Pago")]
        ComprobantePago,
        Dni,
        CertificadoNoRodamiento,
        BoletoCompraVenta,
        CertificadoFabrica,
        CertificadoImportacion,
        Remito,
        FormularioInscripcion,
        FacturaProveedor,
        FacturaCliente
    }

    public class Factura
    {
        public int FacturaID { get; set; }
        public string FacturaFechaEmision { get; set; }

        public string FacturaNumero { get; set; }
        // A, B, C
        public string FacturaTipo { get; set; }

        public FacturaStatus FacturaEstado { get; set; }
        public string FacturaMontoTotal { get; set; }
    }

    public class FacturaCliente : Factura
    {
        public int FacturaClienteID { get; set; }

        //Para la ORDEN:
        [Display(Name = "Cliente ID")]
        public int ClienteID { get; set; }
        [Display(Name = "Solicitud ID")]
        public int SolicitudID { get; set; }

        //Navigation Properties - Listas    
        [ForeignKey("SolicitudID")]
        public virtual Solicitud Factura_Solicitud { get; set; }
        [ForeignKey("ClienteID")]
        public virtual Cliente Factura_Cliente { get; set; }
    }

    public class FacturaProveedor : Factura
    {
        public int FacturaProveedorID { get; set; }

        //Para la ORDEN:
        [Display(Name = "Proveedor ID")]
        public int ProveedorID { get; set; }
        [Display(Name = "Orden ID")]
        public int OrdenID { get; set; }

        //Navigation Properties - Listas  
        [ForeignKey("OrdenID")]
        public virtual Orden Factura_Orden { get; set; }
        [ForeignKey("ProveedorID")]
        public virtual Proveedor Factura_Proveedor { get; set; }
    }

    //el estado "Orden Enviada" inicia el proceso de la creación de la orden y se continua la gestión desde la orden
    public enum FacturaStatus
    {
        Cargada,
        [Display(Name = "En Proceso")]
        En_Proceso,
        Denegada,                
        Aprobada,                        
        Cancelada
    }
}