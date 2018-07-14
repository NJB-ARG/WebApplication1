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
    public class HojaProspectoViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HojaProspectoViewModels
        public ActionResult Index()
        {
            return View(db.HojaProspectoViewModels.ToList());
        }

        // GET: HojaProspectoViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HojaProspectoViewModel hojaProspectoViewModel = db.HojaProspectoViewModels.Find(id);
            if (hojaProspectoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(hojaProspectoViewModel);
        }

        // GET: HojaProspectoViewModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HojaProspectoViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HojaProspectoViewModelID,HojaProspectoFecCreacion,HojaProspectoFecUltMod,HojaProspectoComentario")] HojaProspectoViewModel hojaProspectoViewModel)
        {
            if (ModelState.IsValid)
            {
                db.HojaProspectoViewModels.Add(hojaProspectoViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hojaProspectoViewModel);
        }

        // GET: HojaProspectoViewModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HojaProspectoViewModel hojaProspectoViewModel = db.HojaProspectoViewModels.Find(id);
            if (hojaProspectoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(hojaProspectoViewModel);
        }

        // POST: HojaProspectoViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HojaProspectoViewModelID,HojaProspectoFecCreacion,HojaProspectoFecUltMod,HojaProspectoComentario")] HojaProspectoViewModel hojaProspectoViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hojaProspectoViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hojaProspectoViewModel);
        }

        // GET: HojaProspectoViewModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HojaProspectoViewModel hojaProspectoViewModel = db.HojaProspectoViewModels.Find(id);
            if (hojaProspectoViewModel == null)
            {
                return HttpNotFound();
            }
            return View(hojaProspectoViewModel);
        }

        // POST: HojaProspectoViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HojaProspectoViewModel hojaProspectoViewModel = db.HojaProspectoViewModels.Find(id);
            db.HojaProspectoViewModels.Remove(hojaProspectoViewModel);
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
