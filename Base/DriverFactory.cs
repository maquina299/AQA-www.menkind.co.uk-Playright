using System.Collections.Concurrent;
using www.menkind.co.uk.Tests;

namespace www.menkind.co.uk.Base
{
    public static class DriverFactory
    {
        // Private logger, only accessible through the GetLogger method
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Public property to access Logger
        public static Logger GetLogger()
        {
            return Logger;
        }
        private static readonly ConcurrentDictionary<int, IWebDriver> _drivers = new();
        static DriverFactory()
        {
            // Initialize NLog
            var config = new XmlLoggingConfiguration("Config/NLog.config");
            LogManager.Configuration = config;
        }
        public static BasePage SetupDriver(bool enableImages = false, string? url = null)

        {
            Logger.Debug("Initializing WebDriver...");

            ChromeOptions options = new();

            if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
            {
                Logger.Info("Running in GitHub Actions - Using CI settings.");
                options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
            }
            else
            {
                Logger.Info("Running locally - Using local settings.");
                options.AddArgument("--start-maximized");
            }

            if (!enableImages)
            {
                options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                Logger.Debug("Images are disabled for this test.");
            }

            IWebDriver driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(90);

            int threadId = Environment.CurrentManagedThreadId;
            Logger.Debug($"WebDriver assigned for thread {threadId}.");

            if (!_drivers.ContainsKey(threadId))
            {
                _drivers[threadId] = driver;
            }
            else
            {
                Logger.Warn($"WebDriver already exists for thread {threadId}. Overwriting.");
                _drivers[threadId].Quit();
                _drivers[threadId] = driver;
            }

            Logger.Debug("WebDriver initialized successfully.");

            var basePage = new BasePage();
            string targetUrl = string.IsNullOrEmpty(url) ? TestData.HomePageURL : url;
            Logger.Debug($"Navigating to {targetUrl}");
            basePage.NavigateToUrl(targetUrl);
            basePage.HandleModals();

            return basePage;
        }

        public static void DisposeCurrentDriver(BasePage basePage)
        {

            int threadId = Environment.CurrentManagedThreadId;
            if (_drivers.TryRemove(threadId, out IWebDriver? driver) && driver != null)
            {
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    basePage.TakeScreenshot();
                    // Capture the screenshot if the test failed
                }
                try
                {
                    Logger.Debug($"Disposing WebDriver for thread {threadId}.");
                    driver.Quit();
                }
                catch (Exception ex)
                {
                    Logger.Warn($"Error disposing WebDriver: {ex.Message}");
                }
                finally
                {
                    driver.Dispose();
                }
            }
        }

        public static IWebDriver GetCurrentDriver()
        {

            int threadId = Environment.CurrentManagedThreadId;
            if (_drivers.TryGetValue(threadId, out IWebDriver? driver) && driver != null)
            {
                return driver;
            }

            throw new InvalidOperationException("No active WebDriver found for this test.");
        }
    }
}
