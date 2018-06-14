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
using System.Net.Mail;

namespace AirHelp.Controllers
{


    public class LoginController : BaseController
    {
        [HttpGet]
        [Route("вход")]
        [Route("вход/{claimId}")]
        public ActionResult Login(string claimId)
        {
            if (!string.IsNullOrEmpty(claimId) && User.Identity.IsAuthenticated)
            {
                using (AirHelpDBContext dc = new AirHelpDBContext())
                {
                    var giid = Guid.Parse(claimId);
                    var claim = dc.Claims.Where(c => c.ClaimId == giid).SingleOrDefault();

                    if (claim != null)
                    {
                       
                        claim.UserId = User.Identity.Name;
                        dc.SaveChanges();
                    }
                }
                return Redirect($"/потвърждение-на-иск/{claimId}");
            }
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
                    user.password = GetHash(Password);
                    dc.SaveChanges();
                    ViewBag.text = "Паролата Ви е сменена успешно";
                    return View("Success");
                }

            }
            return View("Success");
        }

        [Authorize]
        [HttpGet]
        [Route("потребител-редакция")]
        public ActionResult ChangeUserData()
        {
            ViewBag.error = false;
            User user = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                user = dc.Users.Where(u => u.Email == User.Identity.Name).SingleOrDefault();
                return View("ChangeUser", user);
            }

            return View("ChangeUser", null);

        }

        [Authorize]
        [HttpGet]
        [Route("gdpr")]
        [Route("gdpr/{userid}")]
        public ActionResult GdprData(string userid)
        {
            var userEmail = "";
            if (User.IsInRole("admin") && string.IsNullOrEmpty(userid))
            {
                userEmail = userid.Replace("34de6r", "@").Replace("43edW", ".");
            }
            else
            {
                userEmail = User.Identity.Name;
            }

            ViewBag.error = false;
            User user = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                user = dc.Users
                    .Include("claims")
                    .Where(u => u.Email == userEmail)
                    .SingleOrDefault();
                return View("GDPR", user);
            }

            return View("GDPR", null);

        }


        [Authorize]
        [HttpPost]
        [Route("потребител-редакция")]
        public ActionResult ChangeUserdataPost()
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                var userId = Request.Form["userId"];
                var FirstName = Request.Form["FirstName"];
                var LastName = Request.Form["LastName"];
                var City = Request.Form["City"];
                var Country = Request.Form["Country"];
                var Adress = Request.Form["Adress"];
                var Tel = Request.Form["Tel"];
                var Password = Request.Form["Password"];


                var user = dc.Users.Where(c => c.UserId == userId).SingleOrDefault();
                user.FirstName = FirstName;
                user.LastName = LastName;
                user.Country = Country;
                user.City = City;
                user.Adress = Adress;
                user.Tel = Tel;
                user.FirstName = FirstName;
                
                dc.SaveChanges();

                Session["user"] = user;

                FormsAuthenticationTicket authTicket =
               new FormsAuthenticationTicket(1, user.UserId, DateTime.Now, DateTime.Now.AddMinutes(200), true, user.Role, "/");
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                                   FormsAuthentication.Encrypt(authTicket));

                return View("ChangeUser", user);

            }

            return View("ChangeUser", null);

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


        [HttpGet]
        [Route("забравена-парола")]
        public ActionResult ForgottenPassword()
        {
            return View("ForgottenPassword");

        }

        [HttpPost]
        [Route("забравена-парола")]
        public ActionResult ForgottenPasswordPost()
        {
            var Email = Request.Form["Email"];

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                
                var user = dc.Users.Where(u => u.Email == Email ).SingleOrDefault();

                if (user == null)
                {
                    ViewBag.error = " Няма потребител с такъв email ";
                    return View("ForgottenPassword");
                }
                else {
                    DateTime limit = DateTime.Now.AddMinutes(30);
                    Guid guid1 = Guid.NewGuid();
                    Guid guid2 = Guid.NewGuid();
                    var securityKey = GetHash(guid1.ToString()) + GetHash(guid2.ToString());
                    securityKey = securityKey
                        .Replace("=", "")
                        .Replace("/", "")
                        .Replace("\\", "")
                        .Replace("+", "");
                    var url = $"helpclaim.eu/забравена-парола/{securityKey}";
                    user.changePasswordCode = securityKey;
                    user.changePasswordCodeValudation = limit;
                    dc.SaveChanges();

                    string bosy = $"<h1>Възстановяване на забравена парола </h1>"+ 
                        $"<p>Възстановяване на забравена парола за потребител на helpclaim.eu. потребител имейл: {user.Email}</p>" +
                        $"<p> Моля последваите лика за избор на нова парола: <a href='{url}' target='_blank'> генериране на парола </a></p>" +
                        $"<p><b>{ConfigurationManager.AppSettings["company"]} 2018</b></p>";



                    MailMessage message = new MailMessage();
                    message.To.Add(new MailAddress(user.Email));
                    message.Subject = "Забравена парола";
                    message.Body = bosy;
                    message.IsBodyHtml = true;

                    try
                    {
                        using (var smtp = new SmtpClient())
                        {
                            smtp.Port = 25;
                            smtp.EnableSsl = false;
                            smtp.Credentials = new System.Net.NetworkCredential("postmaster@helpclaim.eu", "K4hvd2357@");
                            smtp.Host = "mail.helpclaim.eu";
                            smtp.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.text = "Грешка при изпращането на имейл за възстановяване на парола";
                        return View("Success");

                    }

                    ViewBag.text = "Изпратен емейл";
                    ViewBag.text = "Изпратен Ви е имейл с линк за избор на нова парола. Валидността на линка е 30 мин.";
                    return View("Success");

                }
                
            }

            return View("ForgottenPassword");

        }


        [HttpGet]
        [Route("забравена-парола/{code}")]
        public ActionResult ForgottenPasswordP(string code)
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {

                var user = dc.Users.Where(u => u.changePasswordCode == code).SingleOrDefault();

                if (user == null) {
                    ViewBag.text = "Невалидан линк";
                    return View("Success");
                }

                if (user != null && DateTime.Now > user.changePasswordCodeValudation)
                {
                    ViewBag.text = "Линка е с изтекла валидност";
                    return View("Success");
                }

                return View("ForgottenPasswordP");

            }

            return View("ForgottenPasswordP");
        }

        [HttpPost]
        [Route("забравена-парола/{code}")]
        public ActionResult ForgottenPasswordPP(string code)
        {
            var Password = Request.Form["Password"];
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {

                var user = dc.Users.Where(u => code != null && u.changePasswordCode == code).SingleOrDefault();

                if (user == null)
                {
                    ViewBag.text = "Невалидан линк";
                    return View("Success");
                }

                if (user != null && DateTime.Now > user.changePasswordCodeValudation)
                {
                    ViewBag.text = "Линка е с изтекла валидност";
                    return View("Success");
                }

                Password = GetHash(Password);
                user.password = Password;
                user.changePasswordCode = null;
                dc.SaveChanges();

                ViewBag.text = "Паролата е зададена успешно";
                return View("Success");

            }

            return View("ForgottenPasswordP");
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