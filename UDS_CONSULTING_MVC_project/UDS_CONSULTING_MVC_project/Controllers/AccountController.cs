using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security;
using UDS_CONSULTING_MVC_project.Models;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Security;


namespace UDS_CONSULTING_MVC_project.Controllers
{
    public class AccountController : Controller
    {
        User user = null;

        List<User> usersArr;
        // GET: Account
        public ActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginModel model)
        {
            DeserializeJsonAccount(model);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(model.Login, true);
                ViewBag.LoggedUser = usersArr.Find(u => u.login == model.Login).name;
                return RedirectToAction("GetData", "Home");
            }
            else
            {
                ModelState.AddModelError("", "User with such login and password not found");
            }
            return View();
        }

        private void DeserializeJsonAccount(LoginModel model)
        {
            using (FileStream fs = new FileStream(Server.MapPath("~/App_Data/accounts.json"), FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<User>));

                usersArr = (List<User>)jsonFormatter.ReadObject(fs);

                foreach (User u in usersArr)
                {
                    if (model.Login == u.login && model.Password == u.password)
                    {
                        user = u;
                        model.Name = user.name;
                        break;
                    }
                }
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            if(User.Identity.IsAuthenticated)
            FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}