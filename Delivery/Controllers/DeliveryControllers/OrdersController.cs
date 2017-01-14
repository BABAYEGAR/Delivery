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

        public ActionResult UsersOrder()
        {
            var loggedinuser = Session["shishaloggedinuser"] as AppUser;
            var orders = db.Orders.Include(o => o.Flavour).Include(o => o.Shisha);
            return View("Index", orders.Where(n => n.AppUserId == loggedinuser.AppUserId).ToList());
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
            var shisha = dbc.Shishas.Find(order.ShishaId);
            var flavour = dbd.Flavours.Find(order.FlavourId);

            //return ordered quantity to inventory
            if (shisha != null) shisha.AvailableQuantity = shisha.AvailableQuantity + order.Quantity;
            if (flavour != null) flavour.AvailableQuantity = flavour.AvailableQuantity + order.Quantity;

            if (shisha != null)
            {
                dbc.Entry(shisha).State = EntityState.Modified;
                dbc.SaveChanges();

               
            }
            if (flavour != null)
            {
                dbd.Entry(flavour).State = EntityState.Modified;
                dbd.SaveChanges();
            }

            //modify order object
            order.OrderStatus = OrderStatus.Cancelled.ToString();
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            TempData["order"] = "Your order has been cancelled successfully!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(FormCollection collectedValues)
        {
            var shisha = Session["shisha"] as Shisha;
            var flavour = Session["flavour"] as Flavour;
            var order = Session["order"] as Order;
            if (shisha != null)
            {
                dbc.Entry(shisha).State = EntityState.Modified;
                dbc.SaveChanges();
            }
            if (flavour != null)
            {
                dbd.Entry(flavour).State = EntityState.Modified;
                dbd.SaveChanges();
            }

            db.Orders.Add(order);
            db.SaveChanges();

            TempData["order"] = "You have successfully placed an order and it will be attented to soonest!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            new MailerDaemon().NewOrder(order);
            Session["shisha"] = null;
            Session["flavour"] = null;
            Session["order"] = null;
            ViewBag.FlavourId = new SelectList(db.Flavours.Where(n => n.AvailableQuantity > 0), "FlavourId", "Name",
                order?.FlavourId);
            ViewBag.ShishaId = new SelectList(db.Shishas.Where(n => n.AvailableQuantity > 0), "ShishaId", "Name",
                order?.ShishaId);

            return RedirectToAction("Index", "Home");
        }

        // GET: Orders/Create
        public ActionResult Create(bool? home)
        {
            var loggedinuser = Session["shishaloggedinuser"] as AppUser;
            ViewBag.FlavourId = new SelectList(db.Flavours, "FlavourId", "Name");
            ViewBag.ShishaId = new SelectList(db.Shishas, "ShishaId", "Name");
            if (home == true)
                if (loggedinuser == null)
                {
                    TempData["order"] = "Please login to place your order!";
                    TempData["notificationtype"] = NotificationType.Danger.ToString();
                    return RedirectToAction("Index", "Home");
                }
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,Name,Location,Quantity")] Order order,
            FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                var loggedinuser = Session["shishaloggedinuser"] as AppUser;
                //form collected values
                var item = typeof(StockItem).GetEnumName(int.Parse(collectedValues["Item"]));
                var flavourId = Convert.ToInt64(collectedValues["FlavourId"]);
                var shishaId = Convert.ToInt64(collectedValues["ShishaId"]);
                var quantity = Convert.ToInt32(collectedValues["Quantity"]);

                //shisha and flavour ids
                var shisha = dbc.Shishas.Find(shishaId);
                var flavour = dbd.Flavours.Find(flavourId);

                if (loggedinuser != null)
                {
                    if (item == StockItem.Shisha.ToString())
                    {
                        order.ShishaId = shishaId;
                        order.FlavourId = null;
                        if (quantity < shisha.AvailableQuantity)
                        {
                            shisha.AvailableQuantity = shisha.AvailableQuantity - quantity;
                            Session["shisha"] = shisha;
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
                        if (quantity < flavour.AvailableQuantity)
                        {
                            flavour.AvailableQuantity = flavour.AvailableQuantity - quantity;
                            Session["flavour"] = flavour;
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
                    var randomNumber = rnd.Next(0, 1000000).ToString("D6");
                    ;
                    order.OrderCode = "SO" + randomNumber;
                    order.Name = loggedinuser.DisplayName;
                    order.DateOrderModified = DateTime.Now;
                    order.AppUserId = loggedinuser.AppUserId;
                    order.Email = loggedinuser.Email;
                    order.Mobile = loggedinuser.Mobile;
                    if (item == StockItem.Shisha.ToString())
                        order.TotalAmount = shisha.UnitAmount*quantity;
                    if (item == StockItem.Flavour.ToString())
                        order.TotalAmount = flavour.UnitAmount*quantity;
                    Session["order"] = order;
                    return RedirectToAction("CheckOut", "Orders");
                }
                else
                {
                    TempData["order"] = "Your session has expired log in and try again!";
                    TempData["notificationtype"] = NotificationType.Success.ToString();
                }
            }

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
                     "OrderId,Location,ShishaId,FlavourId,Quantity,DateOfOrder,FlavourId,ShishaId,OrderCode,OrderStatus,AppUserId,TotalAmount"
             )] Order order)
        {
            if (ModelState.IsValid)
            {
                order.DateOrderModified = DateTime.Now;
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