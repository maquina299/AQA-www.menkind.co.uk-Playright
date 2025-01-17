using OpenQA.Selenium;

namespace MenkindRegistrationTests.Pages
{
    public class RegistrationPageObjects
    {
        private readonly IWebDriver _driver;

        public RegistrationPageObjects(IWebDriver driver)
        {
            _driver = driver;
        }

        // Локаторы
        private IWebElement FirstNameField => _driver.FindElement(By.Id("firstname"));
        private IWebElement LastNameField => _driver.FindElement(By.Id("lastname"));
        private IWebElement EmailField => _driver.FindElement(By.Id("email_address"));
        private IWebElement PasswordField => _driver.FindElement(By.Id("password"));
        private IWebElement ConfirmPasswordField => _driver.FindElement(By.Id("password-confirmation"));
        private IWebElement SubmitButton => _driver.FindElement(By.CssSelector("button[title='Create an Account']"));

        // Методы взаимодействия
        public void EnterFirstName(string firstName) => FirstNameField.SendKeys(firstName);
        public void EnterLastName(string lastName) => LastNameField.SendKeys(lastName);
        public void EnterEmail(string email) => EmailField.SendKeys(email);
        public void EnterPassword(string password)
        {
            PasswordField.SendKeys(password);
            ConfirmPasswordField.SendKeys(password);
        }
        public void Submit() => SubmitButton.Click();
        public string GenerateUniqueEmail()
        {
            return $"testuser_{Guid.NewGuid()}@example.com";
        }

    }
}
