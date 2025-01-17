using MenkindRegistrationTests.Pages;
using OpenQA.Selenium.Support.UI;
using www.menkind.co.uk.Pages;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    public class RegistrationTests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private HomePageObject _homePage;

        [SetUp]
        public void SetUp()
        {
            // Инициализация драйвера Chrome
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _driver.Manage().Window.Maximize();
            _homePage.HandleModals();
        }


        [Test]
        public void UserRegistration_ShouldSucceed()
        {
            var registrationPage = new RegistrationPageObjects(_driver);
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk/customer/account/create/");

            registrationPage.EnterFirstName("TestFirstName");
            registrationPage.EnterLastName("TestLastName");
            registrationPage.EnterEmail(registrationPage.GenerateUniqueEmail());
            registrationPage.EnterPassword("TestPassword123!");
            registrationPage.Submit();

            // Проверка успеха
            var successMessage = _wait.Until(driver =>
                driver.FindElement(By.CssSelector(".message-success"))
            );
            Assert.That(successMessage.Displayed, "Success message should be displayed");
            Assert.That(successMessage.Text, Does.Contain("Thank you for registering"));
        }
        [TearDown]
        public void TearDown()
        {
            // Закрыть браузер
            _driver?.Quit(); _driver?.Dispose();
        }
    }
}