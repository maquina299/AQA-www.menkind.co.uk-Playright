using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;
using OpenQA.Selenium.Support.Extensions;


namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Home Page")]

    public class HomePageTests : BasePage
    {      
        private  HomePageObject _homePageObject;
        private WebDriverWait _wait;


        public HomePageTests() : base(InitializeDriver())
        {
            _homePageObject = new HomePageObject(_driver!);
        }


        private static IWebDriver InitializeDriver()
        {
            ChromeOptions options = GetChromeOptions();
            return new ChromeDriver(options);
        }

        static HomePageTests()
        {
            // Initialize NLog
            var config = new XmlLoggingConfiguration("Config/NLog.config");
            LogManager.Configuration = config;
        }

            [SetUp]

        public void Setup()

        {
            Logger.Info("Initializing ChromeDriver...");
            Logger.Info("ChromeDriver initialized.");
            _driver!.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Manage().Window.Maximize();

            _homePageObject = new HomePageObject(_driver);
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk");
            HandleModals();

            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

        }

        [Test]
        [AllureTag("Regression")]
        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTms("TMS-12")]
        public void HomePageLoadsSuccessfully()
        {
            Logger.Debug("Executing HomePageLoadsSuccessfully rest");
            Assert.Multiple(() =>
            {
                Logger.Debug("Checking if logo is displayed");
                Assert.That(_homePageObject.IsLogoDisplayed(), Is.True, "Logo is not displayed");
                Assert.That(_homePageObject.IsLogoLoaded(), Is.True, "Logo is not properly loaded or may be corrupted");
                Logger.Debug("Verifying page title");
                Assert.That(_homePageObject.GetTitle(), Is.EqualTo("Menkind | Unique Gadgets & Gifts for Men"), "Page title does not match expected");
            });
            Logger.Debug("Test completed successfully");
        }
        [Test]
        public void LoginSuccessful()
        {
            Logger.Debug("Executing LoginSuccessful test");
            _homePageObject.Submit();
            Logger.Debug("Login button clicked");
            _homePageObject.EnterLoginEmail("testuser_cd17ee00-86e6-4e48-acbd-cc5835d8be2b@example.com");
            _homePageObject.EnterLoginPass("TestPassword123!");
            var successMessage = _wait?.Until(driver =>
                   driver.FindElement(By.CssSelector("h1.page-heading.classyunicodedone"))
               );
            Logger.Info("Executing test user's logging");
            Assert.That(_homePageObject.IsUserLoggedIn(), Is.True, "User is not logged in after registration");
            Logger.Info("User is logged in succesfuly");
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