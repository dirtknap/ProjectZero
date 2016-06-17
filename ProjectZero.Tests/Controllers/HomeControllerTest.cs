using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectZero.Controllers;
using ProjectZero.Database.Dal.Composite.Interfaces;

namespace ProjectZero.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeController_Index()
        {
            var dal = new Mock<IArticleTeaserDal>();
            var controller = new HomeController(dal.Object);

            var result = controller.About() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void HomeCOntroller_About()
        {
            var dal = new Mock<IArticleTeaserDal>();
            var controller = new HomeController(dal.Object);

            var result = controller.About() as ViewResult;

            Assert.AreEqual("Project Zero.", result.ViewBag.Message);
        }

    }
}
