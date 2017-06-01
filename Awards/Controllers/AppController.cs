using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Awards.Controllers
{
    public class AppController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
