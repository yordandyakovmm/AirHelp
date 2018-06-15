using AirHelp.DAL;
using System;
using System.Net.Mail;
using System.Web.Mvc;
using System.Linq;

namespace AirHelp.Controllers
{
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
            //message.To.Add(new MailAddress("office@helpclaim.eu"));
            message.To.Add(new MailAddress("manager@helpclaim.eu"));
            //message.To.Add(new MailAddress("consulting@helpclaim.eu"));
            //message.To.Add(new MailAddress("lawyer@helpclaim.eu"));
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

        [Authorize]
        [HttpGet]
        [Route("документи/{doc}/{ext}")]
        public ActionResult ProtectedDocuments(string doc, string ext)
        {
            var userID = User.Identity.Name;
            var docU = doc + "/" +  ext;
            var docF = doc + "." + ext;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                var isAdmin = User.IsInRole("admin");
                var filePath = "";
                var fileName = "";
                var document = dc.Documents.Where(d => d.DocumentName == docF && (d.Claim.UserId == userID || isAdmin)).SingleOrDefault();
                if (document != null)
                {
                    filePath = $"~/UserDocuments/{docF}";
                    fileName = docF;


                    var length = new System.IO.FileInfo(Server.MapPath(filePath)).Length;
                    Response.BufferOutput = false;
                    Response.AddHeader("Content-Length", length.ToString());
                    return File(Server.MapPath(filePath), System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    var url = $"/документи/{docU}";
                    var pdf = dc.Claims.Where(c => (c.UserId == userID || isAdmin) && (c.AttorneyUrl == url || c.contractUrl == url)).SingleOrDefault();
                    if (pdf != null)
                    {
                        filePath = $"~/UserDocuments/{docF}";
                        fileName = docF;

                        var length = new System.IO.FileInfo(Server.MapPath(filePath)).Length;
                        Response.BufferOutput = false;
                        Response.AddHeader("Content-Length", length.ToString());
                        return File(Server.MapPath(filePath), System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
                    }
                }

            }

            ViewBag.text = "защитена информация";
            ViewBag.subtext = "файла е защитен от неауторизиран достъп";
            return View("Success");
        }

    }
}