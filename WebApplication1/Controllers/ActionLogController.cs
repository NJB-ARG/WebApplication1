using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ActionLogController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ActionLog
        public ActionResult Index()
        {
            return View(db.ActionLogs.ToList());
        }

        // GET: ActionLog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionLog actionLog = db.ActionLogs.Find(id);
            if (actionLog == null)
            {
                return HttpNotFound();
            }
            return View(actionLog);
        }

        // GET: ActionLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActionLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActionLogID,Controller,Action,IP,DateTime")] ActionLog actionLog)
        {
            if (ModelState.IsValid)
            {
                db.ActionLogs.Add(actionLog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(actionLog);
        }

        // GET: ActionLog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionLog actionLog = db.ActionLogs.Find(id);
            if (actionLog == null)
            {
                return HttpNotFound();
            }
            return View(actionLog);
        }

        // POST: ActionLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActionLogID,Controller,Action,IP,DateTime")] ActionLog actionLog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionLog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actionLog);
        }

        // GET: ActionLog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionLog actionLog = db.ActionLogs.Find(id);
            if (actionLog == null)
            {
                return HttpNotFound();
            }
            return View(actionLog);
        }

        // POST: ActionLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActionLog actionLog = db.ActionLogs.Find(id);
            db.ActionLogs.Remove(actionLog);
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
