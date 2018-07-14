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
    public class TipoDepartamentoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TipoDepartamento
        public ActionResult Index()
        {
            return View(db.TipoDepartamentoes.ToList());
        }

        // GET: TipoDepartamento/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDepartamento tipoDepartamento = db.TipoDepartamentoes.Find(id);
            if (tipoDepartamento == null)
            {
                return HttpNotFound();
            }
            return View(tipoDepartamento);
        }

        // GET: TipoDepartamento/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoDepartamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TipoDepartamentoID,TipoDepartamentoNombre,TipoDepartamentoDesc")] TipoDepartamento tipoDepartamento)
        {
            if (ModelState.IsValid)
            {
                db.TipoDepartamentoes.Add(tipoDepartamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoDepartamento);
        }

        // GET: TipoDepartamento/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDepartamento tipoDepartamento = db.TipoDepartamentoes.Find(id);
            if (tipoDepartamento == null)
            {
                return HttpNotFound();
            }
            return View(tipoDepartamento);
        }

        // POST: TipoDepartamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TipoDepartamentoID,TipoDepartamentoNombre,TipoDepartamentoDesc")] TipoDepartamento tipoDepartamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoDepartamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoDepartamento);
        }

        // GET: TipoDepartamento/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoDepartamento tipoDepartamento = db.TipoDepartamentoes.Find(id);
            if (tipoDepartamento == null)
            {
                return HttpNotFound();
            }
            return View(tipoDepartamento);
        }

        // POST: TipoDepartamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoDepartamento tipoDepartamento = db.TipoDepartamentoes.Find(id);
            db.TipoDepartamentoes.Remove(tipoDepartamento);
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
