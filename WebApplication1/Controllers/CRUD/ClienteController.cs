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
    public class ClienteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cliente
        //public ActionResult Index()
        //{
        //    return View(db.Personas.OfType<Cliente>().ToList());
        //}
        public ActionResult Index(int? id, int? solicitudID)
        {
            var viewModel = new ClienteIndexViewModel();
            viewModel.Clientes = db.Personas.OfType<Cliente>()
                .Include(i => i.Cliente_Solicitudes.Select(c => c.Solicitud_Paginas))
                .Include(i => i.Cliente_Solicitudes.Select(c => c.Solicitud_LineasSolicitud))
                .Include(i => i.Cliente_Solicitudes.Select(c => c.Solicitud_Cliente))
                .Include(i => i.Cliente_Solicitudes.Select(c => c.Solicitud_Empleado))
                .Include(i => i.Cliente_Solicitudes.Select(c => c.Solicitud_Prospecto))
                .OrderBy(i => i.PersonaApellido);

            if (id != null)
            {
                ViewBag.ClienteID = id.Value;
                viewModel.Solicitudes = viewModel.Clientes.Where(i => i.PersonaID == id.Value).Single().Cliente_Solicitudes;
            }

            if (solicitudID != null)
            {
                ViewBag.SolicitudID = solicitudID.Value;
                viewModel.Paginas = viewModel.Solicitudes.Where(x => x.SolicitudID == solicitudID).Single().Solicitud_Paginas;
                viewModel.LineasSolicitud = viewModel.Solicitudes.Where(x => x.SolicitudID == solicitudID).Single().Solicitud_LineasSolicitud;

                // Lazy loading
                //viewModel.Enrollments = viewModel.Courses.Where(
                //    x => x.CourseID == courseID).Single().Enrollments;
                // Explicit loading
                //var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseID).Single();
                //db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                //foreach (Enrollment enrollment in selectedCourse.Enrollments)
                //{
                //    db.Entry(enrollment).Reference(x => x.Student).Load();
                //}

                //viewModel.Enrollments = selectedCourse.Enrollments;
            }

            return View(viewModel);
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Cliente cliente = db.Personas.Find(id);
            Cliente cliente = db.Personas.OfType<Cliente>().SingleOrDefault(s => s.PersonaID == id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonaID,PersonaNombre,PersonaApellido,PersonaTelefono,PersonaDireccion,PersonaCUIL,PersonaDni,PersonaMail,PersonaFechaNacimiento,PersonaSexo,PersonaNacionalidad,PersonaLocalidad")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Personas.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Cliente cliente = db.Personas.Find(id);

            //Instructor instructor = db.Instructors
            //.Include(i => i.OfficeAssignment)
            //.Where(i => i.ID == id)
            //.Single();

            Cliente cliente = db.Personas.OfType<Cliente>().SingleOrDefault(s => s.PersonaID == id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonaID,PersonaNombre,PersonaApellido,PersonaTelefono,PersonaDireccion,PersonaCUIL,PersonaDni,PersonaMail,PersonaFechaNacimiento,PersonaSexo,PersonaNacionalidad,PersonaLocalidad")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Cliente cliente = db.Personas.Find(id);
            Cliente cliente = db.Personas.OfType<Cliente>().SingleOrDefault(s => s.PersonaID == id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Cliente cliente = db.Personas.Find(id);
            Cliente cliente = db.Personas.OfType<Cliente>().SingleOrDefault(s => s.PersonaID == id);
            db.Personas.Remove(cliente);
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
