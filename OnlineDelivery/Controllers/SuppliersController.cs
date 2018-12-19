using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineDelivery.Controllers;
using OnlineDelivery.Models;

namespace OnlineDelivery.Controllers
{
    public class SuppliersController : Controller
    {
        public ActionResult Home()
        {
            return View();
        }

        private OnlineOrderEntities db = new OnlineOrderEntities();

        // GET: Suppliers
        public ActionResult SuppliersList()
        {
            return View(db.Suppliers.ToList());
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
