using www.menkind.co.uk.Pages;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [AllureNUnit]
    [AllureSuite("Homepage")]
    [Obsolete]
    public class HomePageTests
    {
        // CHANGED: Removed the IWebDriver field since BasePage creates its own driver.
        private BasePage _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private HomePageObject homePage;

        [SetUp]
        public void SetUp()
        {
            bool enableImages = TestContext.CurrentContext.Test.Name == nameof(HomePageLoadsSuccessfully);
            _basePage = DriverFactory.SetupDriver(enableImages);
            homePage = new HomePageObject(_basePage.Driver);
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
          if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                homePage.TakeScreenshot();
                // Capture the screenshot if the test failed
            }
            DriverFactory.DisposeCurrentDriver(homePage);
        }
    }
}
