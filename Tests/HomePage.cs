using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;
using OpenQA.Selenium.Support.Extensions;
using MenkindRegistrationTests.Pages;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Homepage")]
    [Obsolete]
    public class HomePageTests 
    {
        private IWebDriver? _driver;
        private BasePage? _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]
        //setup with enabling the images for the homepageloads test
        public void SetUp()
        {
            bool enableImages = TestContext.CurrentContext.Test.Name == nameof(HomePageLoadsSuccessfully);
            InitializeDriverWithOptions(enableImages);
            
            _basePage = new BasePage(_driver);
            _basePage.NavigateToUrl("https://www.menkind.co.uk/");
            _basePage.HandleModals();
        }

        private void InitializeDriverWithOptions(bool enableImages)
        {
            ChromeOptions options = new();

            if (!enableImages)
            {
                // Disable image loading
                options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
            }

            _driver = new ChromeDriver(options);
            _driver.Manage().Window.Maximize();
        }
        //standard ver of setup
        /*public void SetUp()
        {           
            _basePage = new BasePage(null);
            _basePage.InitializeDriver();
            _driver = _basePage.GetDriver();

            // Navigate to the homepage
            _basePage.NavigateToUrl("https://www.menkind.co.uk/");
            _basePage.HandleModals();
        }*/



        [Test]
        [Category("Smoke")]
        [AllureSubSuite("Smoke")]
        [AllureTag("Smoke")]
        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTms("TMS-xx")]
        public void HomePageLoadsSuccessfully()
        {
            if (_driver == null)
            {
                throw new InvalidOperationException("WebDriver is not initialized (null).");
            }
            var homePage = new HomePageObject(_driver!);
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
        [Category("Regression")]
        [AllureTag("Regression")]
        [AllureSubSuite("Regression")]

        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTms("TMS-xx")]
        public void LoginSuccessful()
        {
            Console.WriteLine(TestData.ValidEmail);
            var homePage = new HomePageObject(_driver);

            Logger.Debug("Executing LoginSuccessful test");

            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", homePage.SignInLink);

            Logger.Debug("Filling in the Testdata");
            
            homePage.EnterLoginEmail(TestData.ValidEmail);
            homePage.EnterLoginPass(TestData.ValidPassword);

            Logger.Debug("Clicking submit button");
               
            homePage.Submit();
            Logger.Debug("Login button clicked");

            Logger.Info("Executing test user's logging");
            Assert.That(homePage.IsUserLoggedIn(), Is.True, "User is not logged in after registration");
            Logger.Info("User is logged in successfully");
        }

        [TearDown]
        public void TearDown()
        {
            _basePage?.TearDown();
            _driver?.Dispose();
        }
    }
}