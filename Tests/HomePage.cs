using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;
using OpenQA.Selenium.Support.Extensions;
using MenkindRegistrationTests.Pages;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Home Page")]

    public class HomePageTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private BasePage? _basePage;
        private bool _testFailed = false;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]

        public void SetUp()
        {


            // Initialize Chrome
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();

            // Initialize BasePage
            _basePage = new BasePage(_driver);

            // Navigate to the homepage or registration page
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk/");

            // Handle modals
            _basePage.HandleModals();
        }

       

        [Test]
        [AllureTag("Regression")]
        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTms("TMS-12")]
        public void HomePageLoadsSuccessfully()
        {
            var homePage = new HomePageObject(_driver);
            Logger.Debug("Executing HomePageLoadsSuccessfully test");
            Assert.Multiple(() =>
            {
                Logger.Debug("Checking if logo is displayed");
                Assert.That(homePage.IsLogoDisplayed(), Is.True, "Logo is not displayed");
                Assert.That(homePage.IsLogoLoaded(), Is.True, "Logo is not properly loaded or may be corrupted");
                Logger.Debug("Verifying page title");
                Assert.That(homePage.GetTitle(), Is.EqualTo("Menkind | Unique Gadgets & Gifts for Men"), "Page title does not match expected");
            });
            Logger.Debug("Test completed successfully");
        }

        [Test]
        public void LoginSuccessful()
        {
            var homePage = new HomePageObject(_driver);

            Logger.Debug("Executing LoginSuccessful test");
            homePage.Submit();
            Logger.Debug("Login button clicked");
            homePage.EnterLoginEmail("testuser_cd17ee00-86e6-4e48-acbd-cc5835d8be2b@example.com");
            homePage.EnterLoginPass("TestPassword123!");
            var successMessage = _wait?.Until(driver =>
                   driver.FindElement(By.CssSelector("h1.page-heading.classyunicodedone"))
               );
            Logger.Info("Executing test user's logging");
            Assert.That(homePage.IsUserLoggedIn(), Is.True, "User is not logged in after registration");
            Logger.Info("User is logged in successfully");
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    }
}