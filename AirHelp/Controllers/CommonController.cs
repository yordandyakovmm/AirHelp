using System.Web.Mvc;


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