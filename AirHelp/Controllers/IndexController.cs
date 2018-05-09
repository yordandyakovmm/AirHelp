using System.Web.Mvc;

namespace AirHelp.Controllers
{


    public class IndexController : BaseController
    {
        [Authorize]
        //[Route("{item}/{category}")]
        public ActionResult Index(string category, string item)
        {
            return View("Index");
        }

    }
}