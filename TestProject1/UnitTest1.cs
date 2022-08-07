using EFCoreScaffoldexample.AdventureWorksEntities;
using System.Linq;
using System.Dynamic;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        AdventureWorksLT2019Context context;
        WebAPI.Controllers.ProductsController productsController;

        [TestInitialize]
        public void MakeItSo()
        {
            context = new AdventureWorksLT2019Context();
            productsController = new WebAPI.Controllers.ProductsController();
        }

        [TestMethod]
        public void TestNoResults()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "a non-existant description", "");
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestNoResults2()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "", "183");
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestNoResults3()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "a non-existant description", "118");
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestAll()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "", "");
            Assert.AreEqual(295, results.Count);
        }

        [TestMethod]
        public void TestCategoryFilter()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "", "18");
            Assert.AreEqual(33,results.Count);
        }

        [TestMethod]
        public void TestCategoriesFilter()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "", "18,33,11,22");
            Assert.AreEqual(38, results.Count);
        }

        [TestMethod]
        public void TestDescriptionFilter()
        {
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "winter", "");
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void TestDescriptionFilter2()
        {
            // FUN FACT! Amazon.ca lets you search for "the", but it assumes you meant "tea" and returns 79 results.
            var results = WebAPI.Models.ProductsLogic.GetProducts(context, "the", "");
            Assert.AreEqual(145, results.Count);
        }

        [TestMethod]
        public void TestSorted()
        {
            List<ExpandoObject> results = productsController.GetProducts("", "", 1, 10, true).Result.Value;
            Assert.AreEqual(10, results.Count);

            var name = results[0].Where(r => r.Key == "name").Select(r => r.Value).FirstOrDefault();
            Assert.AreEqual("All-Purpose Bike Stand", name);
        }

        [TestMethod]
        public void TestSortedDescending()
        {
            List<ExpandoObject> results = productsController.GetProducts("", "", 1, 10, false).Result.Value;
            Assert.AreEqual(10, results.Count);

            var name = results[0].Where(r => r.Key == "name").Select(r => r.Value).FirstOrDefault();
            Assert.AreEqual("Women's Tights, S", name);
        }

        [TestMethod]
        public void TestPagination()
        {
            var results = productsController.GetProducts("", "", 1, 10, true).Result.Value;
            Assert.AreEqual(10, results.Count);
        }
    }
}
