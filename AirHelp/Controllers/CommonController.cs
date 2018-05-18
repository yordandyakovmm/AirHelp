using System;
using System.Net.Mail;
using System.Web.Mvc;


namespace AirHelp.Controllers
{
    [Authorize]
    public class CommonController : BaseController
    {

        [HttpGet]
        [Route("контакти")]
        public ActionResult Contact()
        {
            return View("ContactForm");
        }

        [HttpPost]
        [Route("контакти")]
        public ActionResult ContactPost()
        {
            var email = Request.Form["Email"];
            var name = Request.Form["name"];
            var text = Request.Form["Text"];

            string bosy = $"<h1>Запитване</h1><p>Име: {name}</p><p>email: {email}</p><p>----------</p><p>{text}</p>";


            MailMessage message = new MailMessage();
            message.To.Add(new MailAddress("yordan.dyakov@mentormate.com"));
            message.Subject = "message";
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
                return View("Success");
            }

            ViewBag.text = "Запитването е изпратено успешно";
            return View("Success");
        }


        [HttpGet]
        [Route("за-нас")]
        public ActionResult ForUs()
        {
            return View("ForUs");
        }


        [Route("пpолитика-на-поверителност")]
        public ActionResult PrivatePolice(string category)
        {
            return View("PrivatePolice");
        }


        [Route("проблеми-с-полета/често-задавани-въпроси")]
        public ActionResult FAQ(string category)
        {
            return View("faq");
        }

        [Route("общи-условия")]
        public ActionResult CommonRule(string category)
        {
            return View("CommonRules");
        }


    }
}