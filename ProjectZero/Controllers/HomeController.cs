using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ProjectZero.Models;

namespace ProjectZero.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(GetTeasers());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project Zero.";

            return View();
        }

        private List<ArticleTeaser> GetTeasers()
        {
            return new List<ArticleTeaser>
            {
                new ArticleTeaser { Title = "Random Bullshit: Chapter 1",
                    TeaserText = "Buncha' shit about a haunted house and death on the beach. Oh yeah, and reasons not to wild shape into a moose.",
                    Published = DateTimeOffset.Now,
                    LastEdited = DateTimeOffset.Now,
                    Tags = new List<string> {"wildShape","dumbDruid","deadRogue" }
                },
                new ArticleTeaser { Title = "Random Bullshit: Chapter 2",
                    TeaserText = "Where in the party finds an abandoned temple in the swamp. *Frog noises intensify*",
                    Published = DateTimeOffset.Now,
                    LastEdited = DateTimeOffset.Now,
                    Tags = new List<string> {"dreamingOfFish","spaceCockroach" }
                },
                new ArticleTeaser { Title = "Random Bullshit: Chapter 3",
                    TeaserText = "Go kill the goblins they said, it will be easy they said.",
                    Published = DateTimeOffset.Now,
                    LastEdited = DateTimeOffset.Now,
                    Tags = new List<string> {"muhBabyTadpole","prisonSex","blueGoblin" }
                },
            };

        }
    }
}