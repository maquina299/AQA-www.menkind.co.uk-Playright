using www.menkind.co.uk.Pages;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)] // Uncomment if you want to run tests in parallel
    [AllureNUnit]
    [AllureSuite("Homepage")]
    [Obsolete]
    public class HomePageTests
    {
        // CHANGED: Removed the IWebDriver field since BasePage creates its own driver.
        private BasePage _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void SetUp()
        {
            bool enableImages = TestContext.CurrentContext.Test.Name == nameof(HomePageLoadsSuccessfully);
            _basePage = DriverFactory.SetupDriver(enableImages); // ✅ Fully handled in DriverFactory
        }

        [Test]
        [Category("Smoke")]
        [AllureSubSuite("Smoke")]
        [AllureTag("Smoke")]
        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTms("TMS-xx")]
        public void HomePageLoadsSuccessfully()
        {
            // CHANGED: Use _basePage.Driver to pass the driver to the page object.
            var homePage = new HomePageObject(_basePage.Driver);
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
            Logger.Debug(TestData.ValidEmail);
            var homePage = new HomePageObject(_basePage.Driver);

            Logger.Debug("Executing LoginSuccessful test");
            homePage.SignIn();
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
            DriverFactory.DisposeCurrentDriver(); // ✅ Each test disposes its own driver only
        }
    }
}
