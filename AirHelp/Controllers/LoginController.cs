﻿using Facebook;
using AirHelp.DAL;
using AirHelp.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using AirHelp.Hellpers;

namespace AirHelp.Controllers
{


    public class LoginController : BaseController
    {
        [HttpGet]
        [Route("вход/{claimId}")]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        [Route("вход")]
        public ActionResult LoginDirect()
        {
            return View("Login");
        }

        [Authorize]
        [HttpGet]
        [Route("смяна-на-парола")]
        public ActionResult ChangePassword()
        {
            ViewBag.error = false;
            return View("ChangePassword");
        }

        [Authorize]
        [HttpPost]
        [Route("смяна-на-парола")]
        public ActionResult ChangePasswordPost()
        {
            var oldPassword = Request.Form["oldPassword"];
            var Password = Request.Form["Password"];
            string hashPassword = GetHash(oldPassword);

            User user = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                user = dc.Users.Where(u => u.Email == User.Identity.Name).SingleOrDefault();


                if (user.password != hashPassword)
                {
                    ViewBag.error = true;
                    return View("ChangePassword");
                }
                else
                {
                    user.password = GetHash(oldPassword);
                    dc.SaveChanges();
                    ViewBag.text = "Паролата Ви е сменена успешно";
                    return View("Success");
                }

            }
            return View("Success");
        }

        [HttpPost]
        [Route("вход")]
        [Route("вход/{claimId}")]
        public ActionResult LoginPost(string claimId)
        {

            var ReturnUrl = Request.QueryString["ReturnUrl"];
            var Email = Request.Form["Email"];
            var Password = Request.Form["Password"];
            if (ReturnUrl == null)
            {
                ReturnUrl = "/";
            }

            string hashPassword = GetHash(Password);

            User user = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                user = dc.Users.Where(u => u.Email == Email && u.password == hashPassword).SingleOrDefault();

                if (user == null)
                {
                    ViewBag.error = "Грешно потребителско име или парола";
                    return View("Login");
                }

                if (claimId != null)
                {
                    var ClaimId = Guid.Parse(claimId);
                    var claim = dc.Claims.Where(c => c.ClaimId == ClaimId).SingleOrDefault();
                    claim.UserId = user.UserId;
                    dc.SaveChanges();
                }
            }
                      

            var VMuser = new VMUser()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PictureUrl = user.PictureUrl,
                Role = user.Role
            };

            Session["user"] = user;

            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, user.UserId, DateTime.Now, DateTime.Now.AddMinutes(200), true, user.Role, "/");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                               FormsAuthentication.Encrypt(authTicket));
            Response.Cookies.Add(cookie);

            if (claimId != null)
            {
                return Redirect($"/потвърждение-на-иск/{claimId}");
            }
            return Redirect(ReturnUrl);

        }

        [Route("изход")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove("user");
            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["appId"],
                client_secret = ConfigurationManager.AppSettings["appSecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }


        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["appId"],
                client_secret = ConfigurationManager.AppSettings["appSecret"],
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            if (accessToken == null)
            {
                return Redirect("/");
            }

            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            var user = new VMUser
            {
                UserId = me.id,
                FirstName = me.first_name,
                LastName = me.last_name,
                Email = me.email,
                PictureUrl = me.picture.data.url
            };

            user = UserHeppler.SyncUserToDatabase(user);

            Session["user"] = user;

            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, user.UserId, DateTime.Now, DateTime.Now.AddMinutes(200), true, user.Role, "/");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                               FormsAuthentication.Encrypt(authTicket));
            Response.Cookies.Add(cookie);
            return Redirect("/");

        }
               
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;

            }
        }


    }
}