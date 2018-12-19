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
	public class ProductiteamsController : Controller
	{

		private OnlineOrderEntities db = new OnlineOrderEntities();

		// GET: Productiteams
		public ActionResult ProductList(int supplier)
		{
			return View(db.Productiteams.ToList());

		}


		public ActionResult Add(int productId, int supplierId)
		{
			Cart cart = Session["Cart"] as Cart;
			if (cart == null || Session["Cart"] == null)
			{
				cart = new Cart();
				Session["Cart"] = cart;
				Session["CartSupplierId"] = supplierId;
			}

			var itemproduct = db.Productiteams.Where(p => p.Product_ID == productId).First();
			cart.Add(itemproduct);

			return RedirectToAction("ShowCart", "Productiteams");
		}


		public ActionResult ShowCart()
		{

			if (Session["Cart"] == null)
				return RedirectToAction("ProductList", "Productiteams");
			Cart cart = Session["Cart"] as Cart;
			ViewBag.nBonus = Session["TBonus"];
			//ViewBag.mBonus = Session["FBonus"];
			return View(cart);
		}

		public ActionResult Remove(int id)
		{
			
			Cart cart = Session["Cart"] as Cart;
			cart.Remove(id);
			return RedirectToAction("ShowCart", "Productiteams");
		}

		public ActionResult Update(FormCollection form)
		{
			var tBonus = 0;
			bool isBonus = false;

			var ABonus = 0;

			Cart cart = Session["Cart"] as Cart;
			int idpr = int.Parse(form["Product_ID"]);
			int sln = int.Parse(form["Product_Quantity"]);
			//int bns = int.Parse(form["aBonus"]);

			
			cart.Update(idpr, sln );
			double total = 0;
			foreach (var item in cart.Items)
			{
				total = total + Convert.ToDouble(item.PQuantity * item.Productiteams.Product_Price.Value);

				if (Session["CustomerId"] != null)
				{
					int id = Convert.ToInt32(Session["CustomerId"].ToString());
					int bonus = 0;
					var listofbonus = db.tblCoins.Where(x => x.CustomerID == id).Select(x => x.CoinAmount);
					foreach (var item2 in listofbonus)
					{
						bonus = bonus + (Convert.ToInt32(item2));
						ABonus = bonus;
						//isBonus = true;
					}
					//TempData["aBonus"] = "bonus";

					if (total >= bonus)
					{
						if (bonus >= 1000)
						{
							var aBonus = Convert.ToInt32((total - bonus).ToString());
							tBonus = aBonus;
							isBonus = true;
						}

					}
				}
			}
			Session["TBonus"] = tBonus;
			ViewBag.TBonus = tBonus;


			//Session["FBonus"] = ABonus;
			//ViewBag.FBonus = ABonus;

			ViewBag.isBonus = isBonus;
			return RedirectToAction("ShowCart", "Productiteams", ViewBag.TBonus);
		}



		public ActionResult Checkout()
		{
			return View("Checkout");
		}

		public ActionResult saveorder(FormCollection fc)
		{
			//var tBonus = 0;
			//bool isBonus = false;
			Cart cart = Session["Cart"] as Cart;

			//shopkepper Address
		
			Shopkeeper shopkeeper = new Shopkeeper();

			shopkeeper.Shopkeeper_Name = fc["Shopkeeper_Name"];
			shopkeeper.Shopkeeper_Phone = Convert.ToInt32(fc["Shopkeeper_Phone"].ToString());
			db.Shopkeepers.Add(shopkeeper);
			//db.SaveChanges();

			//Payment

			Payment payment = new Payment();

			payment.Payment_Type = fc["Payment_Type"];
			payment.Payment_Phone = int.Parse(fc["Payment_Phone"]);
			payment.Payment_Code = int.Parse(fc["Payment_Code"]);
			payment.Payment_Amount = int.Parse(fc["Payment_Amount"]);
			db.Payments.Add(payment);
			//db.SaveChanges();

			//order main

			OrderMain orderMain = new OrderMain();

			orderMain.Shopkeeper_ID = shopkeeper.Shopkeeper_ID;
			orderMain.Payment_ID = payment.Payment_ID;
			//supplier id ---
			orderMain.Supplier_ID = int.Parse(Session["CartSupplierId"].ToString());
			orderMain.Order_Date = DateTime.Now.ToString();
			orderMain.Delivery_Date = fc["Delivery_Date"];
			orderMain.Shopkeeper_Location = fc["Shopkeeper_Location"];
			orderMain.Shopkeeper_Area = fc["Shopkeeper_Area"];
			db.OrderMains.Add(orderMain);
			//db.SaveChanges();

			//double sumTotal = Convert.ToDouble(cart.Items.Select(x => x.Productiteams.Product_Price).Sum());
			double total = 0;
			foreach (var item in cart.Items)
			{
				OrderSub orderSub = new OrderSub();
				total = total + Convert.ToDouble(item.PQuantity * item.Productiteams.Product_Price.Value);

				orderSub.OrderMain_ID = orderMain.OrderMain_ID;
				orderSub.Product_ID = item.Productiteams.Product_ID;
				orderSub.Quantity = item.PQuantity;
				orderSub.Total_price = Convert.ToInt32(item.PQuantity * item.Productiteams.Product_Price.Value);
				db.OrderSubs.Add(orderSub);

				if (Session["CustomerId"] != null)
				{
					int id = Convert.ToInt32(Session["CustomerId"].ToString());
					int bonus = 0;
					var listofbonus = db.tblCoins.Where(x => x.CustomerID == id).Select(x => x.CoinAmount);
					foreach (var item2 in listofbonus)
					{
						bonus = bonus + (Convert.ToInt32(item2));
					}
					//TempData["aBonus"] = "bonus";

					if (total >=bonus)
					{
						if (bonus >= 1000)
						{
							var aBonus = Convert.ToInt32((total - bonus).ToString());
							orderSub.Total_price = aBonus;
							var bonusToUpdate = db.tblCoins.Where(x => x.CustomerID == id).Select(x => x);
							foreach (var item3 in bonusToUpdate)
							{
								item3.CoinAmount = (Convert.ToInt32(item3.CoinAmount) - bonus).ToString();
							}

						
							//ViewBag.TBonus = aBonus;
						}
					}

				}
			}
			

			if (Session["CustomerId"] != null)
			{

				tblCoin coin = new tblCoin();
				coin.CustomerID = int.Parse(Session["CustomerId"].ToString());
				coin.OrderMain_ID = orderMain.OrderMain_ID;
				double Bonus = Math.Floor(total / 1000) * 10;
				coin.CoinAmount = Convert.ToDouble(Bonus).ToString();
				db.tblCoins.Add(coin);
			}
			//ViewBag.TBonus = tBonus;
			//ViewBag.isBonus = isBonus;
			db.SaveChanges();
			cart.Clear();
			
			return View();
		}

		public ActionResult Dashboard()
		{
			if (Session["CustomerId"] != null)
			{
				int id = Convert.ToInt32(Session["CustomerId"].ToString());
				int bonus = 0;
				var listofbonus = db.tblCoins.Where(x => x.CustomerID == id).Select(x => x.CoinAmount);
				foreach (var item2 in listofbonus)
				{
					bonus = bonus + (Convert.ToInt32(item2));
				}

				ViewBag.mBonus = bonus;
			}
		return View("Dashboard", ViewBag.mBonus);
		}
	}

}
// <add name="OnlineOrder" connectionString="metadata=res://*/Models.ProductData.csdl|res://*/Models.ProductData.ssdl|res://*/Models.ProductData.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=KASHEM\SQLEXPRESS;initial catalog=OnlineOrder;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
//  <add name = "OnlineOrderData" connectionString="metadata=res://*/Models.ModelProduct.csdl|res://*/Models.ModelProduct.ssdl|res://*/Models.ModelProduct.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DESKTOP-2678HI6\SQLEXPRESS;initial catalog=OnlineOrder;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />