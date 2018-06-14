using AirHelp.DAL;
using AirHelp.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AirHelp.Controllers
{


    public class PdfController : BaseController
    {

       
        [Route("attorneyPdf-09-234-5644-34e5345-2343246/{id}")]
        public ActionResult AttorneyHtml(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims
                    .Include("User")
                    .Include("AirPorts")
                    .Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("AttorneyPdf", model);

        }

        [Authorize]
        [Route("пълномощно/{id}")]
        public ActionResult AttorneyPdf(Guid id)
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                var userID = User.Identity.Name;
                var isAdmin = User.IsInRole("admin");
                var claim = dc.Claims.Include("User").Where(c => c.ClaimId == id && (isAdmin || c.UserId == userID)).SingleOrDefault();
                if (claim == null)
                {
                    ViewBag.text = "защитена информация";
                    return View("Views/Login/Success.cshtml");
                }

            }

            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/attorneyPdf-09-234-5644-34e5345-2343246/{id}";
           
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;
            converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
            
            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);

            Response.ContentType = "application/pdf";
            doc.Save(Response.OutputStream);
            doc.Close();
            Response.End();
            return null;
        }

        
        [Route("contractPdf-09-4565-3453-2345435-2355/{id}")]
        public ActionResult ContractHtml(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims
                    .Include("User")
                    .Include("AirPorts")
                    .Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("ContractPdf", model);

        }

        [Authorize]
        [Route("договор/{id}")]
        public ActionResult ContractPdf(Guid id)
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                var userID = User.Identity.Name;
                var isAdmin = User.IsInRole("admin");
                var claim = dc.Claims.Include("User").Where(c => c.ClaimId == id && (isAdmin || c.UserId == userID)).SingleOrDefault();
                if (claim == null)
                {
                    ViewBag.text = "защитена информация";
                    return View("Views/Login/Success.cshtml");
                }

            }
            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/contractPdf-09-4565-3453-2345435-2355/{id}";

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            converter.Options.MarginTop = 30;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 20;
            converter.Options.MarginRight = 20;
            converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;

            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);

            Response.ContentType = "application/pdf";
            doc.Save(Response.OutputStream);
            doc.Close();
            Response.End();
            return null;
        }

    }
}