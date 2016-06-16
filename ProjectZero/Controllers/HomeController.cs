using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ProjectZero.Database.Dal.Composite;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Models;

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