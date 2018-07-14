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

namespace WebApplication1.Controllers
{
    public class LineaSolicitudController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LineaSolicitud
        public ActionResult Index()
        {
            var lineaSolicituds = db.LineaSolicituds.Include(l => l.LineaSolicitud_Producto).Include(l => l.LineaSolicitud_Solicitud);
            return View(lineaSolicituds.ToList());
        }

        // GET: LineaSolicitud/Details/5
        public ActionResult Details(int? id, int? solicitudID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineaSolicitud lineaSolicitud = db.LineaSolicituds.Find(id);
            if (lineaSolicitud == null)
            {
                return HttpNotFound();
            }
            ViewBag.SolicitudID = solicitudID;
            return View(lineaSolicitud);
        }

        // GET: LineaSolicitud/Create
        public ActionResult Create(int? solicitudID)
        {           
            ViewBag.ProductoID = new SelectList(db.Productos, "ProductoID", "ProductoDesc");
            //ViewBag.SolicitudID = new SelectList(db.Solicituds.AsEnumerable(), "SolicitudID", "SolicitudDescripcion", solicitudID);
            ViewBag.SolicitudID = solicitudID;
            ViewBag.LineaSolicitudNum = db.LineaSolicituds.Where(r => r.LineaSolicitud_Solicitud.SolicitudID == solicitudID).Count() + 1;
            return View();
        }

        // POST: LineaSolicitud/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LineaSolicitudID,SolicitudID,ProductoID,LineaSolicitudNum,LineaSolicitudCantidad")] LineaSolicitud lineaSolicitud)
        {
            if (ModelState.IsValid)
            {
                lineaSolicitud.LineaSolicitudPUprod = db.Productos.Where(r => r.ProductoID == lineaSolicitud.ProductoID).Single().ProductoPrecio;
                lineaSolicitud.LineaSolicitudMoneda = db.Productos.Where(r => r.ProductoID == lineaSolicitud.ProductoID).Single().ProductoMoneda;
                lineaSolicitud.LineaSolicitudMonto = lineaSolicitud.LineaSolicitudPUprod * lineaSolicitud.LineaSolicitudCantidad;
                lineaSolicitud.LineaSolicitudNum = db.LineaSolicituds.Where(r => r.LineaSolicitud_Solicitud.SolicitudID == lineaSolicitud.SolicitudID).Count() + 1;
                db.LineaSolicituds.Add(lineaSolicitud);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", "Solicitud", new { id = lineaSolicitud.SolicitudID });
            }
            //Por si da error mantengo los valores para ProductoID y SolicitudID
            ViewBag.ProductoID = new SelectList(db.Productos, "ProductoID", "ProductoDesc", lineaSolicitud.ProductoID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudDescripcion", lineaSolicitud.SolicitudID);
            return View(lineaSolicitud);
        }

        // GET: LineaSolicitud/Edit/5
        public ActionResult Edit(int? id, int? solicitudID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineaSolicitud lineaSolicitud = db.LineaSolicituds.Find(id);
            if (lineaSolicitud == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ProductoID = new SelectList(db.Productos, "ProductoID", "ProductoDesc", lineaSolicitud.ProductoID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudDescripcion", lineaSolicitud.SolicitudID);  
            ViewBag.SolicitudProveedorID = new SelectList(db.Proveedores, "ProveedorID", "ProveedorNombre");
            ViewBag.LineaSolicitudNum = lineaSolicitud.LineaSolicitudNum;
            ViewBag.SolicitudID = solicitudID;            
            ViewBag.Controladora = Controladora;
            ViewBag.ProductoID = lineaSolicitud.ProductoID;
            return View(lineaSolicitud);
        }

        // POST: LineaSolicitud/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LineaSolicitudID,SolicitudID,ProductoID,LineaSolicitudNum,LineaSolicitudCantidad,SolicitudProveedorID,SolicitudProveedorNombre")] LineaSolicitud lineaSolicitud)
        {
            if (ModelState.IsValid)
            {
                if (lineaSolicitud.SolicitudProveedorID == null)
                {
                    lineaSolicitud.LineaSolicitudPUprod = db.Productos.Where(r => r.ProductoID == lineaSolicitud.ProductoID).Single().ProductoPrecio;
                    lineaSolicitud.LineaSolicitudMoneda = db.Productos.Where(r => r.ProductoID == lineaSolicitud.ProductoID).Single().ProductoMoneda;
                    lineaSolicitud.LineaSolicitudMonto = lineaSolicitud.LineaSolicitudPUprod * lineaSolicitud.LineaSolicitudCantidad;
                    lineaSolicitud.LineaSolicitudNum = db.LineaSolicituds.AsNoTracking().Where(r => r.LineaSolicitudID == lineaSolicitud.LineaSolicitudID).Single().LineaSolicitudNum;
                    db.Entry(lineaSolicitud).State = EntityState.Modified;
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                    return RedirectToAction("Edit", "Solicitud", new { id = lineaSolicitud.SolicitudID });
                }
                else
                {
                    db.Entry(lineaSolicitud).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Create", "Orden", new { idSolicitud = lineaSolicitud.SolicitudID });
                }
                
            }
            // si da error
            ViewBag.ProductoID = new SelectList(db.Productos, "ProductoID", "ProductoDesc", lineaSolicitud.ProductoID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudDescripcion", lineaSolicitud.SolicitudID);
            return View(lineaSolicitud);
        }

        // GET: LineaSolicitud/Delete/5
        public ActionResult Delete(int? id, int? solicitudID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LineaSolicitud lineaSolicitud = db.LineaSolicituds.Find(id);
            if (lineaSolicitud == null)
            {
                return HttpNotFound();
            }
            ViewBag.SolicitudID = solicitudID;
            return View(lineaSolicitud);
        }

        // POST: LineaSolicitud/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LineaSolicitud lineaSolicitud = db.LineaSolicituds.Find(id);
            db.LineaSolicituds.Remove(lineaSolicitud);
            db.SaveChanges();

            int num_linea = 0;
            LineaSolicitud linea_update = new LineaSolicitud();
            var lineasToUpdate = db.LineaSolicituds.Where(r => r.SolicitudID == lineaSolicitud.SolicitudID).ToList();
            foreach (LineaSolicitud linea in lineasToUpdate)
            {                
                linea_update = db.LineaSolicituds.Find(linea.LineaSolicitudID);
                linea_update.LineaSolicitudNum = ++num_linea;
                db.Entry(linea_update).State = EntityState.Modified;
                db.SaveChanges();                                
            }
                        
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", "Solicitud", new { id = lineaSolicitud.SolicitudID });
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
