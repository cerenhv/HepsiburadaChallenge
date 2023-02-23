using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SpecFlow.Actions.Selenium;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using TechTalk.SpecFlow;

namespace HepsiburadaChallenge.StepDefinitions
{
    [Binding]
    public class HepsiburadaScenariosStepDefinitions
    {
        IWebDriver driver;

        [Before]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments("start-maximized");
            options.AddArguments("disable-notifications");
            options.AddArguments("disable-popup-blocking");
            driver = new ChromeDriver(options);

            driver.Navigate().GoToUrl("https://www.hepsiburada.com/");
        }

        [Given(@"I open the Chrome browser and go to Hepsiburada website.")]
        public void OpenBrowserAndGotoWebSite()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait.Until(driver => driver.FindElement(By.Id("myAccount")));
        }

        [Given(@"I login with '([^']*)' and '([^']*)'")]
        public void Login(string email, string password)
        {
            driver.FindElement(By.Id("myAccount")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("login")).Click();
            Thread.Sleep(2000);

            Actions acts = new Actions(driver);
            acts.SendKeys(Keys.PageDown).Build().Perform();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("onetrust-accept-btn-handler")).Click();
            Thread.Sleep(2000);

            //Login with Google
            driver.FindElement(By.Id("btnGoogle")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElement(By.Id("identifierId")));
            driver.FindElement(By.Id("identifierId")).SendKeys(email);
            driver.FindElement(By.XPath("//*[@id=\"identifierNext\"]/div/button")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.Name("password")).SendKeys(password);
            driver.FindElement(By.XPath("//*[@id=\"passwordNext\"]/div/button")).Click();

            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            wait2.Until(driver => driver.FindElement(By.Id("myAccount")));
            Assert.AreEqual("https://www.hepsiburada.com/", driver.Url.ToString());
        }

        [Given(@"I go to user informations")]
        public void GotoUserInformations()
        {
            driver.FindElement(By.Id("myAccount")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id=\"myAccount\"]/div/div/ul/li[5]/a")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual("https://hesabim.hepsiburada.com/uyelik-bilgilerim", driver.Url.ToString());
        }

        [When(@"I search '([^']*)' in product catalog")]
        public void SearchProduct(string searchText)
        {
            driver.FindElement(By.ClassName("theme-IYtZzqYPto8PhOx3ku3c")).SendKeys(searchText);
            driver.FindElement(By.ClassName("searchBoxOld-yDJzsIfi_S5gVgoapx6f")).Click();
            Thread.Sleep(2000);
        }

        [When(@"I add (.*) and (.*) products to the basket")]
        public void AddProductsToTheBasket(int number1, int number2)
        {
            string firstProduct = "", secondProduct = "";

            if (number1 >= 1 && number2 >= 1)
            {
                firstProduct = "i" + (number1 - 1);
                secondProduct = "i" + (number2 - 1);
            }

            IWebElement product1 = driver.FindElement(By.XPath("//*[@id=\"" + firstProduct + "\"]"));

            Actions actions = new Actions(driver);
            actions.SendKeys(Keys.Down).ScrollToElement(product1).Build().Perform();
            actions.MoveToElement(product1).Build().Perform();
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//*[@id=\"" + firstProduct + "\"]/div/a/div[2]/button")).Click();

            IWebElement product2 = driver.FindElement(By.XPath("//*[@id=\"" + secondProduct + "\"]"));
            //Actions actions2 = new Actions(driver);
            actions.MoveToElement(product2).Build().Perform();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id=\"" + secondProduct + "\"]/div/a/div[2]/button")).Click();
        }

        [When(@"I go to the basket and view added products")]
        public void GotoBasket()
        {
            Actions acts = new Actions(driver);
            acts.SendKeys(Keys.PageUp).Build().Perform();

            driver.Navigate().GoToUrl("https://checkout.hepsiburada.com/sepetim");
            Thread.Sleep(3000);
            string totalProductAmount = driver.FindElement(By.XPath("//*[@id=\"basket-item-count\"]")).Text.ToString();
            Assert.AreEqual("https://checkout.hepsiburada.com/sepetim", driver.Url.ToString());
            Assert.AreEqual(2, Convert.ToInt32(totalProductAmount));
        }


        [When(@"I increase amounts of products and check the amounts")]
        public void IncreaseAmount()
        {
            IWebElement product2 = driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/div/div[2]/div[2]/section/section/div[4]/ul/li/div[1]/div[2]/div[2]/div[2]/div[1]/div/a[2]"));
            Actions actions3 = new Actions(driver);
            actions3.SendKeys(Keys.Down).ScrollToElement(product2).Build().Perform();

            product2.Click();
            Thread.Sleep(5000);
            string ProductAmount = driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/div/div[2]/div[2]/section/section/div[4]/ul/li/div[1]/div[2]/div[2]/div[2]/div[1]/div/input")).GetAttribute("value").ToString();
            Assert.AreEqual(2, Convert.ToInt32(ProductAmount));
            
            //Delete products from basket
            actions3.SendKeys(Keys.PageUp).Build().Perform();
            driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/div/div[2]/div[2]/section/section/div[3]/ul/li/div[1]/div[2]/div[2]/div[2]/div[1]/div/a[1]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/div/div[2]/div[2]/section/section/div[3]/ul/li/div[1]/div[2]/div[2]/div[2]/div[1]/a")).Click();
            Thread.Sleep(2000);
        }

        [When(@"I change birth date")]
        public void ChangeBirthDate()
        {
            var random = new Random();
            var lowerBound = 1;
            var upperBound = 28;
            var rNum = random.Next(lowerBound, upperBound);
            driver.FindElement(By.Id("txtBirthDay")).SendKeys("07."+rNum.ToString()+".1990");
        }

        [Then(@"I update the informations and check the date")]
        public void UpdateUserInfos()
        {
            Actions actUserInfo = new Actions(driver);
            actUserInfo.SendKeys(Keys.Down).ScrollToElement(driver.FindElement(By.XPath("//*[@class=\"dZAEjxk-ljB6ui8oOynrl\"]/button"))).Build().Perform();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@class=\"dZAEjxk-ljB6ui8oOynrl\"]/button")).Click();
            Thread.Sleep(3000);
        }

        [Then(@"I go back to home page")]
        public void GotoHomePage()
        {
            driver.FindElement(By.XPath("//*[@class=\"sf-voltran-body voltran-body full Header\"]/div/div/div[1]/div/div[1]/a")).Click();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            wait.Until(driver => driver.FindElement(By.Id("myAccount")));
            Assert.AreEqual("https://www.hepsiburada.com/", driver.Url.ToString());
        }

        [Then(@"I logout from Hepsiburada")]
        public void Logout()
        {
            driver.FindElement(By.Id("myAccount")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//*[@id=\"myAccount\"]/div/div/ul/li[10]/a")).Click();
            Thread.Sleep(3000);

            string label1 = driver.FindElement(By.ClassName("sf-OldMyAccount-d0xCHLV38UCH5cD9mOXq")).Text.ToString();
            Assert.AreEqual("Giriş Yap", label1);
        }

        [AfterScenario]
        public void Close()
        {
            driver.Quit();
        }
    }
}
