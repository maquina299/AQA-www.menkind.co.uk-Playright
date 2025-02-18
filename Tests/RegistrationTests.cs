using MenkindRegistrationTests.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests
{
    [AllureNUnit]
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Registration Page")]
    [Obsolete]
    public class RegistrationTests
    {
        private BasePage _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private RegistrationPageObjects registrationPage;

        [SetUp]
        public void SetUp()
        {
            _basePage = DriverFactory.SetupDriver(false, TestData.RegistrationPageURL); // ✅ Centralized driver setup
            registrationPage = new RegistrationPageObjects(_basePage.Driver);

        }

        [Test]
        [Category("Regression")]
        [AllureTag("Regression")]
        [AllureOwner("Vlad")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTms("TMS-xx")]
        public void UserRegistration_ShouldSucceed()
        {
            Logger.Info("Started to fill in required registration fields");

            // Fill in all the registration fields
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

            // Submit Registration
            registrationPage.Submit();
            Logger.Info("Registration submitted");

            // Verify Registration Success
            Logger.Info("Executing Test_SuccessMessage and checking user's login status after registration");

            Assert.Multiple(() =>
            {
                Assert.That(registrationPage.IsSuccessMessageVisible(), "Success message should be displayed");
                Assert.That(registrationPage.GetSuccessMessageText(), Does.Contain("Your account has been created"));
                Assert.That(registrationPage.IsUserLoggedIn(), Is.True, "User is not logged in after registration");
            });

            Logger.Info("Test completed successfully");
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.DisposeCurrentDriver(registrationPage);
        }
    }
}
