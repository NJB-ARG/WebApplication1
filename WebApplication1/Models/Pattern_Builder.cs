using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    /// <summary>
    /// Builder Design Pattern.
    /// </summary>

    /// <summary>
    /// The 'Director' class
    /// </summary>
    class DirectorDocumento
    {
        // Builder uses a complex series of steps
        public void Construir(DocumentoBuilder documentoBuilder)
        {
            documentoBuilder.ConstruirCabecera();
            documentoBuilder.ConstruirDetalle();
            //documentoBuilder.ActualizarEstado();
        }
    }

    /// <summary>
    /// The 'Builder' abstract class
    /// </summary>
    // NJB: le puse public
    public abstract class DocumentoBuilder
    {
        protected Documento documento;
        //protected int Iddocumento;

        //// Gets ID instance
        //public int IdDocumento
        //{
        //    get { return Iddocumento; }
        //}

        // Gets documento instance
        public Documento Documento
        {
            get { return documento; }

        }

        // Abstract build methods
        public abstract void ConstruirCabecera();
        public abstract void ConstruirDetalle();
    }

    /// <summary>
    /// The 'ConcreteBuilder0' class
    /// </summary>
    /// // NJB: le puse public
    public class HojaProspectoBuilder : DocumentoBuilder
    {

        public HojaProspectoBuilder()
        {
            //Iddocumento = 1;
            documento = new Documento(1, "HojaProspecto");
        }

        //Guarda paginas de cabecera en el DOCUMENTO
        public override void ConstruirCabecera()
        {
            //Llamada a Template Method que usa Factory Method
            HojaProspectoCabecera hojaProspectoCabecera = new HojaProspectoCabecera();
            //Leer paginas que devolvio el factory method 
            foreach (Pagina paginahojaProspectoCabecera in hojaProspectoCabecera.Paginas)
            {
                //Ponerles el tipo "Cabecera"
                //paginahojaProspectoCabecera.PaginaTipo = 'C';
                //cargarlas en el dictionary del DOCUMENTO a devolver
                documento[paginahojaProspectoCabecera.GetType().Name] = paginahojaProspectoCabecera;
            }
        }


        //Guarda paginas de detalle en el DOCUMENTO
        public override void ConstruirDetalle()
        {
            //Llamada a Template Method que usa Factory Method
            HojaProspectoDetalle hojaProspectoDetalle = new HojaProspectoDetalle();
            //Leer paginas que devolvio el factory method 
            foreach (Pagina paginahojaProspectoDetalle in hojaProspectoDetalle.Paginas)
            {
                //Ponerles el tipo "Detalle"
                //paginahojaProspectoDetalle.PaginaTipo = 'D';
                //cargarlas en el dictionary del DOCUMENTO a devolver
                documento[paginahojaProspectoDetalle.GetType().Name] = paginahojaProspectoDetalle;
            }

        }

    }

    /// <summary>
    /// The 'ConcreteBuilder1' class
    /// </summary>
    /// // NJB: le puse public
    public class SolicitudBuilder : DocumentoBuilder
    {
        public SolicitudBuilder()
        {
            documento = new Documento(2, "SolicitudVenta");
        }

        //Guarda paginas de cabecera en el DOCUMENTO
        public override void ConstruirCabecera()
        {
            //Llamada a Template Method que usa Factory Method
            SolicitudCabecera solicitudCabecera = new SolicitudCabecera();
            //Leer paginas que devolvio el factory method 
            foreach (Pagina paginaSolicitudCabecera in solicitudCabecera.Paginas)
            {
                //Ponerles el tipo "Cabecera"
                //paginaSolicitudCabecera.PaginaTipo = 'C';
                //paginaSolicitudCabecera.PaginaTipoDocumento = "Solicitud";
                string key_string = paginaSolicitudCabecera.GetType().Name;
                //cargarlas en el dictionary del DOCUMENTO a devolver
                documento[paginaSolicitudCabecera.GetType().Name] = paginaSolicitudCabecera;
            }
        }


        //Guarda paginas de detalle en el DOCUMENTO
        public override void ConstruirDetalle()
        {
            //Llamada a Template Method que usa Factory Method
            SolicitudDetalle solicitudDetalle = new SolicitudDetalle();
            //Leer paginas que devolvio el factory method 
            foreach (Pagina paginaSolicitudDetalle in solicitudDetalle.Paginas)
            {
                //Ponerles el tipo "Detalle"
                //paginaSolicitudDetalle.PaginaTipo = 'D';
                //cargarlas en el dictionary del DOCUMENTO a devolver
                documento[paginaSolicitudDetalle.GetType().Name] = paginaSolicitudDetalle;
            }

        }

    }

    /// <summary>
    /// The 'ConcreteBuilder2' class
    /// </summary>
    public class OrdenBuilder : DocumentoBuilder
{
        //private DatosOrdenPagina _datosOrden = new DatosOrdenPagina(); 

        //public OrdenBuilder(DatosOrdenPagina datosOrdenCrear)
        //{
        //    Iddocumento = datosOrdenCrear.PaginaID;
        //    documento = new Documento(datosOrdenCrear.PaginaID, "DatosOrdenVenta");
        //    _datosOrden = datosOrdenCrear;
        //}

        public OrdenBuilder()
        {
            documento = new Documento(3, "OrdenVenta");          
        }

        public override void ConstruirCabecera()
    {
        //Llamada a Template Method que usa Factory Method
        OrdenCabecera ordenCabecera = new OrdenCabecera();
        //Leer paginas que devolvio el factory method 
        foreach (Pagina paginaOrdenCabecera in ordenCabecera.Paginas)
        {
            //Ponerles el tipo cabecera
            //paginaOrdenCabecera.PaginaTipo = 'C';
            //cargarlas en el dictionary del DOCUMENTO a devolver
            documento[paginaOrdenCabecera.GetType().Name] = paginaOrdenCabecera;
        }

    }

        public override void ConstruirDetalle()
        {
            //Llamada a Template Method que usa Factory Method
            OrdenDetalle ordenDetalle = new OrdenDetalle();


            //agrego el objeto "PartesDocumento" al diccionario de DOCUMENTO
            documento["Test"] = ordenDetalle;

            //Leer paginas que devolvio el factory method 
            foreach (Pagina paginaOrdenDetalle in ordenDetalle.Paginas)
            {
                //Ponerles el tipo cabecera
                //paginaOrdenDetalle.PaginaTipo = 'C';
                //cargarlas en el dictionary del DOCUMENTO a devolver
                documento[paginaOrdenDetalle.GetType().Name] = paginaOrdenDetalle;
            }

        }

}

    /// <summary>
    /// The 'Product' class
    /// </summary>
   public class Documento: Pagina
    {
        public int DocumentoID { get; set; }
        private int _documentoNumero;
        private string _documentoTipo;
        //se carga pagina a pagina indicando si es de cabecera o detalle donde el KEY es código de página
        private Dictionary<string, Pagina> _pagina =
          new Dictionary<string, Pagina>();

        // Constructor
        public Documento(int documentoNumero, string documentoTipo)
        {
            this._documentoNumero = documentoNumero;
            this._documentoTipo = documentoTipo;
        }

        public Documento()
        {
        }

        // Indexer
        public Pagina this[string key]
        {
            get { return _pagina[key]; }
            set { _pagina[key] = value; }
            //set { _colors.Add(key, value);}
        }

        public Dictionary<string, Pagina> paginas
        {
            get { return _pagina; }
        }

        //public void Show()
        //{
        //    Console.WriteLine("\n---------------------------");
        //    Console.WriteLine("Vehicle Type: {0}", _documentoTipo);
        //    Console.WriteLine(" Cabecera : {0}", _partes["cabecera"]);
        //    Console.WriteLine(" Detalle : {0}", _partes["detalle"]);            
        //}
    }
}