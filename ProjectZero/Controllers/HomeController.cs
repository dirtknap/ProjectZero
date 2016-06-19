using System.Web.Mvc;
using ProjectZero.Database.Dal.Composite.Interfaces;

namespace ProjectZero.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleTeaserDal teaserDal;


        public HomeController(IArticleTeaserDal teaserDal)
        {
            this.teaserDal = teaserDal;
        }

        public ActionResult Index()
        {
            var results = teaserDal.GetAll();
                        return View(results);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project Zero.";

            return View();
        }
    }
}