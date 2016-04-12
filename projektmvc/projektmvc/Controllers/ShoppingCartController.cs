using projektmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projektmvc.Controllers
{
    public class ShoppingCartController : Controller
    {
		List<OrderItem> orderedItems = new List<OrderItem>();

		// GET: ShoppingCart
		public ActionResult ShoppingCart()
		{
			if (Session["Cart"] != null)
			{
				orderedItems = (List<OrderItem>)Session["Cart"];
			}

			//Lägg till logik för att räkna ut siffrorna
			object itTot = OrderItem.CalculateNumberOfItems(orderedItems);
			object prTot = OrderItem.CalculateTotal(orderedItems);

			if (itTot == null || prTot == null)
			{
				ViewBag.ItemsTotal = 0;
				ViewBag.PriceTotal = 0;
			}
			else
			{
				ViewBag.ItemsTotal = itTot;
				ViewBag.PriceTotal = prTot;
			}

            Session["InCart"] = itTot;

            return View(orderedItems);
		}

		//Add product to cart
		public ActionResult AddToCart(string productId, string productName, string productPrice)
		{
			List<ProductModel> products = (List<ProductModel>)Session["Products"];

			if (Session["Cart"] != null)
			{
				orderedItems = (List<OrderItem>)Session["Cart"];
			}

			int id = int.Parse(productId);
			int price = int.Parse(productPrice);
			int NumInStock = 0;
			bool inCart = false;

			//Kolla om varan finns i lager
			foreach (ProductModel m in products)
			{
				if (m.Id == id)
				{
					//Kolla i produktlistan hur många som finns i lager, spara i variabel
					NumInStock = (m.NumberInStock > 0) ? m.NumberInStock : 0;
					break;
				}
			}

			//Kolla om en produkt med samma id redan finns i ordern
			foreach (OrderItem item in orderedItems)
			{
				if (item.Id == id)
				{
					inCart = true;
					//TEST
					if (NumInStock >= item.Numbers + 1)//Om antalet i lager är större än eller lika med det antal man har +1
					{
						OrderItem.EditNumberOfItems(item, item.Numbers + 1);
					}
					Session["Cart"] = orderedItems;
				}

				//break;
			}

			if (!inCart)
			{
				if (NumInStock >= 1)//Om antalet i lager är större än eller lika med 1
				{
					orderedItems.Add(new OrderItem(id, productName, price, 1));
				}
				Session["Cart"] = orderedItems;
			}

			orderedItems = (List<OrderItem>)Session["Cart"];

			//Lägg till logik för att räkna ut siffrorna
			object itTot = OrderItem.CalculateNumberOfItems(orderedItems);
			object prTot = OrderItem.CalculateTotal(orderedItems);

			if (itTot == null || prTot == null)
			{
				ViewBag.ItemsTotal = 0;
				ViewBag.PriceTotal = 0;
			}
			else
			{
				ViewBag.ItemsTotal = (int)itTot;
				ViewBag.PriceTotal = (int)prTot;
			}

            Session["InCart"] = itTot;

            return View("../Product/Products", products);

			
		}

		//Actionen som tar bort objekt ur ordern
		public ActionResult Remove(string id)
		{
			List<OrderItem> orderedItems = (List<OrderItem>)Session["Cart"];
			int prodId;
			int.TryParse(id, out prodId);

			foreach (OrderItem item in orderedItems)
			{
				if (item.Id == prodId)
				{
					orderedItems.Remove(item);
					break; //avbryt när den har gjort det en gång
				}
			}

			Session["Cart"] = orderedItems;

			//Lägg till logik för att räkna ut siffrorna
			object itTot = OrderItem.CalculateNumberOfItems(orderedItems);
			object prTot = OrderItem.CalculateTotal(orderedItems);

			if (itTot == null || prTot == null)
			{
				ViewBag.ItemsTotal = 0;
				ViewBag.PriceTotal = 0;
			}
			else
			{
				ViewBag.ItemsTotal = (int)itTot;
				ViewBag.PriceTotal = (int)prTot;
			}

            Session["InCart"] = itTot;

            return View("ShoppingCart", orderedItems); //Skickar tillbaka vyn Products med modellen products

		}

		public ActionResult Edit()
		{
			List<OrderItem> orderedItems = (List<OrderItem>)Session["Cart"];
			List<ProductModel> products = (List<ProductModel>)Session["Products"];

			string num = Request.Form["amount"];
			string id = Request.Form["pId"];

			int pId = int.Parse(id);
			int amount = int.Parse(num);

			int NumInStock = 0;

			//Kolla om varan finns i lager
			foreach (ProductModel m in products)
			{
				if (m.Id == pId)
				{
					//Kolla i produktlistan hur många som finns i lager, spara i variabel
					NumInStock = (m.NumberInStock > 0) ? m.NumberInStock : 0;
					break;
				}
			}

			//Ändra antal
			foreach (OrderItem item in orderedItems)
			{
				if (pId == item.Id)
				{
					if (NumInStock >= amount)
					{
						OrderItem.EditNumberOfItems(item, amount);
						break;
					}
					else
					{
                        // Om det inte finns så många som man vill lägga till
                        //lägg till så många som faktiskt finns
                        OrderItem.EditNumberOfItems(item, NumInStock);
                        //Gör något för att visa att det inte gick
                    }
				}
			}

			Session["Cart"] = orderedItems;

			//Logik för att räkna ut siffrorna
			object itTot = OrderItem.CalculateNumberOfItems(orderedItems);
			object prTot = OrderItem.CalculateTotal(orderedItems);

			if (itTot == null || prTot == null)
			{
				ViewBag.ItemsTotal = 0;
				ViewBag.PriceTotal = 0;
			}
			else
			{
				ViewBag.ItemsTotal = (int)itTot;
				ViewBag.PriceTotal = (int)prTot;
			}

            Session["InCart"] = itTot;

            return View("ShoppingCart", orderedItems);
		}

		public ActionResult Send()
		{
			Session["Cart"] = null;
            Session["InCart"] = null;

			return View();
		}
	}
}

