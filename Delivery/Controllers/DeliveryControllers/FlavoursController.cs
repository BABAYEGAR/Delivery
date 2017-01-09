using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;
using Delivery.Data.Service.Enums;

namespace Delivery.Controllers.DeliveryControllers
{
    public class FlavoursController : Controller
    {
        private readonly FlavourDataContext _db = new FlavourDataContext();
        private readonly StockLogDataContext _dbc = new StockLogDataContext();

        // GET: Flavours
        public ActionResult Index()
        {
            return View(_db.Flavours.ToList());
        }
        // GET: StockLogs
        public ActionResult StockLogs(long? id)
        {
            return View(_dbc.StockLogs.Where(n=>n.FlavourId == id).OrderByDescending(n=>n.ActionDate));
        }


        [HttpGet]
        public ActionResult StockManager(long id, string add, string remove)
        {
            var flavour = _db.Flavours.Find(id);
            if (add != null)
                ViewBag.Action = add;

            if (remove != null)
                ViewBag.Action = remove;
            return View(flavour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockManager(
            [Bind(Include = "FlavourId,Name,AvailableQuantity,SafetyStock")] Flavour flavour,
            FormCollection collectedValues)
        {
            var quantityAdded = Convert.ToInt32(collectedValues["AddValue"]);
            var quantityRemoved = Convert.ToInt32(collectedValues["RemoveValue"]);
            StockLog stockLog = new StockLog();
            stockLog.ActionDate = DateTime.Now;
            stockLog.ShishaId = null;
            stockLog.ItemName = flavour.Name;
            stockLog.ItemCategory = StockItem.Flavour.ToString();
            stockLog.FlavourId = Convert.ToInt64(collectedValues["FlavourId"]);
            if (quantityRemoved < flavour.AvailableQuantity)
            {
                var action = collectedValues["action"];
                var amount = 0;
                if (action == StockAction.Add.ToString())
                {
                    flavour.AvailableQuantity = flavour.AvailableQuantity + quantityAdded;
                    amount = quantityAdded;
                    stockLog.Amount = quantityAdded;
                    stockLog.Action = StockLogAction.Added.ToString();
                }
                if (action == StockAction.Remove.ToString())
                {
                    flavour.AvailableQuantity = flavour.AvailableQuantity - quantityRemoved;
                    amount = quantityRemoved;
                    stockLog.Amount = quantityRemoved;
                    stockLog.Action = StockLogAction.Removed.ToString();
                }
                //database entry for flavour
                _db.Entry(flavour).State = EntityState.Modified;
                _db.SaveChanges();

                //database entry for stock log
                _dbc.StockLogs.Add(stockLog);
                _dbc.SaveChanges();
                TempData["flavour"] = "You have suucessfully " + action + "ed " + amount + " stock item(s)" +
                                      " successfully";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            TempData["flavour"] = "This item is out of stock for the requested quantity!";
            TempData["notificationtype"] = NotificationType.Danger.ToString();

            return RedirectToAction("Index");
        }

        // GET: Flavours/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var flavour = _db.Flavours.Find(id);
            if (flavour == null)
                return HttpNotFound();
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
        public ActionResult Create([Bind(Include = "FlavourId,Name,AvailableQuantity,SafetyStock,UnitAmount")] Flavour flavour)
        {
            if (ModelState.IsValid)
            {
                _db.Flavours.Add(flavour);
                _db.SaveChanges();
                TempData["flavour"] = "You have successfully created a new item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(flavour);
        }

        // GET: Flavours/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var flavour = _db.Flavours.Find(id);
            if (flavour == null)
                return HttpNotFound();
            return View(flavour);
        }

        // POST: Flavours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FlavourId,Name.AvailableQuantity,SafetyStock,UnitAmount")] Flavour flavour)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(flavour).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["flavour"] = "You have successfully modified an item!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(flavour);
        }

        // GET: Flavours/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var flavour = _db.Flavours.Find(id);
            if (flavour == null)
                return HttpNotFound();
            return View(flavour);
        }

        // POST: Flavours/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var flavour = _db.Flavours.Find(id);
            _db.Flavours.Remove(flavour);
            _db.SaveChanges();
            TempData["flavour"] = "You have successfully deleted an item!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}