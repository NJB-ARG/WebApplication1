using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Paginas
{
    public class ComentarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comentario
        public ActionResult Index()
        {
            var paginas = db.Paginas.OfType<Comentario>().Include(c => c.Pagina_Orden).Include(c => c.Pagina_Solicitud);
            return View(paginas.ToList());
        }

        // GET: Comentario/Details/5
        public ActionResult Details(int? id, int? DocumentoID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Comentario comentario = db.Paginas.Find(id);
            Comentario comentario = db.Paginas.OfType<Comentario>().SingleOrDefault(s => s.PaginaID == id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            return View(comentario);
        }

        // GET: Comentario/Create
        public ActionResult Create(int? DocumentoID, string Controladora)
        {
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID");
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID");
            //return View();

            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            Comentario comentario = new Comentario();
            comentario.ComentarioPersona = HttpContext.User.Identity.Name;
            ViewBag.ComentarioPersona = comentario.ComentarioPersona;
            comentario.PaginaNombre = "Comentarios";
            ViewBag.PaginaNombre = comentario.PaginaNombre;
            comentario.PaginaTipo = "D";
            ViewBag.PaginaTipo = comentario.PaginaTipo;
            comentario.PaginaTipoDocumento = Controladora;
            ViewBag.PaginaTipoDocumento = comentario.PaginaTipoDocumento;
            comentario.PaginaNumero = 8;
            ViewBag.PaginaNumero = comentario.PaginaNumero;

            if (Controladora == "Solicitud")
            {
                comentario.SolicitudID = DocumentoID;
                comentario.OrdenID = null;
            }
            else if (Controladora == "Orden")
            {
                comentario.OrdenID = DocumentoID;
                comentario.SolicitudID = null;
            }

            return View(comentario);
        }

        // POST: Comentario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaginaID,PaginaNumero,PaginaNombre,PaginaValidada,PaginaTipoDocumento,PaginaTipo,SolicitudID,OrdenID,ComentarioNum,ComentarioFecha,ComentarioPersona,ComentarioTexto")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Paginas.Add(comentario);
                db.SaveChanges();
                //return RedirectToAction("Index");
                if (comentario.SolicitudID != null)
                {
                    return RedirectToAction("Edit", "Solicitud", new { id = comentario.SolicitudID });
                }
                else if (comentario.OrdenID != null)
                {
                    return RedirectToAction("Edit", "Orden", new { id = comentario.OrdenID });
                }
            }

            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", comentario.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", comentario.SolicitudID);
            return View(comentario);
        }

        // GET: Comentario/Edit/5
        public ActionResult Edit(int? id, int? DocumentoID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Comentario comentario = db.Paginas.Find(id);
            Comentario comentario = db.Paginas.OfType<Comentario>().SingleOrDefault(s => s.PaginaID == id);
            comentario.ComentarioPersona = HttpContext.User.Identity.Name;
            ViewBag.ComentarioPersona = comentario.ComentarioPersona;
            comentario.PaginaNombre = "Comentarios";
            ViewBag.PaginaNombre = comentario.PaginaNombre;
            comentario.PaginaTipo = "D";
            ViewBag.PaginaTipo = comentario.PaginaTipo;
            comentario.PaginaTipoDocumento = Controladora;
            ViewBag.PaginaTipoDocumento = comentario.PaginaTipoDocumento;
            comentario.PaginaNumero = 8;
            ViewBag.PaginaNumero = comentario.PaginaNumero;
            if (comentario == null)
            {
                return HttpNotFound();
            }
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", comentario.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", comentario.SolicitudID);
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            return View(comentario);
        }

        // POST: Comentario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaginaID,PaginaNumero,PaginaNombre,PaginaValidada,PaginaTipoDocumento,SolicitudID,OrdenID,ComentarioNum,ComentarioFecha,ComentarioPersona,ComentarioTexto")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentario).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                if (comentario.SolicitudID != null)
                {
                    return RedirectToAction("Edit", "Solicitud", new { id = comentario.SolicitudID });
                }
                else if (comentario.OrdenID != null)
                {
                    return RedirectToAction("Edit", "Orden", new { id = comentario.OrdenID });
                }
            }
            //ViewBag.OrdenID = new SelectList(db.Ordens, "OrdenID", "OrdenID", comentario.OrdenID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", comentario.SolicitudID);
            return View(comentario);
        }

        // GET: Comentario/Delete/5
        public ActionResult Delete(int? id, int? DocumentoID, string Controladora)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Comentario comentario = db.Paginas.Find(id);
            Comentario comentario = db.Paginas.OfType<Comentario>().SingleOrDefault(s => s.PaginaID == id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.DocumentoID = DocumentoID;
            ViewBag.Controladora = Controladora;
            return View(comentario);
        }

        // POST: Comentario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? DocumentoID, string Controladora)
        {
            //Comentario comentario = db.Paginas.Find(id);
            Comentario comentario = db.Paginas.OfType<Comentario>().SingleOrDefault(s => s.PaginaID == id);
            db.Paginas.Remove(comentario);
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
