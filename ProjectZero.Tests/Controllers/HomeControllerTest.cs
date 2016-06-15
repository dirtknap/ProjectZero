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
        public void Index()
        {
            // Arrange
            var dal = new Mock<IArticleTeaserDal>();
            var controller = new HomeController(dal.Object);

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            var dal = new Mock<IArticleTeaserDal>();
            var controller = new HomeController(dal.Object);

            // Act
            var result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Project Zero.", result.ViewBag.Message);
        }

    }
}
