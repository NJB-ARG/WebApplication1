using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Routing;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;

namespace WebApplication1.Controllers.CRUD
{
    public class DocumentoAdjuntoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DocumentoAdjunto
        public ActionResult Index()
        {
            var documentoAdjuntoes = db.DocumentoAdjuntoes.Include(d => d.DocumentoAdjunto_Orden).Include(d => d.DocumentoAdjunto_Solicitud);
            return View(documentoAdjuntoes.ToList());
        }

        // GET: DocumentoAdjunto/Details/5
        public ActionResult Details(int? id, int? DocumentoID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentoAdjunto documentoAdjunto = db.DocumentoAdjuntoes.Find(id);
            if (documentoAdjunto == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            return View(documentoAdjunto);
        }

        // GET: DocumentoAdjunto/Create
        public ActionResult Create(int? DocumentoID, string Controladora)
        {
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID");
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID");
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            DocumentoAdjunto documentoAdjunto = new DocumentoAdjunto();
            if (Controladora == "Solicitud")
            {
                documentoAdjunto.SolicitudID = DocumentoID;
                documentoAdjunto.OrdenID = null;
            }
            else if (Controladora == "Orden")
            {
                documentoAdjunto.OrdenID = DocumentoID;
                documentoAdjunto.SolicitudID = null;
            }
                       
            return View(documentoAdjunto);
        }

        // POST: DocumentoAdjunto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DocumentoAdjuntoID,DocumentoAdjuntoFechaCreacion,DocumentoAdjuntoFechaAprobacion,DocumentoAdjuntoCargado,DocumentoAdjuntoAprobado,DocumentoAdjuntoComentario,DocumentoAdjuntoTipo,SolicitudID,OrdenID")] DocumentoAdjunto documentoAdjunto)
        {
            if (ModelState.IsValid)
            {
                db.DocumentoAdjuntoes.Add(documentoAdjunto);
                db.SaveChanges();
                //return RedirectToAction("Index");
                if (documentoAdjunto.SolicitudID != null)
                {
                    return RedirectToAction("Edit", "Solicitud", new { id = documentoAdjunto.SolicitudID });
                }
                else if (documentoAdjunto.OrdenID != null)
                {
                    return RedirectToAction("Edit", "Orden", new { id = documentoAdjunto.OrdenID });
                }                
            }

            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", documentoAdjunto.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudID", documentoAdjunto.SolicitudID);
            return View(documentoAdjunto);
        }

        // GET: DocumentoAdjunto/Edit/5
        public ActionResult Edit(int? id, int? DocumentoID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentoAdjunto documentoAdjunto = db.DocumentoAdjuntoes.Find(id);
            if (documentoAdjunto == null)
            {
                return HttpNotFound();
            }
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", documentoAdjunto.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", documentoAdjunto.SolicitudID);
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            return View(documentoAdjunto);
        }

        // POST: DocumentoAdjunto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DocumentoAdjuntoID,DocumentoAdjuntoFechaCreacion,DocumentoAdjuntoFechaAprobacion,DocumentoAdjuntoCargado,DocumentoAdjuntoAprobado,DocumentoAdjuntoComentario,DocumentoAdjuntoTipo,SolicitudID,OrdenID")] DocumentoAdjunto documentoAdjunto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentoAdjunto).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                if (documentoAdjunto.SolicitudID != null)
                {
                    return RedirectToAction("Edit", "Solicitud", new { id = documentoAdjunto.SolicitudID });
                }
                else if (documentoAdjunto.OrdenID != null)
                {
                    return RedirectToAction("Edit", "Orden", new { id = documentoAdjunto.OrdenID });
                }                
            }
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", documentoAdjunto.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudID", documentoAdjunto.SolicitudID);
            return View(documentoAdjunto);
        }

        // GET: DocumentoAdjunto/Delete/5
        public ActionResult Delete(int? id, int? DocumentoID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentoAdjunto documentoAdjunto = db.DocumentoAdjuntoes.Find(id);
            if (documentoAdjunto == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            return View(documentoAdjunto);
        }

        // POST: DocumentoAdjunto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? DocumentoID, string Controladora)
        {
            DocumentoAdjunto documentoAdjunto = db.DocumentoAdjuntoes.Find(id);
            db.DocumentoAdjuntoes.Remove(documentoAdjunto);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", Controladora, new { id = DocumentoID });
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
