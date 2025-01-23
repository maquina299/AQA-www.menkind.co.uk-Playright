using www.menkind.co.uk.Base;


namespace www.menkind.co.uk.Pages
{
    public class HomePageObject : BasePage
    {
       
        public HomePageObject(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement? LogoSelector => _driver?.FindElement(By.CssSelector("a.header__logo"));
        private IWebElement? SignInButton => _driver?.FindElement(By.CssSelector("a.header__sign-in"));
        private IWebElement? LoginEmailField => _driver?.FindElement(By.Id("login_email"));
        private IWebElement? LoginPassField => _driver?.FindElement(By.Id("login_pass"));


        public bool IsLogoDisplayed()
        {
            return LogoSelector?.Displayed ?? false;
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
                WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(10));
                IWebElement logo = wait.Until(driver => driver.FindElement(By.CssSelector("a.header__logo")));

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
        public void EnterLoginEmail(string loginEmail) => LoginEmailField?.SendKeys(loginEmail);
        public void EnterLoginPass(string pass) => LoginPassField?.SendKeys(pass);
        public bool IsUserLoggedIn()
        {
            return SignInButton?.Displayed ?? false;
        }


        public void Submit() => SignInButton?.Click();


    }
}