using System.Web.Mvc;
using ProjectZero.Database.Dal.Composite.Interfaces;

namespace ProjectZero.Controllers
{
    public class HomeController : Controller
    {
        private IArticleTeaserDal teaserDal;


        public HomeController(IArticleTeaserDal teaserDal)
        {
            this.teaserDal = teaserDal;
        }

        public ActionResult Index()
        {
            var results = teaserDal.GetAllTeasers();
                        return View(results);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project Zero.";

            return View();
        }
    }
}