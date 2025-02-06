
namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        private IWebDriver _driver;
        protected WebDriverWait _wait;
        protected static readonly Logger Logger; // CHANGED: Single static Logger declaration
        private ChromeOptions _defaultOptions = new ChromeOptions();

        // Static constructor to initialize the Logger configuration
        static BasePage()
        {
            var config = new XmlLoggingConfiguration("Config/NLog.config");
            LogManager.Configuration = config;
            Logger = LogManager.GetCurrentClassLogger();
        }

        // CHANGED: Constructor that creates a new WebDriver instance with the enableImages option.
        public BasePage(bool enableImages = false)
        {
            InitializeDriver(enableImages);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        // CHANGED: Overloaded constructor to use an existing WebDriver instance (for page objects).
        public BasePage(IWebDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        private void InitializeDriver(bool enableImages)
        {
            Logger.Debug("Initializing WebDriver...");

            if (Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true")
            {
                Logger.Info("Running in GitHub Actions - Using CI settings.");
                _defaultOptions.AddArgument("--headless");
                _defaultOptions.AddArgument("--no-sandbox");
                _defaultOptions.AddArgument("--disable-dev-shm-usage");
            }
            else
            {
                Logger.Info("Running locally - Using local settings.");
                _defaultOptions.AddArgument("--start-maximized");
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

        // CHANGED: Expose the private _driver so that page objects can use it.
        public IWebDriver Driver => _driver;

        public void NavigateToUrl(string url)
        {
            if (_driver == null)
                throw new InvalidOperationException("WebDriver is not initialized.");

            Logger.Debug($"Navigating to {url}");
            _driver.Navigate().GoToUrl(url);
        }

        public IWebElement WaitForElementToBeVisible(By locator, TimeSpan? timeout = null)
        {
            return new WebDriverWait(_driver, timeout ?? TimeSpan.FromSeconds(5))
                        .Until(ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitForElementToBeClickable(By locator, TimeSpan? timeout = null)
        {
            return new WebDriverWait(_driver, timeout ?? TimeSpan.FromSeconds(3))
                        .Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        public void Dispose()
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
                Logger.Warn("WebDriver is not initialized.");
                return;
            }

            try
            {
                // CHANGED: Using inline locator for cookies accept button.
                WaitForElementToBeVisible(By.XPath("//button[contains(text(), 'Allow all Cookies')]")).Click();
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
                // CHANGED: Using inline locator for discount modal close button.
                WaitForElementToBeVisible(By.ClassName("klaviyo-close-form")).Click();
            }
            catch (Exception ex)
            {
                Logger.Warn($"No discount modal icon or failed to close it: {ex.Message}");
                try
                {
                    WaitForElementToBeVisible(By.XPath("//button[contains(text(), 'No Thanks')]")).Click();
                }
                catch (Exception exi)
                {
                    Logger.Warn($"No discount modal found or failed to close it: {ex.Message}");
                }
            }
        }
    }
}
