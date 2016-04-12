using projektmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projektmvc.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Products()
        {
            List<ProductModel> products = (List<ProductModel>)Session["Products"];
            return View(products);
        }

        public ActionResult Create()
        {
            if ((string)Session["username"] == "admin")
            {
                ViewBag.Message = "";

                string id = Request["id"];
                string name = Request["name"];
                string model = Request["model"];
                string price = Request["price"];
                string stock = Request["stock"];
                string link = Request["link"];

                List<ProductModel> products = (List<ProductModel>)Session["Products"];

                bool validId = true;

                //Kolla om id-fältet är tomt, i så fall gör inget
                if (id != null)
                {
                    int Id, Price, Stock;
                    int.TryParse(id, out Id);
                    int.TryParse(price, out Price);
                    int.TryParse(stock, out Stock);


                    foreach (ProductModel m in products)
                    {
                        if (m.Id == Id)
                        {
                            validId = false;
                            ViewBag.Message = "Id already exists.";
                            break; //Behöver inte fortsätta leta om det redan fanns
                        }

                        validId = true;
                        
                    }

                    if (validId)
                    {
                        if (link =="")
                        {
                            products.Add(new ProductModel(Id, name, model, Price, Stock));
                        }
                        else
                        {
                            products.Add(new ProductModel(Id, name, model, Price, Stock, link));
                        }

                        ViewBag.Message = "Product added successfully.";
                    }

                }

                Session["Products"] = products;

                return View();
            }
            else
            {
                return Redirect("/Product/Products");
            }
            
        }
        //Actionen som gör att man kan redigera bilar ur listan
        public ActionResult Edit(string prodId, string prodName, string prodModel, string prodPrice, string prodStock, string prodLink)
        {
            if ((string)Session["username"] == "admin")
            {
                List<ProductModel> products = (List<ProductModel>)Session["Products"];

                int Id, Price, Stock;
                int.TryParse(prodId, out Id);
                int.TryParse(prodPrice, out Price);
                int.TryParse(prodStock, out Stock);

                //För att plocka in värdena i formuläret så att man inte behöver skriva
                ViewBag.Id = Id;
                ViewBag.Name = prodName;
                ViewBag.Model = prodModel;
                ViewBag.Price = Price;
                ViewBag.Stock = Stock;
                ViewBag.Link = prodLink;

                //Hämta värdena ur formuläret
                string id = Request["id"];
                string name = Request["name"];
                string model = Request["model"];
                string price = Request["price"];
                string stock = Request["stock"];
                string link = Request["link"];

                //Konvertera de som ska vara nummer till int
                int IdForm, PriceForm, StockForm;
                int.TryParse(id, out IdForm);
                int.TryParse(price, out PriceForm);
                int.TryParse(stock, out StockForm);

                //KOD HÄR
                foreach (ProductModel m in products)
                {
                    if (m.Id == IdForm)
                    {
                        
                        ProductModel.EditProduct(m, name, model, PriceForm, StockForm, link);
                        ViewBag.Message = "Product updated successfully.";
                        break; //Sluta när den har ändrats
                    }
                }


                Session["Products"] = products;
                return View(); 
            }
            else
            {
                return Redirect("/Product/Products");
            }
                
        }

        //Actionen som tar bort bilar ur listan
        public ActionResult Delete(string productId)
        {
            List<ProductModel> products = (List<ProductModel>)Session["Products"];
            int prodId;
            int.TryParse(productId, out prodId);

            foreach (ProductModel m in products)
            {
                if (m.Id == prodId)
                {
                    products.Remove(m);
                    break; //avbryt när den har gjort det en gång
                }
            }

            Session["Products"] = products;

            return View("Products", products); //Skickar tillbaka vyn Products med modellen products

        }
    }
}