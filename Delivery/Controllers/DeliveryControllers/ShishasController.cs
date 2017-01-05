﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;

namespace Delivery.Controllers.DeliveryControllers
{
    public class ShishasController : Controller
    {
        private ShishaDataContext _db = new ShishaDataContext();

        // GET: Shishas
        public ActionResult Index()
        {
            return View(_db.Shishas.ToList());
        }

        // GET: Shishas/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shisha shisha = _db.Shishas.Find(id);
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
                _db.Shishas.Add(shisha);
                _db.SaveChanges();
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
            Shisha shisha = _db.Shishas.Find(id);
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
                _db.Entry(shisha).State = EntityState.Modified;
                _db.SaveChanges();
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
            Shisha shisha = _db.Shishas.Find(id);
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
            Shisha shisha = _db.Shishas.Find(id);
            _db.Shishas.Remove(shisha);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
