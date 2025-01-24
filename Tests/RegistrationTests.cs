using MenkindRegistrationTests.Pages;
using www.menkind.co.uk.Base;
using OpenQA.Selenium.Support.Extensions;

namespace www.menkind.co.uk.Tests
{
    [AllureNUnit]
    [TestFixture]
    [AllureSuite("Main suite")]
    [AllureSubSuite("Registration Page")]
    [Obsolete]
    public class RegistrationTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private BasePage? _basePage;
        private bool _testFailed = false;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]

        public void SetUp()
        {
            // Initialize Chrome with headless option
            ChromeOptions options = new();
            options.AddArgument("--headless"); options.AddArgument("--no-sandbox"); options.AddArgument("--disable-dev-shm-usage");

            // Initialize Chrome
            _driver = new ChromeDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            _driver.Manage().Window.Maximize();

            // Initialize BasePage
            _basePage = new BasePage(_driver);

            // Navigate to the homepage or registration page
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk/login.php?action=create_account");

            // Handle modals
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
            try
            {
                var registrationPage = new RegistrationPageObjects(_driver);
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


                Logger.Info("Test data input complited");

                registrationPage.Submit();
                Logger.Info("Registration submitted");

                // Success message validation, user's log in
                var successMessage = _wait.Until(driver =>
                    driver.FindElement(By.CssSelector("h1.page-heading.classyunicodedone"))
                );
                Logger.Info("Executing Test_SuccessMessage and user's logging in after registration");
                Assert.Multiple(() =>
                {
                    Assert.That(successMessage.Displayed, "Success message should be displayed");
                    Assert.That(successMessage.Text, Does.Contain("Your account has been created"));
                    Assert.That(registrationPage.IsUserLoggedIn(), Is.True, "User is not logged in after registration");

                });
                Logger.Info("Test completed successfully");

            }
            catch (Exception ex)
            {
                _testFailed = true;
                Assert.Fail($"Test failed with exception: {ex.Message}");
            }
        }
        [TearDown]
        public void TearDown()
        {
            if (_testFailed)
            {
                Console.WriteLine("Test failed. Browser will remain open for inspection.");
            }
            else
            {
                _driver?.TakeScreenshot().SaveAsFile("success_screenshot.png");
                // Close the browser
                _driver?.Quit(); _driver?.Dispose();
            }
        }
    }
}