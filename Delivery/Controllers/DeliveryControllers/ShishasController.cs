using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;
using Delivery.Data.Service.Enums;
using Delivery.Data.Service.FileUploader;

namespace Delivery.Controllers.DeliveryControllers
{
    public class ShishasController : Controller
    {
        private readonly ShishaDataContext _db = new ShishaDataContext();
        private readonly StockLogDataContext _dbc = new StockLogDataContext();

        // GET: Shishas
        public ActionResult Index()
        {
            return View(_db.Shishas.ToList());
        }
        // GET: StockLogs
        public ActionResult StockLogs(long? id)
        {
            return View(_dbc.StockLogs.Where(n => n.ShishaId == id).OrderByDescending(n => n.ActionDate));
        }
        [HttpGet]
        public ActionResult StockManager(long id, string add, string remove)
        {
            var shisha = _db.Shishas.Find(id);
            if (add != null)
                ViewBag.Action = add;

            if (remove != null)
                ViewBag.Action = remove;
            return View(shisha);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockManager([Bind(Include = "ShishaId,Name,AvailableQuantity,SafetyStock")] Shisha shisha,
            FormCollection collectedValues)
        {
            var quantityAdded = Convert.ToInt32(collectedValues["AddValue"]);
            var quantityRemoved = Convert.ToInt32(collectedValues["RemoveValue"]);
            var image = Request.Files["image"];
            StockLog stockLog = new StockLog();
            //stock log 
            stockLog.ActionDate = DateTime.Now;
            stockLog.ShishaId = Convert.ToInt64(collectedValues["ShishaId"]);
            stockLog.ItemName = shisha.Name;
            stockLog.ItemCategory = StockItem.Shisha.ToString();
            stockLog.FlavourId = null;
            if (quantityRemoved < shisha.AvailableQuantity)
            {
                shisha.Image = (image != null) && (image.FileName != "")
                    ? new FileUploader().UploadFile(image, UploadType.Shisha)
                    : null;
                var action = collectedValues["action"];
                var amount = 0;
             
                if (action == StockAction.Add.ToString())
                {
                    shisha.AvailableQuantity = shisha.AvailableQuantity + quantityAdded;
                    amount = quantityAdded;
                    stockLog.Action = StockLogAction.Added.ToString();
                    stockLog.Amount = quantityAdded;

                }
                if (action == StockAction.Remove.ToString())
                {
                    shisha.AvailableQuantity = shisha.AvailableQuantity - quantityRemoved;
                    amount = quantityRemoved;
                    stockLog.Action = StockLogAction.Removed.ToString();
                    stockLog.Amount = quantityRemoved;
                }
                //database entry for shisha
                _db.Entry(shisha).State = EntityState.Modified;
                _db.SaveChanges();

                //database entry for stock log
                _dbc.StockLogs.Add(stockLog);
                _dbc.SaveChanges();
                TempData["shisha"] = "You have suucessfully " + action + "ed " + amount + " stock item(s)" +
                                     " successfully";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            TempData["shisha"] = "This item is out of stock for the requested quantity!";
            TempData["notificationtype"] = NotificationType.Danger.ToString();

            return RedirectToAction("Index");
        }

        // GET: Shishas/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var shisha = _db.Shishas.Find(id);
            if (shisha == null)
                return HttpNotFound();
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
        public ActionResult Create([Bind(Include = "ShishaId,UnitAmount,Name,AvailableQuantity,SafetyStock")] Shisha shisha)
        {
            if (ModelState.IsValid)
            {
                _db.Shishas.Add(shisha);
                _db.SaveChanges();
                TempData["shisha"] = "You have added a new item successfully!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(shisha);
        }

        // GET: Shishas/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var shisha = _db.Shishas.Find(id);
            if (shisha == null)
                return HttpNotFound();
            return View(shisha);
        }

        // POST: Shishas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShishaId,Name,UnitAmount,AvailableQuantity,SafetyStock")] Shisha shisha)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(shisha).State = EntityState.Modified;
                _db.SaveChanges();
                TempData["shisha"] = "You have modified an item successfully!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(shisha);
        }

        // GET: Shishas/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var shisha = _db.Shishas.Find(id);
            if (shisha == null)
                return HttpNotFound();
            return View(shisha);
        }

        // POST: Shishas/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var shisha = _db.Shishas.Find(id);
            _db.Shishas.Remove(shisha);
            _db.SaveChanges();
            TempData["shisha"] = "You have deleted an item successfully!";
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