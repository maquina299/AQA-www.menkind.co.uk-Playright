using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NLog;

namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ✅ WebDriver is now retrieved from DriverFactory dynamically
        public IWebDriver Driver => DriverFactory.GetCurrentDriver();

        // ✅ WebDriverWait is now dynamically created to ensure it's always available
        protected WebDriverWait Wait => new(Driver, TimeSpan.FromSeconds(10));

        public BasePage() { }

        // ✅ Navigate to a URL (Ensures WebDriver is valid before navigation)
        public void NavigateToUrl(string url)
        {
            if (Driver == null)
            {
                Logger.Error("WebDriver is null. Cannot navigate to URL.");
                throw new InvalidOperationException("WebDriver is not initialized.");
            }

            Logger.Debug($"Navigating to {url}");
            Driver.Navigate().GoToUrl(url);
        }

        // ✅ Default wait for element to be visible
        public IWebElement WaitForElementToBeVisible(By locator, TimeSpan? timeout = null)
        {
            return new WebDriverWait(Driver, timeout ?? TimeSpan.FromSeconds(5))
                        .Until(ExpectedConditions.ElementIsVisible(locator));
        }

        // ✅ Default wait for element to be clickable
        public IWebElement WaitForElementToBeClickable(By locator, TimeSpan? timeout = null)
        {
            return new WebDriverWait(Driver, timeout ?? TimeSpan.FromSeconds(3))
                        .Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        // ✅ Improved Modal Handling (Handles popups only if elements exist)
        public void HandleModals()
        {
            if (Driver == null)
            {
                Logger.Warn("WebDriver is not initialized.");
                return;
            }

            try
            {
                var cookieButton = WaitForElementToBeVisible(By.XPath("//button[contains(text(), 'Allow all Cookies')]"), TimeSpan.FromSeconds(5));
                cookieButton?.Click();
                Logger.Info("Cookies modal closed.");
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Cookies modal not found within 5 seconds.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while handling cookies modal: {ex.Message}");
            }

            try
            {
                var discountClose = WaitForElementToBeVisible(By.ClassName("klaviyo-close-form"), TimeSpan.FromSeconds(5));
                discountClose?.Click();
                Logger.Info("Discount modal closed.");
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Discount modal not found.");
            }
            catch (Exception ex)
            {
                Logger.Warn($"Error while handling discount modal: {ex.Message}");
                try
                {
                    var alternativeClose = WaitForElementToBeVisible(By.XPath("//button[contains(text(), 'No Thanks')]"), TimeSpan.FromSeconds(5));
                    alternativeClose?.Click();
                    Logger.Info("Alternative discount modal closed.");
                }
                catch (WebDriverTimeoutException)
                {
                    Logger.Warn("Alternative discount modal not found.");
                }
                catch (Exception exi)
                {
                    Logger.Warn($"Error handling alternative discount modal: {exi.Message}");
                }
            }
        }


    }
}
