using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Web.Mvc;
using UDS_CONSULTING_MVC_project.Models;

namespace UDS_CONSULTING_MVC_project.Controllers
{
    public class HomeController : Controller
    {
        DataContractJsonSerializer jsonFormatterData = new
                DataContractJsonSerializer(typeof(List<Data>));
        DataContractJsonSerializer jsonFormatterAccounts = new
                DataContractJsonSerializer(typeof(List<User>));
        List<Data> allPeople;
        List<User> accounts;

        public ActionResult Index()
        {
            string result = "You're not authorize";
            if (User.Identity.IsAuthenticated)
            {
                GetAccount();
                result = "you are logged: " + accounts.Find(u => u.login == User.Identity.Name).name.ToString();
            }
            ViewBag.result = result;
            return View();
        }

        [Authorize]
        public ActionResult GetData()
        {
            using (FileStream fs = new FileStream(Server.MapPath("~/App_Data/data.json"), FileMode.OpenOrCreate))
            {
                allPeople = (List<Data>)jsonFormatterData.ReadObject(fs);
                GetAccount();
                ViewBag.LoggedUserName = accounts.Find(u => u.login == User.Identity.Name).name;
            }
            return View(allPeople);
        }

        [Authorize]
        public ActionResult SeeSingleData(string str)
        {
            GetData();
            Data newMen = allPeople.Find(s => s.name == str);
            GetAccount();
            ViewBag.LoggedUserName = accounts.Find(u => u.login == User.Identity.Name).name;
            ViewBag.Image = "http://dummyimage.com/150/99cccc/ffffff.gif&text=The+image!";
            ViewBag.dataTime = DateTime.Parse(newMen.registered).ToLocalTime();
            return View(newMen);
        }

        
        public ActionResult OrderByKey()
        {
            GetAccount();
            ViewBag.LoggedUserName = accounts.Find(u => u.login == User.Identity.Name).name;
            string strKey = RouteData.Values["id"].ToString();
            GetData();
            return View("GetData", allPeople.Where(p => p.tags.Contains(strKey)).ToList());
        }

        [Authorize]
        public ActionResult SeeAll()
        {
            if(allPeople == null)
                GetData();
            return View("GetData", allPeople);
        }

        private void GetAccount()
        {
            using (FileStream fstr = new FileStream(Server.MapPath("~/App_Data/accounts.json"), FileMode.OpenOrCreate))
            {
                accounts = (List<User>)jsonFormatterAccounts.ReadObject(fstr);
            }
        }
    }
}