using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Models
{
    public class WebApplication1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WebApplication1Context() : base("name=WebApplication1Context")
        {
        }

        //Documento
        public System.Data.Entity.DbSet<WebApplication1.Models.Documento> Documentos { get; set; }
        
        //Paginas
       // public System.Data.Entity.DbSet<WebApplication1.Models.DatosSolicitudPagina> DatosSolicitudPaginas { get; set; }

        //Entidades
        //public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.DocumentoVenta> Ventas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Orden> Ordens { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Solicitud> Solicituds { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.HojaProspectoViewModel> HojaProspectoViewModels { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Persona> Personas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Pagina> Paginas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.LineaSolicitud> LineaSolicituds { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Producto> Productoes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Cuenta> Cuentas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.RoleViewModel> RoleViewModels { get; set; }

        //public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.SolicitudesPersonas> SolicitudesPersonas { get; set; }

        //public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Prospecto> Prospectos { get; set; }

        //public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Empleado> Empleados { get; set; }
    }

    //public class InheritanceMappingContext : DbContext
    //{
    //    public DbSet<Pagina> Paginas { get; set; }
    //}
}
