namespace MenkindRegistrationTests.Pages
{
    public class RegistrationPageObjects : BasePage
    {
        public RegistrationPageObjects(IWebDriver driver) : base() { }

        private By SuccessMessage = By.CssSelector("h1.page-heading.classyunicodedone");
        private By EmailField = By.Id("FormField_1_input");
        private By PasswordField = By.Id("FormField_2_input");
        private By ConfirmPasswordField = By.Id("FormField_3_input");
        private By FirstNameField = By.Id("FormField_4_input");
        private By LastNameField = By.Id("FormField_5_input");
        private By PhoneNumField = By.Id("FormField_7_input");
        private By PostCodeField = By.Id("FormField_13_input");
        private By AddressField = By.Id("FormField_8_input");
        private By CityField = By.Id("FormField_10_input");
        private By CountryField = By.Id("FormField_11_select");
        private By CountyField = By.Id("FormField_12_input");
        private By SubmitButton = By.CssSelector("input[type='submit'][value='Create Account']");
        private By AccountLink = By.CssSelector("a.header__sign-in");

        public bool IsUserLoggedIn()
        {
            return WaitForElementToBeVisible(AccountLink, TimeSpan.FromSeconds(5)).Displayed;
        }

        public void EnterFirstName(string firstName)
            => WaitForElementToBeVisible(FirstNameField).SendKeys(firstName);

        public void EnterLastName(string lastName)
            => WaitForElementToBeVisible(LastNameField).SendKeys(lastName);

        public void EnterEmail(string email)
            => WaitForElementToBeVisible(EmailField).SendKeys(email);

        public void EnterPassword(string password)
        {
            WaitForElementToBeVisible(PasswordField).SendKeys(password);
            WaitForElementToBeVisible(ConfirmPasswordField).SendKeys(password);
        }

        public void EnterPhoneNumber(string phoneNumber)
            => WaitForElementToBeVisible(PhoneNumField).SendKeys(phoneNumber);

        public void EnterPostCode(string postCode)
            => WaitForElementToBeVisible(PostCodeField).SendKeys(postCode);

        public void EnterAddress(string address)
            => WaitForElementToBeVisible(AddressField).SendKeys(address);

        public void EnterCity(string city)
            => WaitForElementToBeVisible(CityField).SendKeys(city);

        public void SelectCountry(string country)
        {
            IWebElement countryDropdown = WaitForElementToBeVisible(CountryField);
            var selectElement = new SelectElement(countryDropdown);
            selectElement.SelectByText(country);
        }

        public void EnterCounty(string county)
            => WaitForElementToBeVisible(CountyField).SendKeys(county);

        public void Submit()
            => WaitForElementToBeVisible(SubmitButton).Click();

        public string GenerateUniqueEmail()
            => $"testuser_{Guid.NewGuid()}@example.com";

        public bool IsSuccessMessageVisible()
        {
            return WaitForElementToBeVisible(SuccessMessage, TimeSpan.FromSeconds(5)).Displayed;
        }

        public string GetSuccessMessageText()
        {
            return WaitForElementToBeVisible(SuccessMessage, TimeSpan.FromSeconds(5)).Text;
        }
    }
}
