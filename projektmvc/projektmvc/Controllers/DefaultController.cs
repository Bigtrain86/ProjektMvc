using projektmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projektmvc.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            string username = Request["username"];
            string password = Request["password"];
            string logoutButton = Request["logoutButton"];


            if (username != null)
            {
                //Försök logga in
                List<UserModel> defaultUsers = (List<UserModel>)Session["Users"];

                foreach (UserModel u in defaultUsers)
                {
                    if (username == u.Name)
                    {
                        if (password == u.Password)
                        {
                            Session["LoginStatus"] = true;
                            Session["username"] = username;
                            //return Redirect("/Secure/Index");
                        }
                        break; //Hoppa ur loopen om användarnamn är rätt men inte lösenord
                    }
                }

            }
            else if (logoutButton != null)
            {
                //logga ut
                Session["LoginStatus"] = false;
                Session["username"] = null;
                Session["InCart"] = null;
            }
            else
            {
                //business as usual
            }


            return View();
        }
        //Visar create account-sidan, om man inte redan är inloggad, annars kommer man tillbaka till index
        public ActionResult CreateAccount()
        {
            string username = Request["username"];
            string password = Request["password"];

            List<UserModel> existingUsers = (List<UserModel>)Session["Users"];
            ViewBag.AccountCreated = false;
            ViewBag.FailedAttempt = false;

            if (!(bool)Session["LoginStatus"])
            {
                bool userExists = false;
                
                foreach (UserModel m in existingUsers)
                {
                    if (m.Name == username)
                    {
                        userExists = true;
                        break;
                    }
                }

                if (!userExists && username !=null && password !=null)
                {
                    existingUsers.Add(new UserModel() { Name = username, Password = password });
                    Session["Users"] = existingUsers;
                    ViewBag.AccountCreated = true;
                    //Session["LoginStatus"] = true;
                }
                //Fixa så att man får ett fel om man anger ett namn som finns
                else if (userExists)
                {
                    ViewBag.FailedAttempt = true;
                }

                return View();
            }
            else
            {
                //Session["username"] = username;
                return Redirect("/Default/Index");
            }
            
        }
		/************* SEARCH *************/
		public ActionResult Search()
		{
			string searchSt = Request["search"];
			List<ProductModel> products = (List<ProductModel>)Session["Products"];
			var results = from m in products
						  select m;

			if (!String.IsNullOrEmpty(searchSt))
			{
				results = results.Where(s => s.Name.ToUpper().Contains(searchSt.ToUpper())
				|| s.Model.ToUpper().Contains(searchSt.ToUpper()));
			}
			return View(results);
		}

        public ActionResult Home()
        {

            return View();
        }


    }
}