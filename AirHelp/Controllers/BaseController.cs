using Facebook;
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
        
    public class LoginController : Controller
    {
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

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove("user");
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