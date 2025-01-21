using NLog;
using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Home Page")]

    public class HomePageTests
    {
        // Added: _basePage for handling common operations
        private IWebDriver _driver;
        private BasePage _basePage;
        private HomePageObject _homePageObject;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Initializing ChromeDriver...");
            ChromeOptions options = new();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--remote-debugging-port=9222");
            _driver = new ChromeDriver(options);
            Console.WriteLine("ChromeDriver initialized.");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Manage().Window.Maximize();


            _basePage = new BasePage(_driver);
            _homePageObject = new HomePageObject(_driver);
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk");
            _basePage.HandleModals();
        }

        [Test]
        [AllureTag("Smoke")]
        [AllureOwner("YourName")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTms("TMS-12")]
        public void Test_HomePageLoadsSuccessfully()
        {
            Logger.Info("Executing Test_HomePageLoadsSuccessfully");
            Assert.Multiple(() =>
            {              
                Assert.That(_homePageObject.IsLogoDisplayed(), Is.True, "Logo is not displayed");
                //Assert.That(_homePageObject.IsLogoLoaded(), Is.True, "Logo is not properly loaded or may be corrupted");
                Assert.That(_homePageObject.GetTitle(), Is.EqualTo("Menkind | Unique Gadgets & Gifts for Men"), "Page title does not match expected");
            });
            Logger.Info("Test completed successfully");
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
        }
    }
}