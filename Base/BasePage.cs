using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.Extensions;
using System.Runtime.InteropServices;

namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        protected static IWebDriver? _driver;
        protected static readonly Logger Logger;
        private ChromeOptions _defaultOptions = new ChromeOptions();
        private static By DiscountIconCloseButton => By.ClassName("klaviyo-close-form");
        private static By CookiesModal => By.XPath("//button[contains(text(), 'Allow all Cookies')]");
        private static By CookiesAcceptButton => By.XPath("//button[contains(text(), 'Allow all Cookies')]");

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
                //Initialize only once
                InitializeDriver(enableImages); 
            }
        }


        private void InitializeDriver(bool enableImages)
        {
            Logger.Debug("Initializing WebDriver...");
                if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
    {
        Logger.Info("Running in GitHub Actions - Using CI settings.");
        _defaultOptions.AddArgument("--headless");  // Run in headless mode in GitHub
        _defaultOptions.AddArgument("--no-sandbox");
        _defaultOptions.AddArgument("--disable-dev-shm-usage");
    }
    else
    {
        Logger.Info("Running locally - Using local settings.");
        _defaultOptions.AddArgument("--start-maximized");  // Open full screen in local
    }

            // Apply the image setting based on enableImages flag
            if (!enableImages)
            {
                _defaultOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2); // Disable images
                Logger.Debug("Images are disabled for this test.");
            }

            try
            {
                _driver = new ChromeDriver(_defaultOptions);
                _driver.Manage().Window.Maximize();
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
            if (_driver == null)
            {
                throw new InvalidOperationException("WebDriver is not initialized.");
            }
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
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        public void HandleModals()

        {
            if (_driver == null)
            {
                Console.WriteLine("WebDriver is not initialized.");
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
           /* try
            {
                // Wait for Discount modal to appear
                var discountModalCloseButton = WaitForElementToBeVisible(By.XPath("//button[starts-with(text(), 'No Thanks.')]"));

                if (discountModalCloseButton.Displayed)
                {
                    discountModalCloseButton.Click();
                    Logger.Info("Discount modal closed.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Info("Discount modal not found within the wait time.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while handling discount modal: {ex.Message}");
            }*/
        }

      
        
    }

}
