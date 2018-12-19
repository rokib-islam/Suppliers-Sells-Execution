using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineDelivery.Models;


namespace OnlineDelivery.Controllers
{
    public class SupplierProductiteamsController : Controller
    {
        private OnlineOrderEntities db = new OnlineOrderEntities();

        // GET: SupplierProductiteams
        public ActionResult Index()
        {
            return View(db.Productiteams.ToList());
        }

        // GET: SupplierProductiteams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productiteam productiteam = db.Productiteams.Find(id);
            if (productiteam == null)
            {
                return HttpNotFound();
            }
            return View(productiteam);
        }

        // GET: SupplierProductiteams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupplierProductiteams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_ID,Product_Name,Product_Price,Product_Quantity")] Productiteam productiteam)
        {
            if (ModelState.IsValid)
            {
                db.Productiteams.Add(productiteam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productiteam);
        }

        // GET: SupplierProductiteams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productiteam productiteam = db.Productiteams.Find(id);
            if (productiteam == null)
            {
                return HttpNotFound();
            }
            return View(productiteam);
        }

        // POST: SupplierProductiteams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_ID,Product_Name,Product_Price,Product_Quantity")] Productiteam productiteam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productiteam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productiteam);
        }

        // GET: SupplierProductiteams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productiteam productiteam = db.Productiteams.Find(id);
            if (productiteam == null)
            {
                return HttpNotFound();
            }
            return View(productiteam);
        }

        // POST: SupplierProductiteams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Productiteam productiteam = db.Productiteams.Find(id);
            db.Productiteams.Remove(productiteam);
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
