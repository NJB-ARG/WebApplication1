using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// Factory Method Design Pattern.
    /// </summary>

    /// <summary>
    /// The 'Creator' abstract class
    /// </summary>
    public abstract class PartesDocumento
    {
        private List<Pagina> _paginas = new List<Pagina>();

        // Constructor calls abstract Factory method
        public PartesDocumento()
        {
            this.CrearPaginas();
        }

        public List<Pagina> Paginas
        {
            get { return _paginas; }
        }

        //**METODOS**
        
        // Factory Method
        public abstract void CrearPaginas();

        // PATTERN: "Template method"
        public abstract void PrimitiveOperation1();
        public abstract void PrimitiveOperation2();

        // The "Template method"
        public void TemplateMethod()
        {
            PrimitiveOperation1();
            PrimitiveOperation2();
            Console.WriteLine("");
        }

        /// The 'Prototype' abstract class
        public abstract PartesDocumento Clone();

    }

    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class HojaProspectoCabecera : PartesDocumento
    {
        public HojaProspectoCabecera()
        {
        }

        // Factory Method implementation
        public override void CrearPaginas()
        {
            //En desarrollo: LOGICA DE QUE DATOS DE LOS INGRESADOS VAN A QUÉ PAGINA
            //paginaSolicitudCabecera.PaginaID = 1;
            //paginaSolicitudCabecera.PaginaNombre = "Datos Solicitud";
            //Paginas.Add();
            //Paginas.Add(new DatosSolicitud { PaginaID = 1, PaginaNombre = "Datos Solicitud" });

            //No desarroladas
            //Paginas.Add(new CuentaBancariaDepositoPagina { PaginaNumero = 2, PaginaNombre = "2" });
            //Paginas.Add(new PagosRealizadosPagina { PaginaNumero = 3, PaginaNombre = "3" });
            //Paginas.Add(new DatosUnidadPagina { PaginaNumero = 4, PaginaNombre = "4" });
        }


        // The "Template method" implementation
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("ConcreteClassA.PrimitiveOperation1()");
        }
        public override void PrimitiveOperation2()
        {
            Console.WriteLine("ConcreteClassA.PrimitiveOperation2()");
        }


        // Create a shallow copy - 'Prototype'
        public override PartesDocumento Clone()
        {
            Console.WriteLine("Cloning PartesDocumento: HojaProspectoCabecera");

            return this.MemberwiseClone() as PartesDocumento;
        }
    }

    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class HojaProspectoDetalle : PartesDocumento
    {
        public HojaProspectoDetalle()
        {
        }

        // Factory Method implementation
        public override void CrearPaginas()
        {
            //Paginas.Add(new HojaProspectoPagina());
            //Paginas.Add(new PropuestaComercialPagina());
            //Paginas.Add(new LegajoPersonaPagina());
        }

        // The "Template method" implementation
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("ConcreteClassB.PrimitiveOperation1()");
        }
        public override void PrimitiveOperation2()
        {
            Console.WriteLine("ConcreteClassB.PrimitiveOperation2()");
        }

        // Create a shallow copy - 'Prototype'
        public override PartesDocumento Clone()
        {
            Console.WriteLine("Cloning PartesDocumento: HojaProspectoDetalle");

            return this.MemberwiseClone() as PartesDocumento;
        }
    }

    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class SolicitudCabecera : PartesDocumento
    {
        public SolicitudCabecera()
        {
        }

        // Factory Method implementation
        public override void CrearPaginas()
        {                     
            //VIEWMODEL NEW();
                       
            Paginas.Add(new CuentaDeposito { PaginaNumero = 1, PaginaNombre = "Cuenta Depósito", PaginaTipo = "C" , PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });
            Paginas.Add(new OfertasMercado { PaginaNumero = 2, PaginaNombre = "Oferta Mercado", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });
            Paginas.Add(new PropuestaComercial { PaginaNumero = 3, PaginaNombre = "Propuesta Comercial", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });            
            Paginas.Add(new DatosUnidad { PaginaNumero = 4, PaginaNombre = "Datos de Unidad", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });
            Paginas.Add(new DatosContacto { PaginaNumero = 5, PaginaNombre = "Datos de Contacto", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });            
        }


        // The "Template method" implementation
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("ConcreteClassA.PrimitiveOperation1()");
        }
        public override void PrimitiveOperation2()
        {
            Console.WriteLine("ConcreteClassA.PrimitiveOperation2()");
        }


        // Create a shallow copy - 'Prototype'
        public override PartesDocumento Clone()
        {
            Console.WriteLine("Cloning PartesDocumento: SolicitudCabecera");

            return this.MemberwiseClone() as PartesDocumento;
        }
    }

    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class SolicitudDetalle : PartesDocumento
    {
        public SolicitudDetalle()
        {
        }

        // Factory Method implementation
        public override void CrearPaginas()
        {                        
            Paginas.Add(new DocAdjunto { PaginaNumero = 6, PaginaNombre = "Documentos Adjuntos", PaginaTipo = "D", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });
            Paginas.Add(new PagosRealizados { PaginaNumero = 7, PaginaNombre = "Pagos Realizados", PaginaTipo = "D", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });
            Paginas.Add(new Comentario { PaginaNumero = 8, PaginaNombre = "Comentarios", PaginaTipo = "D", PaginaValidada = false, PaginaTipoDocumento = "Solicitud" });
        }

        // The "Template method" implementation
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("ConcreteClassB.PrimitiveOperation1()");
        }
        public override void PrimitiveOperation2()
        {
            Console.WriteLine("ConcreteClassB.PrimitiveOperation2()");
        }

        // Create a shallow copy - 'Prototype'
        public override PartesDocumento Clone()
        {
            Console.WriteLine("Cloning PartesDocumento: SolicitudDetalle");

            return this.MemberwiseClone() as PartesDocumento;
        }
    }

    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class OrdenCabecera : PartesDocumento
    {
        public OrdenCabecera()
        {
        }

        // Factory Method implementation
        public override void CrearPaginas()
        {
            Paginas.Add(new Seguro { PaginaNumero = 1, PaginaNombre = "Seguro Unidad", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Orden" });
            Paginas.Add(new ServicioTecnico { PaginaNumero = 2, PaginaNombre = "Servicio Técnico", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Orden" });
            Paginas.Add(new Gestoria { PaginaNumero = 3, PaginaNombre = "Gestoría", PaginaTipo = "C", PaginaValidada = false, PaginaTipoDocumento = "Orden" });                            
        }

        // The "Template method" implementation
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("ConcreteClassC.PrimitiveOperation1()");
        }
        public override void PrimitiveOperation2()
        {
            Console.WriteLine("ConcreteClassC.PrimitiveOperation2()");
        }

        // Create a shallow copy - 'Prototype'
        public override PartesDocumento Clone()
        {
            Console.WriteLine("Cloning PartesDocumento: OrdenCabecera");

            return this.MemberwiseClone() as PartesDocumento;
        }
    }

    /// <summary>
    /// A 'ConcreteCreator' class
    /// </summary>
    class OrdenDetalle : PartesDocumento
    {
        public OrdenDetalle()
        {
        }

        // Factory Method implementation
        public override void CrearPaginas()
        {
            Paginas.Add(new DocAdjunto { PaginaNumero = 1, PaginaNombre = "Documentos Adjuntos", PaginaTipo = "D", PaginaValidada = false, PaginaTipoDocumento = "Orden" });
            Paginas.Add(new Comentario { PaginaNumero = 2, PaginaNombre = "Comentarios", PaginaTipo = "D", PaginaValidada = false, PaginaTipoDocumento = "Orden" });
        }

        // The "Template method" implementation
        public override void PrimitiveOperation1()
        {
            Console.WriteLine("ConcreteClassD.PrimitiveOperation1()");
        }
        public override void PrimitiveOperation2()
        {
            Console.WriteLine("ConcreteClassD.PrimitiveOperation2()");
        }

        // Create a shallow copy - 'Prototype'
        public override PartesDocumento Clone()
        {
            Console.WriteLine("Cloning PartesDocumento: OrdenDetalle");

            return this.MemberwiseClone() as PartesDocumento;
        }
    }
}