using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace testProject_automationExercise
{
    public class TestCase1_Registration
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
        }

        public static string randomString()
        {
            Random gen = new Random();
            int randomInt = (int)gen.NextInt64(50000);
            String randomIntStr = randomInt.ToString();
            return randomIntStr;
        }
        public static DateTime randomDate()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            DateTime randomDate = DateTime.Today.AddDays(-gen.Next(range));
            return randomDate;
        }

        public bool IsElementPresent(By by)
        {
            try {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            { return false; }
        }

        public bool IsElementEnabled(IWebElement element)
        { return element.Enabled; }

        [Test, Order(1)]
        [Ignore("ignore test case 1")]
        public void testCase1()
        {
            driver.Navigate().GoToUrl("https://www.automationexercise.com"); // step 1 & 2 launch browser and navigate to url
            string actualPage = driver.Title;

            // 3. Verify that home page is visible successfully
            Assert.That(actualPage, Is.EqualTo("Automation Exercise"));
            if (IsElementPresent(By.XPath("//*[@id=\"header\"]/div/div/div")) //header
                && IsElementPresent(By.XPath("//*[@id=\"slider\"]/div/div/div")) //carousel
                && IsElementPresent(By.XPath("/html/body/section[2]/div/div")) //category & feature items
                && IsElementPresent(By.XPath("//*[@id=\"footer\"]/div[2]")) //footer
                && IsElementPresent(By.XPath("//*[@id=\"footer\"]/div[3]")) //copyright footer
                )
            { TestContext.Progress.WriteLine("Homepage is available"); }
            else
            { TestContext.Progress.WriteLine("Homepage has error. Please investigate."); }

            driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div/div/div[2]/div/ul/li[4]/a")).Click(); // 4. Click on 'Signup / Login' button

        }

        [Test, Order(2)] // login/sign-up page and registration form
        // [Ignore("ignore test case 2")]
        [Retry(3)]
        public void testCase2()
        {
            driver.Navigate().GoToUrl("https://www.automationexercise.com/login");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            Actions action = new Actions(driver);
            IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

            // 5. Verify 'New User Signup!' is visible
            if (
                IsElementPresent(By.XPath("//*[@id=\"form\"]/div/div/div[3]")) //sign up form
                && driver.PageSource.Contains("New User Signup!") //copyright footer
                )
            { TestContext.Progress.WriteLine("Signup form is available"); }
            else { TestContext.Progress.WriteLine("Signup form has error. Please investigate."); }

            driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div[3]/div/form/input[2]")).SendKeys("mtest" + randomString()); // 6. Enter name
            driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div[3]/div/form/input[3]")).SendKeys("mtest" + randomString() + "@gmail.com"); // 6. Enter email address
            driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div[3]/div/form/button")).Click(); // 7. Click 'Signup' button

            // 8. Verify that 'ENTER ACCOUNT INFORMATION' is visible
            Thread.Sleep(3000); // wait for page to load
            if (
                IsElementPresent(By.XPath("//*[@id=\"form\"]/div/div/div/div")) //registration form
                && driver.PageSource.Contains("Enter Account Information") //sub-header
                )
            { 
                TestContext.Progress.WriteLine("Registration form is available");

                WebElement elementEmail = (WebElement)driver.FindElement(By.Id("email"));
                jse.ExecuteScript("arguments[0].scrollIntoView(true);", elementEmail);
                Thread.Sleep(3000);

                driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/ins[2]/div[2]/iframe")));
                if (IsElementPresent(By.Id("cbb")))
                { 
                    driver.FindElement(By.Id("cbb")).Click();
                }
                else { TestContext.Progress.WriteLine("No ads pop-up in the footer."); }
            }
            else
            { TestContext.Progress.WriteLine("Registration form has error. Please investigate."); }

            //checking name and email fields
            Thread.Sleep(3000);
            driver.SwitchTo().DefaultContent();
            string userName = driver.FindElement(By.Id("name")).GetAttribute("value");
            string email = driver.FindElement(By.Id("email")).GetAttribute("value");
            if (driver.FindElement(By.Id("name")).GetAttribute("value") != null //if name has value
                && driver.FindElement(By.Id("email")).GetAttribute("value") != null //if email has value
                )
            {
                TestContext.Progress.WriteLine("Name: " + userName + " and email: " + email);
            }
            else
            { TestContext.Progress.WriteLine("Name and email fields have error. Please investigate."); }

            // 9. Fill details: Title, Name, Email, Password, Date of birth
            driver.FindElement(By.Id("password")).SendKeys("password" + randomString());
            driver.FindElement(By.Id("days")).SendKeys("1");
            driver.FindElement(By.Id("months")).SendKeys("1");
            driver.FindElement(By.Id("years")).SendKeys("1996");


            jse.ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.Id("optin")));
            action.MoveToElement(driver.FindElement(By.Id("newsletter")), 2, 2).Click().Perform(); // 10. Select checkbox 'Sign up for our newsletter!'
            action.MoveToElement(driver.FindElement(By.Id("optin")), 2, 2).Click().Perform(); // 11. Select checkbox 'Receive special offers from our partners!'

            // 12.  Fill details: First name, Last name, Company, Address, Address2, Country, State, City, Zipcode, Mobile Number
            driver.FindElement(By.Id("first_name")).SendKeys("FName " + randomString());
            driver.FindElement(By.Id("last_name")).SendKeys("LName " + randomString());
            driver.FindElement(By.Id("company")).SendKeys("Company" + randomString());
            driver.FindElement(By.Id("address1")).SendKeys(randomString() + " Test Street, PO Box 1" + randomString());
            driver.FindElement(By.Id("address2")).SendKeys("CA 1" + randomString());
            driver.FindElement(By.Id("country")).SendKeys("United States");
            driver.FindElement(By.Id("state")).SendKeys("California");
            driver.FindElement(By.Id("city")).SendKeys("Los Angeles");
            driver.FindElement(By.Id("zipcode")).SendKeys("1" + randomString());
            driver.FindElement(By.Id("mobile_number")).SendKeys("+1 999" + randomString());


            jse.ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div/div[1]/form/button")));
            driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div/div[1]/form/button")).Click();// 13. Click 'Create Account button'

            
            if (driver.PageSource.Contains("Account Created!") // 14. Verify that 'ACCOUNT CREATED!' is visible
                && driver.PageSource.Contains("Congratulations! Your new account has been successfully created!")
                && driver.PageSource.Contains("You can now take advantage of member privileges to enhance your online shopping experience with us.")
                )
            { TestContext.Progress.WriteLine("Account successfully created"); }
            else
            { TestContext.Progress.WriteLine("Registration encountered error. Please investigate."); }

            driver.FindElement(By.XPath("//*[@id=\"form\"]/div/div/div/div/a")).Click();// 15.Click 'Continue' button


            Thread.Sleep(3000);
            driver.SwitchTo().Frame("aswift_2");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//*[@id=\"dismiss-button\"]")).Click(); //Close ads pop-up after registration
            Thread.Sleep(3000);
            driver.SwitchTo().DefaultContent();
            //if (IsElementPresent(By.Id("dismiss-button")))
            //{
            //    driver.FindElement(By.Id("dismiss-button")).Click(); //Close ads pop-up after registration
            //    Thread.Sleep(3000);
            //    driver.SwitchTo().DefaultContent();
            //}
            //else { TestContext.Progress.WriteLine("No ads pop-up displayed"); }


            TestContext.Progress.WriteLine("You are now logged in as (" + userName + ")"); // 16. Verify that 'Logged in as username' is visible
            driver.FindElement(By.XPath("//*[@id=\"header\"]/div/div/div/div[2]/div/ul/li[5]/a")).Click(); // 17. Click 'Delete Account' button

            
            if (driver.PageSource.Contains("Account Deleted!") //18. Verify that 'ACCOUNT DELETED!' is visible and click 'Continue' button
                && driver.PageSource.Contains("Your account has been permanently deleted!")
                && driver.PageSource.Contains("You can create new account to take advantage of member privileges to enhance your online shopping experience with us.")
                )
            { TestContext.Progress.WriteLine("Account successfully deleted"); }
            else
            { TestContext.Progress.WriteLine("Account deletion encountered error. Please investigate."); }
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    driver.Close();
        //}
    }

}