using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;
using Delivery.Data.Service.EmailService;
using Delivery.Data.Service.Enums;

namespace Delivery.Controllers.DeliveryControllers
{
    public class OrdersController : Controller
    {
        private OrderDataContext db = new OrderDataContext();
        private ShishaDataContext dbc = new ShishaDataContext();
        private FlavourDataContext dbd = new FlavourDataContext();

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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
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
        public ActionResult Create([Bind(Include = "OrderId,Name,Email,Mobile,Location,Quantity")] Order order,FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var item = typeof(StockItem).GetEnumName(int.Parse(collectedValues["Item"]));
                long flavourId = Convert.ToInt64(collectedValues["FlavourId"]);
                long shishaId = Convert.ToInt64(collectedValues["ShishaId"]);
                int quantity = Convert.ToInt32(collectedValues["Quantity"]);
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
                    }
                }
                order.OrderStatus = OrderStatus.New.ToString();
                order.DateOfOrder = DateTime.Now;
                Random rnd = new Random();
                int randomNumber = rnd.Next(1, 9);
                order.OrderStatus = "SO " + randomNumber;
                db.Orders.Add(order);
                db.SaveChanges();
                TempData["order"] = "You have successfully placed an order and it will be attented to soonest!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                //new MailerDaemon().NewOrder(order);
                return RedirectToAction("Index");
            }

            ViewBag.FlavourId = new SelectList(db.Flavours, "FlavourId", "Name", order.FlavourId);
            ViewBag.ShishaId = new SelectList(db.Shishas, "ShishaId", "Name", order.ShishaId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.FlavourId = new SelectList(db.Flavours, "FlavourId", "Name", order.FlavourId);
            ViewBag.ShishaId = new SelectList(db.Shishas, "ShishaId", "Name", order.ShishaId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,Location,ShishaId,FlavourId,Quantity,CreatedBy,DateCreated,DateLastModified,LastModifiedBy")] Order order)
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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
