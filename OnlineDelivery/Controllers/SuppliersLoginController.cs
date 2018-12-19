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
    public class SuppliersLoginController : Controller
    {
        private OnlineOrderEntities db = new OnlineOrderEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Supplier user)
        {
            using (OnlineOrderEntities db = new OnlineOrderEntities())
            {
                var Details = db.Suppliers.SingleOrDefault(x => x.Supplier_Name == user.Supplier_Name && x.Password == user.Password);
                if (Details != null)
                {
                    Session["supplierId"] = Details.Supplier_ID.ToString();
                    Session["supliername"] = Details.Supplier_Name.ToString();
                    return RedirectToAction("Dasboard");
                }
                else
                {
                    ModelState.AddModelError( "", "User Name or Password is invalid .");
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

        public ActionResult OrderShow(OrderMain order)
        {
            if (Session["supplierId"] != null)
            {
                var supplierId = int.Parse(Session["supplierId"].ToString());
                List<OrderMain> details = db.OrderMains.Where(o => o.Supplier_ID == supplierId).ToList();
                     details.Select(s => new OrderMain
                     {
                         Shopkeeper_ID = s.Shopkeeper_ID,
                         Payment_ID = s.Payment_ID,
                         Order_Date = s.Order_Date,
                         Delivery_Date = s.Delivery_Date,
                         Shopkeeper_Location = s.Shopkeeper_Location,
                         Shopkeeper_Area = s.Shopkeeper_Area
                     }).ToList();
                return View(details);
            }
            return View();
        }         
    }
}
