using MenkindRegistrationTests.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    public class RegistrationTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private BasePage? _basePage;
        private bool _testFailed = false;

        [SetUp]
        public void SetUp()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            // Initialize Chrome
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();

            // Initialize BasePage
            _basePage = new BasePage(_driver);

            // Navigate to the homepage or registration page
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk/login.php?action=create_account");

            // Handle modals
            _basePage.HandleModals();
        }


        [Test]
        public void UserRegistration_ShouldSucceed()
        {
            try
            {
                var registrationPage = new RegistrationPageObjects(_driver);

                // Fill in all the registration fields
                registrationPage.EnterFirstName("TestFirstName");
                registrationPage.EnterLastName("TestLastName");
                registrationPage.EnterEmail(registrationPage.GenerateUniqueEmail());
                registrationPage.EnterPassword("TestPassword123!");
                registrationPage.EnterPhoneNumber("1234567890");
                registrationPage.EnterPostCode("SW1A 1AA");
                registrationPage.EnterAddress("10 Downing Street");
                registrationPage.EnterCity("London");
                registrationPage.SelectCountry("United Kingdom");
                registrationPage.EnterCounty("London");
                registrationPage.SelectCountry("United Kingdom");

                registrationPage.Submit();

                // Success message validation
                var successMessage = _wait.Until(driver =>
                    driver.FindElement(By.CssSelector("h1.page-heading.classyunicodedone"))
                );
                Assert.That(successMessage.Displayed, "Success message should be displayed");
                Assert.That(successMessage.Text, Does.Contain("Your account has been created"));
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
                // Optionally, you can take a screenshot here
                // _driver?.GetScreenshot().SaveAsFile("error_screenshot.png", ScreenshotImageFormat.Png);
            }
            else
            {
                // Закрыть браузер
                _driver?.Quit(); _driver?.Dispose();
            }
        }
    }
}