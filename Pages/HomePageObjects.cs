
using OpenQA.Selenium.Interactions;

namespace www.menkind.co.uk.Pages
{
    public class HomePageObject : BasePage
    {
        public HomePageObject() {}
        private By LogoSelector => By.CssSelector("a.header__logo");
        private By SignInLink => By.CssSelector("a.header__sign-in");
        private By LoginEmailField => By.Id("login_email");
        private By LoginPassField => By.Id("login_pass");
        private By SignInButton => By.CssSelector("input[type='submit'][value='Sign In']");
        private By AccountLink => By.CssSelector("a.header__sign-in[href = '/account.php']");
        public void EnterLoginEmail(string loginEmail) => WaitForElementToBeVisible(LoginEmailField).SendKeys(loginEmail);
        public void EnterLoginPass(string pass) => WaitForElementToBeVisible(LoginPassField).SendKeys(pass);

        /* private IWebElement? LogoSelector => _driver?.FindElement(By.CssSelector("a.header__logo"));
         public IWebElement? SignInLink => _driver?.FindElement(By.CssSelector("a.header__sign-in"));
         private IWebElement? LoginEmailField => _driver?.FindElement(By.Id("login_email"));
         private IWebElement? LoginPassField => _driver?.FindElement(By.Id("login_pass"));
         public IWebElement? SignInButton => _driver?.FindElement(By.CssSelector("input[type='submit'][value='Sign In']"));
         public IWebElement? AccountLink => _driver?.FindElement(By.CssSelector("a.header__sign-in[href = '/account.php']"));
        */



        public bool IsLogoDisplayed()
        {
            return _driver?.FindElement(LogoSelector).Displayed ?? false;
        }

        public bool IsLogoLoaded()
        {
            if (_driver == null)
            {
                Logger.Error("WebDriver is not initialized.");
                return false;
            }

            try
            {
                // Wait for the logo to be present
                IWebElement logo = WaitForElementToBeVisible(LogoSelector);

                // Check if the logo image is loaded using JavaScript
                string script = @"
                    var img = arguments[0].querySelector('img');
                    return img && img.complete && 
                           typeof img.naturalWidth != 'undefined' && 
                           img.naturalWidth > 0;
                ";
                bool isLoaded = (bool)((IJavaScriptExecutor)_driver).ExecuteScript(script, logo);

                if (!isLoaded)
                {
                    Logger.Warn("Logo image is not fully loaded");
                }

                return isLoaded;
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Error("Timeout occurred while waiting for logo element");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Unexpected error occurred while checking if logo is loaded");
                return false;
            }
        }

        public string GetTitle()
        {
            return _driver?.Title ?? string.Empty;
        }
        //Login test part
        public bool IsUserLoggedIn()
        {
            try
            {
                return WaitForElementToBeVisible(AccountLink).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        public void SignIn()
        {
            WaitForElementToBeClickable(SignInLink).Click();
        }
        public void Submit() => WaitForElementToBeVisible(SignInButton).Click();


    }
}