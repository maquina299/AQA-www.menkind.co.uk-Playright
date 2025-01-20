using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Pages
{
    public class HomePageObject : BasePage
    {
        public HomePageObject(IWebDriver driver) : base(driver)
        {
        }
        private IWebElement LogoSelector => _driver.FindElement((By.CssSelector("a.header__logo")));

        public bool IsLogoDisplayed()
        {
            return LogoSelector.Displayed;
        }
        public bool IsLogoLoaded()
        {
            var logo = LogoSelector;
            return (bool)((IJavaScriptExecutor)_driver).ExecuteScript(
                "return arguments[0].complete && typeof arguments[0].naturalWidth != 'undefined' && arguments[0].naturalWidth > 0", logo);
        }


        public string GetTitle()
        {
            return _driver.Title;
        }

        // Add other methods specific to the home page as needed!
    }
}