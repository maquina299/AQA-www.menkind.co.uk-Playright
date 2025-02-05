using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;

namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        protected static IWebDriver? _driver;
        protected static readonly Logger Logger;
        private ChromeOptions _defaultOptions = new ChromeOptions();

        // Locators
        private static readonly By CookiesAcceptButton = By.XPath("//button[contains(text(), 'Allow all Cookies')]");
        private static readonly By DiscountIconCloseButton = By.ClassName("klaviyo-close-form");
        private static readonly By AlternativeDiscountCloseButton = By.XPath("//button[contains(text(), 'No Thanks')]");

        static BasePage()
        {
            // Initialize NLog
            var config = new XmlLoggingConfiguration("Config/NLog.config");
            LogManager.Configuration = config;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public BasePage(bool enableImages = false)
        {
            if (_driver == null)
            {
                InitializeDriver(enableImages);
            }
        }

        private void InitializeDriver(bool enableImages)
        {
            Logger.Debug("Initializing WebDriver...");

            if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
            {
                Logger.Info("Running in GitHub Actions - Using CI settings.");
                _defaultOptions.AddArgument("--headless");  // Headless mode for CI
                _defaultOptions.AddArgument("--no-sandbox");
                _defaultOptions.AddArgument("--disable-dev-shm-usage");
            }
            else
            {
                Logger.Info("Running locally - Using local settings.");
                _defaultOptions.AddArgument("--start-maximized");  // Fullscreen for local tests
            }

            if (!enableImages)
            {
                _defaultOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                Logger.Debug("Images are disabled for this test.");
            }

            try
            {
                _driver = new ChromeDriver(_defaultOptions);
                Logger.Debug("WebDriver initialized successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error initializing WebDriver: {ex.Message}");
                throw;
            }
        }

        public static IWebDriver GetDriver()
        {
            return _driver ?? throw new InvalidOperationException("WebDriver is not initialized.");
        }

        public void NavigateToUrl(string url)
        {
            if (_driver == null) throw new InvalidOperationException("WebDriver is not initialized.");
            Logger.Debug($"Navigating to {url}");
            _driver.Navigate().GoToUrl(url);
        }

        public IWebElement WaitForElementToBeVisible(By locator, TimeSpan? timeout = null)
        {
            WebDriverWait wait = new(_driver, timeout ?? TimeSpan.FromSeconds(5));
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitForElementToBeClickable(By locator, TimeSpan? timeout = null)
        {
            WebDriverWait wait = new(_driver, timeout ?? TimeSpan.FromSeconds(5));
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public static void QuitDriver()
        {
            _driver?.Quit();
            _driver?.Dispose();
            _driver = null;
        }

        public void HandleModals()
        {
            if (_driver == null)
            {
                Logger.Warn("WebDriver is not initialized.");
                return;
            }

            try
            {
            WaitForElementToBeVisible(CookiesAcceptButton).Click();
            Logger.Info("Cookies modal closed.");
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Cookies modal not found within the wait time.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while handling cookies modal: {ex.Message}");
            }

            try
            {
                WaitForElementToBeVisible(DiscountIconCloseButton).Click();
            }
            catch (Exception ex)
            {
                Logger.Warn("No discount modal icon or failed to close it: {ex.Message}");
                try
                {
                    WaitForElementToBeVisible(AlternativeDiscountCloseButton).Click();
                }

                catch (Exception exi)
                {
                    Logger.Warn($"No discount modal found or failed to close it: {ex.Message}");
                }
            }

        }

        /* 
         public void HandleModals()

         {
         if (_driver == null)
         {
          Logger.Debug("WebDriver is not initialized.");
          return;
         }
         try
         {
          WaitForElementToBeClickable(CookiesAcceptButton).Click();
          Logger.Info("Cookies modal closed.");

         }
         catch (WebDriverTimeoutException)
         {
             Logger.Warn("Cookies modal not found within the wait time.");
         }
         catch (Exception ex)
         {
             Logger.Error($"Error while handling cookies modal: {ex.Message}");
         }

         try
         {
             WaitForElementToBeClickable(DiscountIconCloseButton).Click();
             Logger.Info("Discount close button clicked.");
         }
         catch (WebDriverTimeoutException)
         {
             Logger.Warn("Discount icon close button not found.");
         }
         catch (Exception ex)
         {
             Logger.Error($"Error while handling discount modal: {ex.Message}");
         }*/
    }
    
}
