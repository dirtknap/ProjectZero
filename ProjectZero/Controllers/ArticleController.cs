using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectZero.Database.Dal.Composite.Interfaces;
using ProjectZero.Database.Dto.Composite;
using ProjectZero.Models;

namespace ProjectZero.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleFullDal dal;

        public ArticleController(IArticleFullDal articleFullDal)
        {
            dal = articleFullDal;
        }

        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestInsert()
        {
            var test = new ArticleFullDto
            {
                Active = true,
                Author = Guid.NewGuid(),
                Id = -1,
                LastEdited = DateTimeOffset.Now,
                Published = DateTimeOffset.Now,
                Name = "Random Bullshit: Chapter 1",
                Tags = "wildShape,deadRogue,sillyDruid",
                Teaser = "Something about a hounted house and death on the beach. Oh, and reasons not to wild shape into a moose",
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse maximus congue nulla. Nam at libero nunc. " + 
                "In laoreet ac justo eu ornare. Curabitur placerat fermentum tortor.\nPellentesque congue mollis tellus nec consequat. Maec" +
                "enas interdum id erat eu ullamcorper. Aenean dignissim turpis non eros luctus, vel finibus est ullamcorper. Maecenas maximus "+
                "dolor sit amet tempor feugiat. Fusce tincidunt vitae metus ornare efficitur.\nVivamus iaculis felis ac fringilla iaculis.Aenean" +
                " augue sapien,rutrum ac magna quis, rhoncus convallis orci.In tincidunt mauris nec sapien rhoncus, ac accumsan dolor hendrerit.In eu congue mauris.Phasellus ac eros risus.Nam non interdum risus.Nulla volutpat nibh quis dolor tempus suscipit.In sit amet tellus eros.Praesent blandit et neque vel maximus."
            };

            test.Id = dal.SaveArticle(test);

            return View(test);
        }

        // GET: Article/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            var article = new ArticleFullDto();

            return View(article);
        }

        // POST: Article/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Edit/5
        public ActionResult Edit(int id = -1)
        {

            return View();
        }

        // POST: Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
