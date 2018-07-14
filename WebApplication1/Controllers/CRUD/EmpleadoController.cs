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
    public class EmpleadoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Empleado
        public ActionResult Index()
        {
            return View(db.Personas.OfType<Empleado>().ToList());
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Empleado empleado = db.Empleados.Find(id);
            Empleado empleado = db.Personas.OfType<Empleado>().SingleOrDefault(s => s.PersonaID == id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: Empleado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleado/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonaID,PersonaNombre,PersonaApellido,PersonaTelefono,PersonaDireccion,PersonaCUIL,PersonaDni,PersonaMail,PersonaFechaNacimiento,PersonaSexo,PersonaNacionalidad,PersonaLocalidad,EmpleadoSector,EmpleadoTipo,EmpleadoTipoDesc,EmpleadoNivel")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Personas.Add(empleado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empleado);
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Empleado empleado = db.Empleados.Find(id);
            Empleado empleado = db.Personas.OfType<Empleado>().SingleOrDefault(s => s.PersonaID == id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonaID,PersonaNombre,PersonaApellido,PersonaTelefono,PersonaDireccion,PersonaCUIL,PersonaDni,PersonaMail,PersonaFechaNacimiento,PersonaSexo,PersonaNacionalidad,PersonaLocalidad,EmpleadoSector,EmpleadoTipo,EmpleadoTipoDesc,EmpleadoNivel")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Empleado empleado = db.Empleados.Find(id);
            Empleado empleado = db.Personas.OfType<Empleado>().SingleOrDefault(s => s.PersonaID == id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Empleado empleado = db.Empleados.Find(id);
            Empleado empleado = db.Personas.OfType<Empleado>().SingleOrDefault(s => s.PersonaID == id);
            db.Personas.Remove(empleado);
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
