using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;

namespace Delivery.Controllers.DeliveryControllers
{
    public class ShishasController : Controller
    {
        private ShishaDataContext db = new ShishaDataContext();

        // GET: Shishas
        public ActionResult Index()
        {
            return View(db.Shishas.ToList());
        }

        // GET: Shishas/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shisha shisha = db.Shishas.Find(id);
            if (shisha == null)
            {
                return HttpNotFound();
            }
            return View(shisha);
        }

        // GET: Shishas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Shishas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShishaId,Name")] Shisha shisha)
        {
            if (ModelState.IsValid)
            {
                db.Shishas.Add(shisha);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shisha);
        }

        // GET: Shishas/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shisha shisha = db.Shishas.Find(id);
            if (shisha == null)
            {
                return HttpNotFound();
            }
            return View(shisha);
        }

        // POST: Shishas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShishaId,Name")] Shisha shisha)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shisha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shisha);
        }

        // GET: Shishas/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shisha shisha = db.Shishas.Find(id);
            if (shisha == null)
            {
                return HttpNotFound();
            }
            return View(shisha);
        }

        // POST: Shishas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Shisha shisha = db.Shishas.Find(id);
            db.Shishas.Remove(shisha);
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
