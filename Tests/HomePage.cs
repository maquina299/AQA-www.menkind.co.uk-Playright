using NLog;
using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    public class HomePageTests
    {
        // Changed: Removed HomePage _homePage as it's no longer needed
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

            // Changed: Initialized BasePage instead of HomePage
            // This allows us to use common functionality like HandleModals
            _basePage = new BasePage(_driver);

            // Added: Initialized HomePageObject
            // This allows us to use home page specific methods
            _homePageObject = new HomePageObject(_driver);

            _driver.Navigate().GoToUrl("https://www.menkind.co.uk");
            // Changed: Use _basePage to handle modals instead of _homePage
            _basePage.HandleModals();
        }

        [Test]
        public void Test_HomePageLoadsSuccessfully()
        {
            Logger.Info("Executing Test_HomePageLoadsSuccessfully");
            Assert.Multiple(() =>
            {
                // Changed: Use _homePageObject instead of _homePage
                // This allows us to use specific methods defined in HomePageObject
                Assert.That(_homePageObject.IsLogoDisplayed(), Is.True, "Logo is not displayed");
                //Assert.That(_homePageObject.IsLogoLoaded(), Is.True, "Logo is not properly loaded or may be corrupted");
                Assert.That(_homePageObject.GetTitle(), Is.EqualTo("Menkind | Unique Gadgets & Gifts for Men"), "Page title does not match expected");
            });
            Logger.Info("Test completed successfully");
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}