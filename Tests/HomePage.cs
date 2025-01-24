using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;
using OpenQA.Selenium.Support.Extensions;
using MenkindRegistrationTests.Pages;

namespace www.menkind.co.uk.Tests
{
    [AllureNUnit]

    [TestFixture]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Home Page")]
    [Obsolete]
    public class HomePageTests
    {
        private IWebDriver _driver;
        private BasePage? _basePage;
        //private readonly bool _testFailed = false;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]

        public void SetUp()
        {
            // Initialize Chrome with headless option
            ChromeOptions options = new ();
            options.AddArgument("--headless");             options.AddArgument("--no-sandbox");            options.AddArgument("--disable-dev-shm-usage");

            // Initialize Chrome
            _driver = new ChromeDriver(options);
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
            Console.WriteLine(TestData.ValidEmail);
            var homePage = new HomePageObject(_driver);

            Logger.Debug("Executing LoginSuccessful test");
            // var successMessage = _wait?.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(homePage.SignInLink));
            // homePage.SignIn();
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", homePage.SignInLink);

            Logger.Debug("Filling in the Testdata");
            
            homePage.EnterLoginEmail(TestData.ValidEmail);
            homePage.EnterLoginPass(TestData.ValidPassword);

            Logger.Debug("Clicking submit button");
               
            //var successMessage = _wait?.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(homePage.SignInButton));
            homePage.Submit();
            Logger.Debug("Login button clicked");

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