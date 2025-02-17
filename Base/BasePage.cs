using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NLog;
using OpenQA.Selenium.Interactions;

namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ✅ WebDriver is now retrieved from DriverFactory dynamically
        public IWebDriver Driver => DriverFactory.GetCurrentDriver();

        // ✅ WebDriverWait is now dynamically created to ensure it's always available
        private readonly Lazy<WebDriverWait> _wait = new(() =>
            new WebDriverWait(DriverFactory.GetCurrentDriver(), TimeSpan.FromSeconds(5)));

        protected WebDriverWait Wait => _wait.Value;

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
                var alternativeClose = WaitForElementToBeVisible(By.XPath("//button[contains(text(), 'No Thanks')]"), TimeSpan.FromSeconds(5));
                alternativeClose?.Click();
                Logger.Info("Alternative discount modal closed.");
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Alternative discount modal not found.");
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

            }
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
        public void ScrollToElementWithActions(By elementLocator)
        {
            // Locate the element using the passed 'By' locator
            var element = Driver.FindElement(elementLocator);

            // Create an Actions object
            Actions actions = new(Driver);

            // Move to the element (this will scroll to it)
            actions.MoveToElement(element).Perform();
            WaitForElementToBeVisible(elementLocator);
        }
        public static string EnsureScreenshotsDirectoryExists()
        {
            string screenshotsFolder;

            if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
            {
            screenshotsFolder = Path.Combine(Environment.GetEnvironmentVariable("GITHUB_WORKSPACE"), "Screenshots");
            }
            else
            {
            screenshotsFolder = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Screenshots");
            }
            //string screenshotsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            // string screenshotsFolder = Path.Combine(Environment.GetEnvironmentVariable("GITHUB_WORKSPACE"), "Screenshots");
            Logger.Debug(screenshotsFolder);

            // Check if the directory exists
            if (!Directory.Exists(screenshotsFolder))
            {
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(screenshotsFolder);
                Logger.Info($"Created Screenshots directory at: {screenshotsFolder}");
            }
            else
            {
                Logger.Info($"Screenshots directory already exists at: {screenshotsFolder}");
            }
            return screenshotsFolder;
        }
        public void TakeScreenshot()
        {
            EnsureScreenshotsDirectoryExists();
            try
            {
               /* if (Driver == null)
                {
                    Logger.Error("WebDriver is null. Cannot capture screenshot.");
                    return;
                }*/


                  string fileName = $"failed_{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
                    Logger.Debug($"File name generated: {fileName}");
                
                
                // Get the Screenshots directory
                string screenshotsFolder = EnsureScreenshotsDirectoryExists();

                // Construct the full file path
                string filePath = Path.Combine(screenshotsFolder, fileName);

                // Capture the screenshot
                ITakesScreenshot screenshotDriver = (ITakesScreenshot)Driver;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(filePath);

                Logger.Info($"Screenshot saved to: {filePath}");
            }
            catch (Exception screenshotEx)
            {
                Logger.Error(screenshotEx, "Failed to capture screenshot");
            }
        }


    }
}
