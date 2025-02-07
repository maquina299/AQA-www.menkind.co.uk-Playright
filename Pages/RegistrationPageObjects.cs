using www.menkind.co.uk.Base;

namespace MenkindRegistrationTests.Pages
{
    public class RegistrationPageObjects : BasePage
    {
        public RegistrationPageObjects(IWebDriver driver) : base() { }
        private By SuccessMessage => By.CssSelector("h1.page-heading.classyunicodedone");
        private IWebElement? EmailField => Driver.FindElement(By.Id("FormField_1_input"));
        private IWebElement? PasswordField => Driver.FindElement(By.Id("FormField_2_input"));
        private IWebElement? ConfirmPasswordField => Driver?.FindElement(By.Id("FormField_3_input"));
        private IWebElement? FirstNameField => Driver?.FindElement(By.Id("FormField_4_input"));
        private IWebElement? LastNameField => Driver?.FindElement(By.Id("FormField_5_input"));
        private IWebElement? PhoneNumField => Driver?.FindElement(By.Id("FormField_7_input"));
        private IWebElement? PostCodeField => Driver?.FindElement(By.Id("FormField_13_input"));
        private IWebElement? AddressField => Driver?.FindElement(By.Id("FormField_8_input"));
        private IWebElement? CityField => Driver?.FindElement(By.Id("FormField_10_input"));
        private IWebElement? CountryField => Driver?.FindElement(By.Id("FormField_11_select"));
        private IWebElement? CountyField => Driver?.FindElement(By.Id("FormField_12_input"));
        private IWebElement? SubmitButton => Driver?.FindElement(By.CssSelector("input[type='submit'][value='Create Account']"));
        public IWebElement? AccountLink => Driver?.FindElement(By.CssSelector("a.header__sign-in"));
        public bool IsUserLoggedIn()
        {
            return AccountLink?.Displayed ?? false;
        }

        // Interaction methods
        public void EnterFirstName(string firstName) => FirstNameField?.SendKeys(firstName);
        public void EnterLastName(string lastName) => LastNameField?.SendKeys(lastName);
        public void EnterEmail(string email) => EmailField?.SendKeys(email);
        public void EnterPassword(string password)
        {
            PasswordField?.SendKeys(password);
            ConfirmPasswordField?.SendKeys(password);
        }
        public void EnterPhoneNumber(string phoneNumber) => PhoneNumField?.SendKeys(phoneNumber);
        public void EnterPostCode(string postCode) => PostCodeField?.SendKeys(postCode);
        public void EnterAddress(string address) => AddressField?.SendKeys(address);
        public void EnterCity(string city) => CityField?.SendKeys(city);
        public void SelectCountry(string country)
        {
            if (CountryField != null)
            {
                var selectElement = new SelectElement(CountryField);
                selectElement.SelectByText(country);
            }
        }
        public void EnterCounty(string county) => CountyField?.SendKeys(county);

        public void Submit() => SubmitButton?.Click();
        public string GenerateUniqueEmail() => $"testuser_{Guid.NewGuid()}@example.com";
        public bool IsSuccessMessageVisible()
        {
            return WaitForElementToBeVisible(SuccessMessage).Displayed;
        }
        public string GetSuccessMessageText()
        {
            return WaitForElementToBeVisible(SuccessMessage).Text;
        }
    }
}