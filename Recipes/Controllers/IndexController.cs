using Facebook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Recipes.Controllers
{
    

    public class IndexController : BaseController
    {

        
        public ActionResult Index(string studentId, string seond)
        {
            return View();
        }

        public ActionResult Edit(string studentId, string seond)
        {
            return View();
        }


        [Route("{category}/{item}")]
        [Route("recipes/{category}/{item}")]
        public ActionResult Spliter(string category, string item)
        {
            var contex = new DAL.AirHelpDBContext();
            var count = contex.Users.Count();
            return View("Index");


        }
    }
}