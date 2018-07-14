using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Service;
using WebApplication1.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Repository
{
    public class IdentityRepository : Controller, IidentityRepository
    {
        public IdentityRepository()
        {
        }

        public IdentityRepository(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        //Creo los managers - INI
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }        
        }

        ApplicationDbContext _dbContext;
        public ApplicationDbContext DbOwinContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }
    //Creo los managers - FIN
        
        private ApplicationDbContext _entities = new ApplicationDbContext();

        //public IEnumerable<RolOperaciones> ListRolOperaciones()
        //{
        //    var roloperaciones = _entities.RolOperaciones
        //    .Include(r => r.RolOperaciones_Operacion.Operacion_Controladora)
        //    .Include(r => r.RolOperaciones_Operacion.Operacion_Accion)
        //    .Include(r => r.RolOperaciones_Rol);

        //    //return _entities.Solicituds.ToList();
        //    return roloperaciones.ToList();
        //}

        //public List<String> EstadosSolVentas()
        //{
        //    var EstadoLst = new List<string>();

        //    //var EstadoQry = from d in _entities.Solicituds orderby d.SolicitudEstado select d.SolicitudEstado;

        //    var estados = Functions.GetEnumList<SolicitudStatus>();

        //    foreach (SolicitudStatus estado in estados)
        //    {
        //        EstadoLst.Add(Functions.GetEnumDescription(estado));
        //    }

        //    //EstadoLst.AddRange(EstadoQry.Distinct());

        //    return EstadoLst;
        //}

        //public IQueryable<Solicitud> FilterSolVentas(string searchString, SolicitudStatus solventaestado)
        //{
        //    //var solventas = from s in _entities.Solicituds select s;
        //    var solventas = _entities.Solicituds
        //                        .Include(c => c.Solicitud_Cliente)
        //                        .Include(c => c.Solicitud_Prospecto)
        //                        .Include(c => c.Solicitud_Empleado)
        //                        .Include(c => c.Solicitud_Paginas)
        //                        .Include(c => c.Solicitud_Ordenes)
        //                        .Include(c => c.Solicitud_LineasSolicitud)
        //                        .Include(c => c.Solicitud_Pagos.Select(p => p.Pago_Cuenta));

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        solventas = solventas.Where(s => s.SolicitudDescripcion.Contains(searchString));
        //    }

        //    if (!string.IsNullOrEmpty(Functions.GetEnumDescription(solventaestado)))
        //    {
        //        solventas = solventas.Where(x => x.SolicitudEstado == solventaestado);
        //    }

        //    return solventas;
        //}

        public async Task<ApplicationRole> BuscarRol(string id)
        {
            return await RoleManager.FindByIdAsync(id);
        }

        //public bool CreateSolVenta(Solicitud solventaToCreate)
        //{
        //    try
        //    {
        //        _entities.Solicituds.Add(solventaToCreate);
        //        _entities.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool EditSolVenta(Solicitud solventaToEdit)
        //{
        //    try
        //    {
        //        _entities.Entry(solventaToEdit).State = EntityState.Modified;
        //        _entities.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteSolVenta(Solicitud solventaToDelete)
        //{
        //    // Database logic
        //    try
        //    {
        //        _entities.Solicituds.Remove(solventaToDelete);
        //        _entities.SaveChanges();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public new void Dispose()
        {
            DbOwinContext.Dispose();
        }
    }

    public interface IidentityRepository
    {
        //bool CreateSolVenta(Solicitud solventaToCreate);
        //IEnumerable<RolOperaciones> ListRolOperaciones();
        //List<String> EstadosSolVentas();
        //IQueryable<Solicitud> FilterSolVentas(string searchString, SolicitudStatus solventaestado);
        Task<ApplicationRole> BuscarRol(string id);
        //bool EditSolVenta(Solicitud solventaToEdit);
        //bool DeleteSolVenta(Solicitud solventaToDelete);
        void Dispose();
    }
}