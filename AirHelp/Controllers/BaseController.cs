﻿using Facebook;
using AirHelp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AirHelp.Hellpers;
using System.Security.Cryptography;
using System.Text;

namespace AirHelp.Controllers
{

    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            ViewBag.siteName = ConfigurationManager.AppSettings["siteName"].ToString();
            ViewBag.company = ConfigurationManager.AppSettings["company"].ToString();
            ViewBag.companyEn = ConfigurationManager.AppSettings["companyEn"].ToString();
            ViewBag.address = ConfigurationManager.AppSettings["address"].ToString();
            ViewBag.addressEn = ConfigurationManager.AppSettings["addressEn"].ToString();

            ViewBag.UIC = ConfigurationManager.AppSettings["UIC"].ToString();
            ViewBag.email = ConfigurationManager.AppSettings["email"].ToString();
            ViewBag.tel = ConfigurationManager.AppSettings["tel"].ToString();
            ViewBag.bankAccount = ConfigurationManager.AppSettings["bankAccount"].ToString();
            ViewBag.hasBankAcount = ConfigurationManager.AppSettings["hasBankAcount"].ToString() == "true";
            ViewBag.manager = ConfigurationManager.AppSettings["manager"].ToString();
            ViewBag.managerEn = ConfigurationManager.AppSettings["managerEn"].ToString();

            //var a = this.Request.Url.ToString();


            base.Initialize(requestContext);

            if (Session != null && Session["user"] != null)
            {
                ViewBag.user = Session["user"];
            }
            else if (User.Identity.IsAuthenticated)
            {
                var user = UserHeppler.GetUserById(User.Identity.Name);
                Session["user"] = user;
                ViewBag.user = Session["user"];
            }

            

        }

        protected void LogWithUser(string user, string role, string firstName = "", string lastName = "", string email = "") {
            var VMuser = new VMUser()
            {
                UserId = user,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                PictureUrl = "",
                Role = role
            };

            Session["user"] = user;

            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, user, DateTime.Now, DateTime.Now.AddMinutes(200), true, role, "/");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                               FormsAuthentication.Encrypt(authTicket));
            Response.Cookies.Add(cookie);
           
        }
        protected string GetHash(string text)
        {
            string hsa256salt = ConfigurationManager.AppSettings["hsa256salt"].ToString();
            var hmacSHA25 = new HMACSHA256(Encoding.ASCII.GetBytes(hsa256salt));
            byte[] hash = hmacSHA25.ComputeHash(Encoding.UTF8.GetBytes(text));
            string hashPassword = Convert.ToBase64String(hash);
            return hashPassword;
        }

    }
   
}