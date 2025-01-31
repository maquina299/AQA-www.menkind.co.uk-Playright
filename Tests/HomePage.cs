using www.menkind.co.uk.Pages;


namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    // This will run all tests in this fixture in parallel
    //[Parallelizable(ParallelScope.All)] 

    [AllureNUnit]
    [AllureSuite("Homepage")]
    [Obsolete]
    public class HomePageTests
    {
        private BasePage? _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]

        //setup with enabling the images for the homepageloads test
        public void SetUp()
        {
            bool enableImages = TestContext.CurrentContext.Test.Name == nameof(HomePageLoadsSuccessfully);
            Logger.Debug("Enable image="+enableImages);
            _basePage = new BasePage(enableImages); 
            _basePage.NavigateToUrl(TestData.HomePageURL);
            _basePage.HandleModals();
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
            var homePage = new HomePageObject();
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
            var homePage = new HomePageObject();

            Logger.Debug("Executing LoginSuccessful test");
            homePage.SignIn();
           // ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", homePage.SignInLink);

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
            BasePage.QuitDriver(); // Quit driver after all tests
        }
    }
}