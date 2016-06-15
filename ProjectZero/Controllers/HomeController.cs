using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ProjectZero.Database.Dal.Composite;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dal.Tables.Interfaces;
using ProjectZero.Models;

namespace ProjectZero.Controllers
{
    public class HomeController : Controller
    {
        private IArticleTeaserDal teaserDalDal;


        public HomeController(IArticleTeaserDal teaserDalDal)
        {
            this.teaserDalDal = teaserDalDal;
        }

        public ActionResult Index()
        {
            var results = teaserDalDal.GetAllTeasers();

            return View(results);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project Zero.";

            return View();
        }
    }
}