using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Service;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.HtmlHelperExtensions;
using System.Web.UI.WebControls;
using System.Net;

namespace WebApplication1.Repository
{
    public class OrdVentaRepository : WebApplication1.Repository.IOrdVentaRepository
    {
        //private OrdVentasDBEntities _entities = new OrdVentasDBEntities();
        private ApplicationDbContext _entities = new ApplicationDbContext();

        public IEnumerable<Solicitud> ListSolVentas()
        {
            var solicitudes = _entities.Solicituds
                                .Include(c => c.Solicitud_Cliente)
                                .Include(c => c.Solicitud_Prospecto)
                                .Include(c => c.Solicitud_Empleado)
                                .Include(c => c.Solicitud_Paginas.Select(p => p.Pagina_Orden))
                                .Include(c => c.Solicitud_Ordenes)
                                .Include(c => c.Solicitud_Sucursal.Sucursal_Empresa)
                                .Include(c => c.Solicitud_LineasSolicitud)
                                .Include(c => c.Solicitud_Pagos.Select(p => p.Pago_Cuenta));

            //return _entities.Solicituds.ToList();
            return solicitudes.ToList();
        }

        public List<String> EstadosOrdVentas()
        {
            var EstadoLst = new List<string>();

            //var EstadoQry = from d in _entities.Solicituds orderby d.SolicitudEstado select d.SolicitudEstado;

            var estados = Functions.GetEnumList<SolicitudStatus>();

            foreach (SolicitudStatus estado in estados)
            {
                //EstadoLst.Add(Functions.GetEnumDescription(estado));
                EstadoLst.Add(estado.ToString());
            }

            //EstadoLst.AddRange(EstadoQry.Distinct());

            return EstadoLst;
        }

        public IQueryable<Solicitud> FilterSolVentas(string solDesc, SolicitudStatus estSol, string solEst, string solEmp, string solSol, string solSuc, string solNum)
        {
            //var solventas = from s in _entities.Solicituds select s;
            var solventas = _entities.Solicituds
                                .Include(c => c.Solicitud_Cliente)
                                .Include(c => c.Solicitud_Prospecto)
                                .Include(c => c.Solicitud_Empleado)
                                .Include(c => c.Solicitud_Paginas.Select(p => p.Pagina_Orden))
                                .Include(c => c.Solicitud_Ordenes)
                                .Include(c => c.Solicitud_Sucursal.Sucursal_Empresa)
                                .Include(c => c.Solicitud_LineasSolicitud)
                                .Include(c => c.Solicitud_Pagos.Select(p => p.Pago_Cuenta));

            if (!String.IsNullOrEmpty(solDesc))
            {
                solventas = solventas.Where(s => s.SolicitudDescripcion.Contains(solDesc));
            }

            //if (!string.IsNullOrEmpty(Functions.GetEnumDescription(solventaestado)))
            if (!string.IsNullOrEmpty(solEst))
            {
                solventas = solventas.Where(x => x.SolicitudEstado == estSol);
            }

            if (!string.IsNullOrEmpty(solEmp))
            {
                solventas = solventas.Where(x => x.Solicitud_Empleado.PersonaNombre.Contains(solEmp) || x.Solicitud_Empleado.PersonaApellido.Contains(solEmp));
            }

            if (!string.IsNullOrEmpty(solSol))
            {
                solventas = solventas.Where(x => x.Solicitud_Cliente.PersonaNombre.Contains(solSol) || x.Solicitud_Prospecto.PersonaNombre.Contains(solSol) ||
                                            x.Solicitud_Cliente.PersonaApellido.Contains(solSol) || x.Solicitud_Prospecto.PersonaApellido.Contains(solSol));
            }

            if (!string.IsNullOrEmpty(solSuc))
            {
                solventas = solventas.Where(x => x.Solicitud_Sucursal.SucursalNombre.Contains(solSuc));
            }

            if (!string.IsNullOrEmpty(solNum))
            {
                solventas = solventas.Where(x => x.SolicitudNum == Int32.Parse(solNum));
            }

            return solventas;
        }

        public Solicitud BuscarSolVenta(int? id)
        {
            return _entities.Solicituds.Find(id);
        }

        public bool CreateOrdVenta(Orden ordventaToCreate)
        {
            try
            {
                _entities.Ordens.Add(ordventaToCreate);
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditOrdVenta(Orden ordventaToEdit)
        {
            try
            {
                _entities.Entry(ordventaToEdit).State = EntityState.Modified;
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteSolVenta(Solicitud solventaToDelete)
        {
            // Database logic
            try
            {
                _entities.Solicituds.Remove(solventaToDelete);
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _entities.Dispose();
        }

        //
        public IQueryable<LineaSolicitud> RecuperarLineas(int id_solventa)
        {
            return _entities.LineaSolicituds.Where(r => r.SolicitudID == id_solventa);
        }

        public int CreateSolVentaID(Solicitud solventaToCreate)
        {
            try
            {
                _entities.Solicituds.Add(solventaToCreate);
                _entities.SaveChanges();
                return solventaToCreate.SolicitudID;
            }
            catch
            {
                return 0;
            }
        }
    }

    public interface IOrdVentaRepository
    {
        bool CreateOrdVenta(Orden ordventaToCreate);
        IEnumerable<Solicitud> ListSolVentas();
        List<string> EstadosOrdVentas();
        IQueryable<Solicitud> FilterSolVentas(string solDesc, SolicitudStatus estSol, string solEst, string solEmp, string solSol, string solSuc, string solNum);
        Solicitud BuscarSolVenta(int? id);
        bool EditOrdVenta(Orden ordventaToEdit);
        bool DeleteSolVenta(Solicitud solventaToDelete);
        void Dispose();

        //
        IQueryable<LineaSolicitud> RecuperarLineas(int id_solventa);
        int CreateSolVentaID(Solicitud solventaToCreate);
    }
}