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
    public class SucursalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sucursal
        public ActionResult Index()
        {
            var sucursales = db.Sucursales.Include(s => s.Sucursal_Empresa);
            return View(sucursales.ToList());
        }

        // GET: Sucursal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sucursal sucursal = db.Sucursales.Find(id);
            if (sucursal == null)
            {
                return HttpNotFound();
            }
            return View(sucursal);
        }

        // GET: Sucursal/Create
        public ActionResult Create()
        {
            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "EmpresaNombre");
            return View();
        }

        // POST: Sucursal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SucursalID,SucursalNombre,SucursalDireccion,EmpresaID")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                db.Sucursales.Add(sucursal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "EmpresaNombre", sucursal.EmpresaID);
            return View(sucursal);
        }

        // GET: Sucursal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sucursal sucursal = db.Sucursales.Find(id);
            if (sucursal == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "EmpresaNombre", sucursal.EmpresaID);
            return View(sucursal);
        }

        // POST: Sucursal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SucursalID,SucursalNombre,SucursalDireccion,EmpresaID")] Sucursal sucursal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sucursal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaID = new SelectList(db.Empresas, "EmpresaID", "EmpresaNombre", sucursal.EmpresaID);
            return View(sucursal);
        }

        // GET: Sucursal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sucursal sucursal = db.Sucursales.Find(id);
            if (sucursal == null)
            {
                return HttpNotFound();
            }
            return View(sucursal);
        }

        // POST: Sucursal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sucursal sucursal = db.Sucursales.Find(id);
            db.Sucursales.Remove(sucursal);
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
