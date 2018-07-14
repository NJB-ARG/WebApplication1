using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Models.ViewModels
{
    public class ClienteIndexViewModel
    {
        public IEnumerable<Cliente> Clientes { get; set; }
        public IEnumerable<Solicitud> Solicitudes { get; set; }
        public IEnumerable<Pagina> Paginas { get; set; }
        public IEnumerable<LineaSolicitud> LineasSolicitud { get; set; }
    }

    //public class RolOperacionesIndexViewModel
    //{
    //    public IQueryable<RolOperaciones> RolOperaciones { get; set; }
    //    public IQueryable<Modulo> Modulos { get; set; }
    //    public List<Controladora> Controladoras { get; set; }
    //    public List<Operacion> Operaciones { get; set; }
    //}

    public class PerfilIndexViewModel
    {
        public IQueryable<Perfil> Perfiles { get; set; }
        public IQueryable<Modulo> Modulos { get; set; }
        public IEnumerable<Controladora> Controladoras { get; set; }
        public IEnumerable<Accion> Acciones { get; set; }
    }

    public class SolicitudPaginasViewModel
    {
        public SolicitudPaginasViewModel ()
        {
            DocAdjuntos = new List<DocAdjunto>();
            PagosRealizados = new List<PagosRealizados>();
            Comentarios = new List<Comentario>();
        }

        public CuentaDeposito CuentaDeposito { get; set; }
        public OfertasMercado OfertasMercado { get; set; }
        public PropuestaComercial PropuestaComercial { get; set; }
        public DatosUnidad DatosUnidad { get; set; }
        public DatosContacto DatosContacto { get; set; }

        public List<DocAdjunto> DocAdjuntos { get; set; }
        public List<PagosRealizados> PagosRealizados { get; set; }
        public List<Comentario> Comentarios { get; set; }
    }

    public class OrdenPaginasViewModel
    {
        public OrdenPaginasViewModel()
        {
            DocAdjuntos = new List<DocAdjunto>();            
            Comentarios = new List<Comentario>();
        }

        public Seguro CuentaDeposito { get; set; }
        public ServicioTecnico OfertasMercado { get; set; }
        public Gestoria PropuestaComercial { get; set; }

        public List<DocAdjunto> DocAdjuntos { get; set; }
        public List<Comentario> Comentarios { get; set; }
    }
}