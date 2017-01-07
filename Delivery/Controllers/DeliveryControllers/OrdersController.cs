using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;
using Delivery.Data.Service.EmailService;
using Delivery.Data.Service.Enums;

namespace Delivery.Controllers.DeliveryControllers
{
    public class OrdersController : Controller
    {
        private readonly OrderDataContext db = new OrderDataContext();
        private readonly ShishaDataContext dbc = new ShishaDataContext();
        private readonly FlavourDataContext dbd = new FlavourDataContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Flavour).Include(o => o.Shisha);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Orders.Find(id);
            if (order == null)
                return HttpNotFound();
            return View(order);
        }

        public ActionResult ChangeStatusToProgress(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Orders.Find(id);
            order.OrderStatus = OrderStatus.InProgress.ToString();
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            TempData["order"] = "The order is now in progress!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatusToDelivered(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Orders.Find(id);
            order.OrderStatus = OrderStatus.Delivered.ToString();
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            TempData["order"] = "The order has been delivered!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }

        public ActionResult ChangeStatusToCancelled(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Orders.Find(id);
            order.OrderStatus = OrderStatus.Cancelled.ToString();
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            TempData["order"] = "Your order has been cancelled successfully!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", "Home");
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.FlavourId = new SelectList(db.Flavours, "FlavourId", "Name");
            ViewBag.ShishaId = new SelectList(db.Shishas, "ShishaId", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,Name,Email,Mobile,Location,Quantity")] Order order,
            FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var item = typeof(StockItem).GetEnumName(int.Parse(collectedValues["Item"]));
                var flavourId = Convert.ToInt64(collectedValues["FlavourId"]);
                var shishaId = Convert.ToInt64(collectedValues["ShishaId"]);
                var quantity = Convert.ToInt32(collectedValues["Quantity"]);
                if (item == StockItem.Shisha.ToString())
                {
                    order.ShishaId = shishaId;
                    order.FlavourId = null;
                    var shisha = db.Shishas.Find(shishaId);
                    if (quantity < shisha.AvailableQuantity)
                    {
                        shisha.AvailableQuantity = shisha.AvailableQuantity - quantity;
                        dbc.Entry(shisha).State = EntityState.Modified;
                        dbc.SaveChanges();
                    }
                    else
                    {
                        TempData["order"] = "This item is out of stock for the requested quantity!";
                        TempData["notificationtype"] = NotificationType.Danger.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
                if (item == StockItem.Flavour.ToString())
                {
                    order.FlavourId = flavourId;
                    order.ShishaId = null;
                    var flavour = db.Shishas.Find(flavourId);
                    if (quantity < flavour.AvailableQuantity)
                    {
                        flavour.AvailableQuantity = flavour.AvailableQuantity - quantity;
                        dbd.Entry(flavour).State = EntityState.Modified;
                        dbd.SaveChanges();
                    }
                    else
                    {
                        TempData["order"] = "This item is out of stock for the requested quantity!";
                        TempData["notificationtype"] = NotificationType.Danger.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
                order.OrderStatus = OrderStatus.New.ToString();
                order.DateOfOrder = DateTime.Now;
                var rnd = new Random();
                var randomNumber = rnd.Next(1, 9);
                order.OrderCode = "SO " + randomNumber;
                db.Orders.Add(order);
                db.SaveChanges();
                TempData["order"] = "You have successfully placed an order and it will be attented to soonest!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                new MailerDaemon().NewOrder(order);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.FlavourId = new SelectList(db.Flavours.Where(n => n.AvailableQuantity > 0), "FlavourId", "Name",
                order.FlavourId);
            ViewBag.ShishaId = new SelectList(db.Shishas.Where(n => n.AvailableQuantity > 0), "ShishaId", "Name",
                order.ShishaId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Orders.Find(id);
            if (order == null)
                return HttpNotFound();
            ViewBag.FlavourId = new SelectList(db.Flavours, "FlavourId", "Name", order.FlavourId);
            ViewBag.ShishaId = new SelectList(db.Shishas, "ShishaId", "Name", order.ShishaId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                 Include =
                     "OrderId,Location,ShishaId,FlavourId,Quantity,CreatedBy,DateCreated,DateLastModified,LastModifiedBy"
             )] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FlavourId = new SelectList(db.Flavours, "FlavourId", "Name", order.FlavourId);
            ViewBag.ShishaId = new SelectList(db.Shishas, "ShishaId", "Name", order.ShishaId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var order = db.Orders.Find(id);
            if (order == null)
                return HttpNotFound();
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}