using OnlineDelivery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineDelivery.Controllers
{
    public class CartItem
    {
        public Productiteam Productiteams { get; set; }
        public int PQuantity { get; set; }
    }

    public class Cart
    {

        List<CartItem> items = new List<CartItem>(); 

        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        //Method
        public void Add(Productiteam prod, int quantity = 1)
        {
            
            var item = items.Find(p => p.Productiteams.Product_ID == prod.Product_ID);
            if (item == null)
            {
                items.Add(new CartItem { Productiteams = prod, PQuantity = quantity });
            }
            else 
            {
                item.PQuantity += quantity;
            }
        }

        public void Update(int id, int quantity)
        {
            var item = items.Find(p => p.Productiteams.Product_ID == id);
            if (item != null)
                item.PQuantity = quantity;
        }

        public void Remove(int id)
        {
            items.RemoveAll(p => p.Productiteams.Product_ID == id);
        }

        public double Total_Price()
        {
            double kq = 0;
            var total = items.Sum(p => p.Productiteams.Product_Price * p.PQuantity);
            if (total != null) kq = (double)total;
            return kq;
        }


        public void Clear() 
        {
            items.Clear();
        }

        internal int Product_Quantity()
        {
            throw new NotImplementedException();
        }
    }

}