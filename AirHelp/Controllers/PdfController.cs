using AirHelp.DAL;
using AirHelp.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AirHelp.Controllers
{


    public class PdfController : BaseController
    {

       
        [Route("attorneyPdf/{id}")]
        public ActionResult AttorneyHtml(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Include("User").Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("AttorneyPdf", model);

        }

        [Route("пълномощно/{id}")]
        public ActionResult AttorneyPdf(Guid id)
        {
            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/attorneyPdf/{id}";
           
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

        
        [Route("contractPdf/{id}")]
        public ActionResult ContractHtml(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Include("User").Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("ContractPdf", model);

        }

        [Route("договор/{id}")]
        public ActionResult ContractPdf(Guid id)
        {
            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/contractPdf/{id}";

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