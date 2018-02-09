using Facebook;
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
        [Route("вход")]
        public ActionResult Login(string ReturnUrl)
        {
            return View("Login");
        }

        [HttpPost]
        [Route("вход")]
        public ActionResult Login(string Email, string Password, string ReturnUrl)
        {

            string hashPassword = GetHash(Password);

            User user = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                user = dc.Users.Where(u => u.Email == Email && u.password == hashPassword).SingleOrDefault();
            }

            if (user == null)
            {
                ViewBag.error = "Грешно потребителско име или парола";
                return View("Login");
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