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
    public class PagoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pago
        public ActionResult Index()
        {
            var pagoes = db.Pagoes.Include(p => p.Pago_Cuenta).Include(p => p.Pago_Solicitud);
            return View(pagoes.ToList());
        }

        // GET: Pago/Details/5
        public ActionResult Details(int? id, int? solicitudID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagoes.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.SolicitudID = solicitudID;
            return View(pago);
        }

        // GET: Pago/Create
        public ActionResult Create(int? solicitudID)
        {
            ViewBag.CuentaID = new SelectList(db.Cuentas, "CuentaID", "CuentaTitular");
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID");
            ViewBag.SolicitudID = solicitudID;
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PagoID,PagoFecha,PagoDescripcion,PagoMonto,SolicitudID,CuentaID")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Pagoes.Add(pago);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", "Solicitud", new { id = pago.SolicitudID });
            }

            ViewBag.CuentaID = new SelectList(db.Cuentas, "CuentaID", "CuentaTitular", pago.CuentaID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", pago.SolicitudID);
            return View(pago);
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int? id, int? solicitudID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagoes.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.CuentaID = new SelectList(db.Cuentas, "CuentaID", "CuentaTitular", pago.CuentaID);
            //ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", pago.SolicitudID);
            ViewBag.SolicitudID = solicitudID;
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PagoID,PagoFecha,PagoDescripcion,PagoMonto,SolicitudID,CuentaID")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pago).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", "Solicitud", new { id = pago.SolicitudID });
            }
            ViewBag.CuentaID = new SelectList(db.Cuentas, "CuentaID", "CuentaTitular", pago.CuentaID);
            ViewBag.SolicitudID = new SelectList(db.Solicituds, "SolicitudID", "SolicitudOwnerID", pago.SolicitudID);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public ActionResult Delete(int? id, int? solicitudID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pago pago = db.Pagoes.Find(id);
            if (pago == null)
            {
                return HttpNotFound();
            }
            ViewBag.SolicitudID = solicitudID;
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pago pago = db.Pagoes.Find(id);
            db.Pagoes.Remove(pago);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Edit", "Solicitud", new { id = pago.SolicitudID });
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
