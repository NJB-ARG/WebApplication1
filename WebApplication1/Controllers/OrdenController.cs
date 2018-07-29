using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using WebApplication1.Repository;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class OrdenController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //*********************
        private IOrdVentaService _service;

        public OrdenController()
        {
            _service = new OrdVentaService(this.ModelState, new OrdVentaRepository());
        }

        public OrdenController(IOrdVentaService service)
        {
            _service = service;
        }
        //*********************
        
        // GET: Orden
        public ActionResult Index()
        {
            var ordens = db.Ordens.Include(o => o.Orden_Solicitud);
            return View(ordens.ToList());
        }

        // GET: Orden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordens.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: Orden/Create
        public ActionResult Create(int? idSolicitud)
        {
            if (idSolicitud == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Solicitud solicitud = db.Solicituds.Find(idSolicitud);
            Solicitud solicitud = _service.BuscarSolVenta(idSolicitud);
            if (solicitud == null)
            {
                return HttpNotFound();
            }
           
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID");
            ViewBag.SolicitudID = idSolicitud;          
            ViewBag.LineasOrden = solicitud.Solicitud_LineasSolicitud;
            return View();
        }

        // POST: Orden/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost(int? idSolicitud)
        {                        

            if (idSolicitud == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Solicitud solicitud = db.Solicituds.Find(idSolicitud);
            Solicitud solicitud = _service.BuscarSolVenta(idSolicitud);
            if (solicitud == null)
            {
                return HttpNotFound();
            }

            Orden orden = new Orden
            {
                Orden_Solicitud = solicitud,
                SolicitudID = solicitud.SolicitudID
            };

            if (ModelState.IsValid)
            {
                //******
                //LLAMADA PATRONES-INI
                //------------------------------------------------------------------------------------------------------------------------------------------
                //--CREACIÓN DEL OBJETO COMPLEJO "SOLICITUD" PASO 1--
                //Patrón Builder: CREA un documento con sus respectivas páginas de datos vacías, las cuales se completan a medida que se avanza con el desarrollo del documento en cuestión  
                //------------------------------------------------------------------------------------------------------------------------------------------

                //The 'Builder' abstract class. La clase abstracta para asignar la clase concreta que quiero crear. En este caso: SOLICITUD
                DocumentoBuilder ordenBuilder;

                //The 'Director' class. Es la clase que usa el objeto complejo concreto para ejecutar el método "construir" que tiene los pasos de creación de cada parte de un documento
                DirectorDocumento directorSolicitud = new DirectorDocumento();

                // Construyo el documento complejo que necesite vacío
                ordenBuilder = new OrdenBuilder();
                directorSolicitud.Construir(ordenBuilder);

                //Cargo el list de paginas del documento en cración. VIEWMODEL a renderizar en la vista
                //ConstructorDocumento.Documento.partes

                orden.Orden_Paginas = ordenBuilder.Documento.paginas.Values.ToList();
                Documento doc = new Documento();

                doc = ordenBuilder.Documento;
                //LLAMADA PATRONES-FIN
                //******

                //db.Ordens.Add(orden);
                //db.SaveChanges();
                _service.CreateOrdVenta(orden);
                return RedirectToAction("Index");
            }

            ViewBag.SolicitudID = new SelectList(db.Solicituds.Where(r => r.SolicitudID == solicitud.SolicitudID), "SolicitudID", "SolicitudOwnerID", orden.SolicitudID);
            return View(orden);
        }

        // GET: Orden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordens.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", orden.SolicitudID);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrdenID,SolicitudID")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", orden.SolicitudID);
            return View(orden);
        }

        // GET: Orden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordens.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: Orden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orden orden = db.Ordens.Find(id);
            db.Ordens.Remove(orden);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
