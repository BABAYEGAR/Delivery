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
    public class FlavoursController : Controller
    {
        private FlavourDataContext db = new FlavourDataContext();

        // GET: Flavours
        public ActionResult Index()
        {
            return View(db.Flavours.ToList());
        }

        // GET: Flavours/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flavour flavour = db.Flavours.Find(id);
            if (flavour == null)
            {
                return HttpNotFound();
            }
            return View(flavour);
        }

        // GET: Flavours/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Flavours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FlavourId,Name")] Flavour flavour)
        {
            if (ModelState.IsValid)
            {
                db.Flavours.Add(flavour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(flavour);
        }

        // GET: Flavours/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flavour flavour = db.Flavours.Find(id);
            if (flavour == null)
            {
                return HttpNotFound();
            }
            return View(flavour);
        }

        // POST: Flavours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlavourId,Name")] Flavour flavour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flavour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(flavour);
        }

        // GET: Flavours/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flavour flavour = db.Flavours.Find(id);
            if (flavour == null)
            {
                return HttpNotFound();
            }
            return View(flavour);
        }

        // POST: Flavours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Flavour flavour = db.Flavours.Find(id);
            db.Flavours.Remove(flavour);
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
