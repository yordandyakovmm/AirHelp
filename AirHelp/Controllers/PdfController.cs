using Facebook;
using Newtonsoft.Json.Linq;
using AirHelp.DAL;
using AirHelp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Device.Location;

namespace AirHelp.Controllers
{


    public class PdfController : BaseController
    {

        [Route("attorney/{id}")]
        public ActionResult AttorneyHtml(Guid id)
        {
            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("Attorney", model);
        }

        [Route("attorneyPdf/{id}")]
        public ActionResult AttorneyHtml2(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();
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

            converter.Options.MarginTop = 18;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
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