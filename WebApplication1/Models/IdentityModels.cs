using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using WebApplication1.Models.ViewModels;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //NJB-INI-Propiedades personalizadas (NJB-Security)
        public string CiudadNatal { get; set; }
        public System.DateTime? FechaNacimiento { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        // Use a sensible display name for views:
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        // Concatenate the address info for display in tables and such:
        public string DisplayAddress
        {
            get
            {
                string dspAddress =
                    string.IsNullOrWhiteSpace(this.Address) ? "" : this.Address;
                string dspCity =
                    string.IsNullOrWhiteSpace(this.City) ? "" : this.City;
                string dspState =
                    string.IsNullOrWhiteSpace(this.State) ? "" : this.State;
                string dspPostalCode =
                    string.IsNullOrWhiteSpace(this.PostalCode) ? "" : this.PostalCode;

                return string
                    .Format("{0} {1} {2} {3}", dspAddress, dspCity, dspState, dspPostalCode);
            }
        }
        //NJB-FIN-Propiedades personalizadas

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Navigation prop - Security
        public virtual Persona ApplicationUser_Persona { get; set; }
    }

    //NJB-INI - Adding a Customized Role to the Identity Samples Project
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }

        //Nav Prop - Security
        public virtual List<Perfil> ApplicationRole_Perfiles { get; set; }
    }
    //

    //this is the Entity Framework context used to manage interaction between our application and the database where our Account data is persisted
    //(which may, or may not be the same database that will be used by the rest of our application)
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }


        //Documento
        //public System.Data.Entity.DbSet<WebApplication1.Models.Documento> Documentos { get; set; }

        //Paginas
        // public System.Data.Entity.DbSet<WebApplication1.Models.DatosSolicitudPagina> DatosSolicitudPaginas { get; set; }

        //Entidades
        //public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Venta> Ventas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Orden> Ordens { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Solicitud> Solicituds { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.HojaProspectoViewModel> HojaProspectoViewModels { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Persona> Personas { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Cliente> Clientes { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Empleado> Empleados { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Prospecto> Prospectos { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Pagina> Paginas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.OfertasMercado> OfertasMercado { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PropuestaComercial> PropuestaComercial { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Seguro> Seguro { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.CuentaDeposito> CuentaDeposito { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.PagosRealizados> PagosRealizados { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.DatosUnidad> DatosUnidad { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.DatosContacto> DatosContacto { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.DocAdjunto> DocAdjunto { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ServicioTecnico> ServicioTecnico { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Gestoria> Gestoria { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Comentario> Comentario { get; set; }       

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.LineaSolicitud> LineaSolicituds { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Producto> Productos { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Proveedor> Proveedores { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Sucursal> Sucursales { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Cuenta> Cuentas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.Modulo> Modulos { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Controladora> Controladoras { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Accion> Acciones { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.Perfil> Perfiles { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ActionLog> ActionLogs { get; set; }
        public System.Data.Entity.DbSet<WebApplication1.Models.ApplicationRole> ApplicationRoles { get; set; }

        //NJB-INI - El database initializer
        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        //NJB-FIN - El database initializer

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Empresa> Empresas { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.TipoDepartamento> TipoDepartamentoes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.Pago> Pagoes { get; set; }

        public System.Data.Entity.DbSet<WebApplication1.Models.ViewModels.DocumentoAdjunto> DocumentoAdjuntoes { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        //    modelBuilder.Entity<RolControladoraAccion>()
        //        .HasRequired(d => d.controladora)
        //        .WithMany()
        //        .HasForeignKey(d => d.ControladoraID)
        //        .WillCascadeOnDelete(false);
        //}
    }
}


//The ClaimsIdentity class is a concrete implementation of a claims-based identity; that is, an identity described by a collection of claims.
//A claim is a statement about an entity made by an issuer that describes a property, right, or some other quality of that entity. 
//Such an entity is said to be the subject of the claim. A claim is represented by the Claim class. 
//The claims contained in a ClaimsIdentity describe the entity that the corresponding identity represents, and can be used to make authorization and authentication decisions.
//A claims-based access model has many advantages over more traditional access models that rely exclusively on roles. 
//For example, claims can provide much richer information about the identity they represent and can be evaluated for authorization or authentication in a far more specific manner.