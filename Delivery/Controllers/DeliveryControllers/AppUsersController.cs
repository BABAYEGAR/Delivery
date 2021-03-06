﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using Delivery.Data.DataContext.DataContext;
using Delivery.Data.Objects.Entities;
using Delivery.Data.Service.Enums;

namespace Delivery.Controllers.DeliveryControllers
{
    public class AppUsersController : Controller
    {
        private readonly AppUserDataContext db = new AppUserDataContext();

        // GET: AppUsers
        public ActionResult Index()
        {
            return View(db.AppUsers.ToList());
        }

        // GET: AppUsers/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = db.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // GET: AppUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "AppUserId,Firstname,Middlename,Lastname,Email,Mobile")] AppUser appUser,
            FormCollection collectedValues)
        {
            var allUsers = db.AppUsers.ToList();
            var userExist = allUsers.FirstOrDefault(n => n.Email == collectedValues["Email"]);
            if (ModelState.IsValid)
            {
                appUser.Password = Membership.GeneratePassword(6, 0);
                appUser.Role = typeof(UserType).GetEnumName(int.Parse(collectedValues["Role"]));
                if (userExist != null)
                {
                    TempData["user"] = "A user with this email exist,Try a different one!";
                    TempData["notificationtype"] = NotificationType.Danger.ToString();
                    return View(appUser);
                }
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                TempData["user"] = "A new user has been successfully created!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }

            return View(appUser);
        }

        // GET: AppUsers/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = db.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "AppUserId,Firstname,Middlename,Lastname,Email,Mobile,Password")] AppUser appUser,
            FormCollection collectedValues)
        {
            if (ModelState.IsValid)
            {
                appUser.Role = typeof(UserType).GetEnumName(int.Parse(collectedValues["Role"]));
                db.Entry(appUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["user"] = "The user has been successfully Modified!";
                TempData["notificationtype"] = NotificationType.Success.ToString();
                return RedirectToAction("Index");
            }
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var appUser = db.AppUsers.Find(id);
            if (appUser == null)
                return HttpNotFound();
            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var appUser = db.AppUsers.Find(id);
            db.AppUsers.Remove(appUser);
            db.SaveChanges();
            TempData["user"] = "The user has been successfully deleted!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
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