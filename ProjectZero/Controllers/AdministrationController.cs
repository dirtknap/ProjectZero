using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Models;
using ProjectZero.Database.Dal.Tables;
using ProjectZero.Database.Dto.Tables;

namespace ProjectZero.Controllers
{
    public class AdministrationController : Controller
    {
        private ISimpleCrudDal<ArticleDto> articlesDal;

        public AdministrationController(ISimpleCrudDal<ArticleDto> articlesDal)
        {
            this.articlesDal = articlesDal;
        }

        // GET: Administration
        public ActionResult Index()
        {

            var model = new AdminMenu {AdminSections = new List<AdminSection>()};

            model.AdminSections.Add(new AdminSection
            {
                SectionName = "User Management",
                AdminPages = new List<AdminPage>
                {
                    new AdminPage {DisplayName = "User List", ControllerAction = "UserList"},
                    new AdminPage {DisplayName = "Pending Registrations", ControllerAction = "PendingReg"},
                }

            });

            model.AdminSections.Add(new AdminSection
            {
                SectionName = "Article Managment",
                AdminPages = new List<AdminPage>
                {
                    new AdminPage {DisplayName = "Article List", ControllerAction = "ArticleList"},
                    new AdminPage {DisplayName = "Tag List", ControllerAction = "TagList"}
                }
            });

            model.AdminSections.Add(new AdminSection
            {
                SectionName = "Site Management",
                AdminPages = new List<AdminPage>
                {
                    new AdminPage {DisplayName = "Site Tools", ControllerAction = ""}
                }
            });


            return View(model);
        }

        public PartialViewResult ArticleList()
        {
            var model = articlesDal.GetAll();

            return PartialView("_ArticleListPartial", model);
        }
    }
}