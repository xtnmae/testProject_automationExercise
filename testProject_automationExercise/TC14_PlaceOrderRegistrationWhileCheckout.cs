using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Xml.Linq;

namespace testProject_automationExercise
{
    public class TC14_PlaceOrderRegistrationWhileCheckout
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");

            driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));

            driver.Navigate().GoToUrl("https://www.automationexercise.com/products"); // step 1 & 2 launch browser and navigate to url
        }

        static IWebElement GetRandomProductCart(IWebDriver driver)
        {
            // Find all the product cart elements
            IReadOnlyCollection<IWebElement> productCartElements = driver.FindElements(By.XPath("//div[@class='productinfo text-center']/a"));
            int numProdCart = productCartElements.Count;

            if (numProdCart > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(numProdCart);
                return productCartElements.ElementAt(randomIndex); 
            }
            else
            {
                IWebElement defaultValue = driver.FindElement(By.XPath("//div[2]/div/div/div[@class='productinfo text-center']/a"));
                return defaultValue;
            }
        }

        static IWebElement GetRandomBrand(IWebDriver driver)
        {
            // Find all the product cart elements
            IReadOnlyCollection<IWebElement> BrandElements = driver.FindElements(By.XPath("//div[@class='brands-name']/ul/li/a"));
            int numBrand = BrandElements.Count;
            Random random = new Random();
            int randomBrand = random.Next(numBrand);

            if (randomBrand > 0)
            { return BrandElements.ElementAt(randomBrand); }
            else
            {
                IWebElement defaultValue = driver.FindElement(By.XPath("//div[@class='brands-name']/ul/li[1]/a"));
                return defaultValue;
            }
        }

        static IWebElement GetRandomCategory(IWebDriver driver)
        {
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            Random random = new Random();

            // Find all the product category elements
            IReadOnlyCollection<IWebElement> ParentCat = driver.FindElements(By.XPath("//div[@class='panel-group category-products']/div/div/h4/a"));
            int rndParentCat = random.Next(ParentCat.Count);

            if (rndParentCat > 0)
            {
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", ParentCat.ElementAt(rndParentCat));
                ParentCat.ElementAt(rndParentCat).Click();
                Thread.Sleep(5000);

                // Find all the displayed child category elements
                IReadOnlyCollection<IWebElement> ChildCat = driver.FindElements(By.XPath("//div[@class='panel-group category-products']/div/div[@class='panel-collapse in']/div/ul/li/a"));
                int rndChildCat = random.Next(ChildCat.Count);

                if (rndChildCat > 0)
                {
                    jse.ExecuteScript("arguments[0].scrollIntoView(true);", ChildCat.ElementAt(rndChildCat));
                    return ChildCat.ElementAt(rndChildCat);
                }
                else
                {
                    IWebElement defaultChildCat = driver.FindElement(By.XPath("//div[@class='panel-group category-products']/div/div[@class='panel-collapse in']/div/ul/li[1]/a"));
                    return defaultChildCat;
                }
            }
            else
            {
                IWebElement defaultParentCat = driver.FindElement(By.XPath("//div[@class='panel-group category-products']/div[1]/div[1]/h4/a"));
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", defaultParentCat);
                defaultParentCat.Click();
                Thread.Sleep(5000);

                // Find all the displayed child category elements
                IReadOnlyCollection<IWebElement> ChildCat = driver.FindElements(By.XPath("//div[@class='panel-group category-products']/div/div[@class='panel-collapse in']/div/ul/li/a"));
                int rndChildCat = random.Next(ChildCat.Count);

                if (rndChildCat > 0)
                {
                    jse.ExecuteScript("arguments[0].scrollIntoView(true);", ChildCat.ElementAt(rndChildCat));
                    return ChildCat.ElementAt(rndChildCat);
                }
                else
                {
                    IWebElement defaultChildCat = driver.FindElement(By.XPath("//div[@class='panel-group category-products']/div/div[@class='panel-collapse in']/div/ul/li[1]/a"));
                    return defaultChildCat;
                }
            }
        }

        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            { return false; }
        }


        //[TearDown]
        //public void TearDown()
        //{
        //    driver.Close();
        //}


        [Test, Order(1)]
        //[Ignore("ignore test case 1")]
        public void testCase1()
        {
            string actualPage = driver.Title;
            Actions action = new Actions(driver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
            Random random = new Random();

            // 3. Verify that product page is visible successfully
            Assert.That(actualPage, Is.EqualTo("Automation Exercise - All Products"));
            if (IsElementPresent(By.XPath("//*[@id=\"header\"]/div/div/div")) //header
                && IsElementPresent(By.XPath("//*[@id=\"advertisement\"]/div")) //sub-header
                && IsElementPresent(By.XPath("//div[@class='col-sm-4']")) //products
                && IsElementPresent(By.XPath("//div[@class='panel panel-default']")) //categories
                && IsElementPresent(By.XPath("//div[@class='brands-name']/ul/li")) //brands
                && IsElementPresent(By.XPath("//*[@id=\"footer\"]/div[2]")) //footer
                && IsElementPresent(By.XPath("//*[@id=\"footer\"]/div[3]")) //copyright footer
                )
            { Console.WriteLine("Homepage is available"); }
            else
            { TestContext.Progress.WriteLine("Homepage has error. Please investigate."); }

            ////4 - A Add products to cart via homepage
            Thread.Sleep(3000);
            if (IsElementPresent(By.XPath("/html/body/ins[2]/div[2]/iframe")))
            {
                driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/ins[2]/div[2]/iframe"))); //close the ad
                if (IsElementPresent(By.Id("cbb")))
                { driver.FindElement(By.Id("cbb")).Click(); }
                else { Console.WriteLine("No ads pop-up in the footer."); }
                driver.SwitchTo().DefaultContent();
            }
            else { Console.WriteLine("No ads pop-up in the footer."); }

            IWebElement Product1 = GetRandomProductCart(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", Product1);
            Thread.Sleep(5000);
            action.MoveToElement(Product1, 2, 2).Click().Perform();
            Console.WriteLine("Product 1 added via homepage: " + Product1.GetAttribute("data-product-id") + "\n--------------");


            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//*[@id=\"cartModal\"]/div/div/div[3]/button")).Click();


            //4 - B via search
            IReadOnlyCollection<IWebElement> ProdNameElements = driver.FindElements(By.XPath("//div[@class='productinfo text-center']/p"));
            List<string> ProdNameList = new List<string>();
            foreach (IWebElement ProdNameElement in ProdNameElements)
            {
                string name = ProdNameElement.Text;
                ProdNameList.Add(name);
            }
            int randomProdName = random.Next(ProdNameList.Count);
            string ProductName = ProdNameList[randomProdName];

            jse.ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.Id("search_product")));
            driver.FindElement(By.Id("search_product")).SendKeys(ProductName);
            driver.FindElement(By.Id("submit_search")).Click();
            Console.WriteLine("Product searched: " + ProductName);
            Thread.Sleep(5000);

            IWebElement Product2 = GetRandomProductCart(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", Product2);
            action.MoveToElement(Product2, 2, 2).Click().Perform();
            Console.WriteLine("Product 2 added via search: " + Product2.GetAttribute("data-product-id") + "\n--------------");
            Thread.Sleep(5000);

            driver.FindElement(By.XPath("//*[@id='cartModal']/div/div/div[3]/button")).Click();
            Thread.Sleep(5000);


            ////4 - C via brand
            IWebElement RandomBrand = GetRandomBrand(driver);
            action.MoveToElement(RandomBrand, 2, 2).Click().Perform(); // select random brand
            Console.WriteLine("Brand selected: " + RandomBrand);
            Thread.Sleep(5000);

            IWebElement Product3 = GetRandomProductCart(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", Product3);
            action.MoveToElement(Product3, 2, 2).Click().Perform();
            Console.WriteLine("Product 3 added via brand: " + Product3.GetAttribute("data-product-id") + "\n--------------");
            Thread.Sleep(5000);

            driver.FindElement(By.XPath("//*[@id='cartModal']/div/div/div[3]/button")).Click();
            Thread.Sleep(5000);


            //4 - D via category
            IWebElement ProductCategory = GetRandomCategory(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", ProductCategory);
            action.MoveToElement(ProductCategory, 2, 2).Click().Perform();
            Console.WriteLine("Product category: " + ProductCategory);
            Thread.Sleep(5000);

            IWebElement Product4 = GetRandomProductCart(driver);
            jse.ExecuteScript("arguments[0].scrollIntoView(true);", Product4);
            action.MoveToElement(Product4, 2, 2).Click().Perform();
            Console.WriteLine("Product 4 added via category: " + Product4.GetAttribute("data-product-id") + "\n--------------");
            Thread.Sleep(5000);

            driver.FindElement(By.XPath("//*[@id='cartModal']/div/div/div[3]/button")).Click();
            Thread.Sleep(5000);


        }
    }
}