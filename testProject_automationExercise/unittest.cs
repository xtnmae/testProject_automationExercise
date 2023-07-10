using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDotNetUnitTests
{
    public class MultipleUrlsTest
    {
        [Test]
        public void shouldBeAbleTocreate3Drivers()
        {
            for (int i = 0; i <= 10; i++)
            {
                WebDriver driver1 = new ChromeDriver("/Users/Puja/Downloads/webdrivers/");
                WebDriver driver2 = new ChromeDriver("/Users/Puja/Downloads/webdrivers/");
                WebDriver driver3 = new ChromeDriver("/Users/Puja/Downloads/webdrivers/");

                try
                {
                    driver1.Navigate().GoToUrl("https://www.behindthename.com/random/random.php?number=1&sets=1&gender=m&surname=&randomsurname=yes&usage_ger=1");
                }
                catch
                {
                    Assert.Fail();
                }

                Thread.Sleep(10000);

                try
                {
                    driver1.Navigate().GoToUrl("https://all-inkl.com/login/");
                }
                catch
                {
                    Assert.Fail();
                }

                Thread.Sleep(10000);

                try
                {
                    driver1.Navigate().GoToUrl("https://all-inkl.com/login/");
                }
                catch
                {
                    Assert.Fail();
                }
                driver1.Close();
                driver2.Close();
                driver3.Close();
            }
        }
    }
}