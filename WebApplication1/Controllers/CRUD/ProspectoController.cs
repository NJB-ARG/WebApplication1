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

namespace WebApplication1.Controllers.CRUD
{
    public class ProspectoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prospecto
        public ActionResult Index()
        {
            //return View(db.Prospectos.ToList());
            return View(db.Personas.OfType<Prospecto>().ToList()); 
        }

        // GET: Prospecto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Prospecto prospecto = db.Prospectos.Find(id);
            Prospecto prospecto = db.Personas.OfType<Prospecto>().SingleOrDefault(s => s.PersonaID == id);
            if (prospecto == null)
            {
                return HttpNotFound();
            }
            return View(prospecto);
        }

        // GET: Prospecto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prospecto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonaID,PersonaNombre,PersonaApellido,PersonaTelefono,PersonaDireccion,PersonaCUIL,PersonaDni,PersonaMail,PersonaFechaNacimiento,PersonaSexo,PersonaNacionalidad,PersonaLocalidad,ProspectoUnidadBuscada,ProspectoProfesion,ProspectoMail2,ProspectoTelefono2,ProspectoFacebook,ProspectoTwiter,ProspectoHorarioContacto,ProspectoCanalInicial,ProspectoUsuarioDeMoto,ProspectoUnidadActual,ProspectoEntregaUnidad,ProspectoAnioUnidad,ProspectoKmUnidad,ProspectoConocimientoTecnico,ProspectoTipoUsoUnidad")] Prospecto prospecto)
        {
            if (ModelState.IsValid)
            {
                db.Personas.Add(prospecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prospecto);
        }

        // GET: Prospecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Prospecto prospecto = db.Prospectos.Find(id);
            Prospecto prospecto = db.Personas.OfType<Prospecto>().SingleOrDefault(s => s.PersonaID == id);
            if (prospecto == null)
            {
                return HttpNotFound();
            }
            return View(prospecto);
        }

        // POST: Prospecto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonaID,PersonaNombre,PersonaApellido,PersonaTelefono,PersonaDireccion,PersonaCUIL,PersonaDni,PersonaMail,PersonaFechaNacimiento,PersonaSexo,PersonaNacionalidad,PersonaLocalidad,ProspectoUnidadBuscada,ProspectoProfesion,ProspectoMail2,ProspectoTelefono2,ProspectoFacebook,ProspectoTwiter,ProspectoHorarioContacto,ProspectoCanalInicial,ProspectoUsuarioDeMoto,ProspectoUnidadActual,ProspectoEntregaUnidad,ProspectoAnioUnidad,ProspectoKmUnidad,ProspectoConocimientoTecnico,ProspectoTipoUsoUnidad")] Prospecto prospecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prospecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prospecto);
        }

        // GET: Prospecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Prospecto prospecto = db.Prospectos.Find(id);
            Prospecto prospecto = db.Personas.OfType<Prospecto>().SingleOrDefault(s => s.PersonaID == id);
            if (prospecto == null)
            {
                return HttpNotFound();
            }
            return View(prospecto);
        }

        // POST: Prospecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Prospecto prospecto = db.Prospectos.Find(id);
            Prospecto prospecto = db.Personas.OfType<Prospecto>().SingleOrDefault(s => s.PersonaID == id);
            db.Personas.Remove(prospecto);
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
