using www.menkind.co.uk.Base;

namespace MenkindRegistrationTests.Pages
{
    public class RegistrationPageObjects : BasePage
    {
        public RegistrationPageObjects(IWebDriver driver) : base(driver)
        {
        }

        // Локаторы
 
        private IWebElement EmailField => _driver.FindElement(By.Id("FormField_1_input"));
        private IWebElement PasswordField => _driver.FindElement(By.Id("FormField_2_input"));
        private IWebElement ConfirmPasswordField => _driver.FindElement(By.Id("FormField_3_input"));
        private IWebElement FirstNameField => _driver.FindElement(By.Id("FormField_4_input"));
        private IWebElement LastNameField => _driver.FindElement(By.Id("FormField_5_input"));
        private IWebElement PhoneNumField => _driver.FindElement(By.Id("FormField_7_input"));
        private IWebElement PostCodeField => _driver.FindElement(By.Id("FormField_13_input"));
        private IWebElement AddressField => _driver.FindElement(By.Id("FormField_8_input"));
        private IWebElement CityField => _driver.FindElement(By.Id("FormField_10_input"));
        private IWebElement CountryField => _driver.FindElement(By.Id("FormField_11_select"));
        private IWebElement CountyField => _driver.FindElement(By.Id("FormField_12_input"));

        private IWebElement SubmitButton => _driver.FindElement(By.CssSelector("input[type='submit'][value='Create Account']"));






        // Interaction methods
        public void EnterFirstName(string firstName) => FirstNameField.SendKeys(firstName);
        public void EnterLastName(string lastName) => LastNameField.SendKeys(lastName);
        public void EnterEmail(string email) => EmailField.SendKeys(email);
        public void EnterPassword(string password)
        {
            PasswordField.SendKeys(password);
            ConfirmPasswordField.SendKeys(password);
        }
        public void EnterPhoneNumber(string phoneNumber) => PhoneNumField.SendKeys(phoneNumber);
        public void EnterPostCode(string postCode) => PostCodeField.SendKeys(postCode);
        public void EnterAddress(string address) => AddressField.SendKeys(address);
        public void EnterCity(string city) => CityField.SendKeys(city);
        public void SelectCountry(string country)
        {
            var selectElement = new SelectElement(CountryField);
            selectElement.SelectByText(country);
        }
        public void EnterCounty(string county) => CountyField.SendKeys(county);

        public void Submit() => SubmitButton.Click();
        public string GenerateUniqueEmail() => $"testuser_{Guid.NewGuid()}@example.com";

    }
}
