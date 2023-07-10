using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace testProject_automationExercise
{
    [TestFixture]
    [Ignore("this test case is complete")]

    public class TestCase14_PlaceOrderRegistrationWhileCheckout
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test, Order(1)] // launch and navigate to automation exercise homepage
        [Retry(3)]
        public void Step1()
        {
            driver.Navigate().GoToUrl("https://www.automationexercise.com");
            string expectedPage = "Automation Exercise";
            string actualPage = driver.Title;
            Assert.AreEqual(expectedPage, actualPage);
            TestContext.Progress.WriteLine(driver.Title);
        }

        [Test, Order(2)]
        public void Step2()
        {
            driver.Navigate().GoToUrl("https://www.automationexercise.com");
            TestContext.Progress.WriteLine(driver.Title);
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }
    }
}