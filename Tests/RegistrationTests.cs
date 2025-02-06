using MenkindRegistrationTests.Pages;

namespace www.menkind.co.uk.Tests
{
    [AllureNUnit]
    [TestFixture]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Registration Page")]
    [Obsolete]
    public class RegistrationTests
    {
        private BasePage _basePage;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private bool _testFailed = false;

        [SetUp]
        public void SetUp()
        {
            _basePage = new BasePage(false); // Pass the WebDriver to BasePage 
            _basePage.NavigateToUrl(TestData.RegistrationPageURL);
            _basePage.HandleModals();
        }

        [Test]
        [Category("Regression")]
        [AllureTag("Regression")]
        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTms("TMS-xx")]
        public void UserRegistration_ShouldSucceed()
        {

                var registrationPage = new RegistrationPageObjects(_basePage.Driver);
                Logger.Info("Started to fill in required registration fields");

                // Fill in all the registration fields
                try
                {
                    registrationPage.EnterFirstName("TestFirstName");
                    registrationPage.EnterLastName("TestLastName");
                    registrationPage.EnterEmail(registrationPage.GenerateUniqueEmail());
                    registrationPage.EnterPassword("TestPassword123!");
                    registrationPage.EnterPhoneNumber("1234567890");
                    registrationPage.SelectCountry("United Kingdom");
                    registrationPage.EnterPostCode("SW1A 1AA");
                    registrationPage.EnterAddress("10 Downing Street");
                    registrationPage.EnterCity("London");
                    registrationPage.EnterCounty("Greater London");
                    Logger.Info("Test data input completed");
                    registrationPage.Submit();
                    Logger.Info("Registration submitted");
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Error occurred while filling in registration fields: {ex.Message}");
                }

                try
                {
                    Logger.Info("Executing Test_SuccessMessage and user's logging in after registration");
                    Assert.Multiple(() =>
                    {
                        Assert.That(registrationPage.IsSuccessMessageVisible, "Success message should be displayed");
                        Assert.That(registrationPage.GetSuccessMessageText(), Does.Contain("Your account has been created"));
                        Assert.That(registrationPage.IsUserLoggedIn(), Is.True, "User is not logged in after registration");
                    });

                    Logger.Info("Test completed successfully");
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error during input actions: {ex.Message}");
                    Assert.Fail($"Input failed: {ex.Message}");
                }
            
        }


        [TearDown]
            public void TearDown()
            {
                _basePage.Driver.Dispose();
                Logger.Info("Browser closed and WebDriver disposed.");
            }
        
    }
}
