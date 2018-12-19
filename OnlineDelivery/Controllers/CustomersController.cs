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
    public class CustomersController : Controller
    {
        private OnlineOrderEntities db = new OnlineOrderEntities();
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(Customer user)
		{
			using (OnlineOrderEntities db = new OnlineOrderEntities())
			{
				var Details = db.Customers.SingleOrDefault(x => x.CustomerEmail == user.CustomerEmail && x.CustomerPassword == user.CustomerPassword);
				if (Details != null)
				{
					Session["CustomerID"] = Details.CustomerID.ToString();
					Session["CustomerName"] = Details.CustomerName.ToString();
					return RedirectToAction("SuppliersList", "Suppliers");
				}
				else
				{
					ModelState.AddModelError("", "User Name or Password is invalid .");
				}
			}
			return View();
		}

		public ActionResult Dasboard()
		{
			if (Session["supplierId"] != null)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Login");
			}
		}


		public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "CustomerID,CustomerName,CustomerEmail,CustomerAddress,CustomerPassword,CustomerGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(customer);
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
