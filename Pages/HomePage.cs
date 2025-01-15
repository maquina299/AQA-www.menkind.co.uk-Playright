using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using www.menkind.co.uk.Pages;
using NLog;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    public class HomePageTests
    {
        private IWebDriver _driver;
        private HomePageObject _homePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]
        public void Setup()
        {

            
            Console.WriteLine("Initializing ChromeDriver...");
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--remote-debugging-port=9222");
            _driver = new ChromeDriver();
            Console.WriteLine("ChromeDriver initialized.");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Manage().Window.Maximize();
            _homePage = new HomePageObject(_driver);
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk");
            // Закрываем модальные окна
            _homePage.HandleModals();
             // Ожидаем появления кнопки "Allow all Cookies" по классу
            //WebDriverWait wait = new (_driver, TimeSpan.FromSeconds(10));
            //IWebElement cookieButton = wait.Until(driver => driver.FindElement(By.ClassName("css-a0j149")));

            // Нажимаем на кнопку
           // cookieButton.Click();
        }

        [Test]
        public void Test_HomePageLoadsSuccessfully()
        {
            Logger.Info("Executing Test_HomePageLoadsSuccessfully");
            Assert.Multiple(() =>
            {
                Assert.That(_homePage.IsLogoDisplayed(), Is.True, "Logo is not displayed");
                Assert.That(_homePage.GetTitle(), Is.EqualTo("Menkind | Unique Gadgets & Gifts for Men"), "Page title does not contain 'Menkind'");
            });
            Logger.Info("Test completed successfully");
        }


     [TearDown]
     public void TearDown() {_driver?.Quit(); _driver?.Dispose(); }
     }
}
