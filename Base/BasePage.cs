using OpenQA.Selenium.Support.Extensions;

namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        protected IWebDriver? _driver;

        protected static readonly Logger Logger;

        static BasePage()
        {
            // Initialize NLog
            var config = new XmlLoggingConfiguration("Config/NLog.config");
            LogManager.Configuration = config;
            Logger = LogManager.GetCurrentClassLogger();
        }
        public BasePage(IWebDriver? driver)
        {
            _driver = driver;
        }
        public void InitializeDriver()
        {
            if (_driver == null)
            {
                ChromeOptions options = new();
                options.AddArgument("--headless"); options.AddArgument("--no-sandbox"); options.AddArgument("--disable-dev-shm-usage");

                _driver = new ChromeDriver(options);
                _driver.Manage().Window.Maximize();
            }
        }
        public IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                throw new InvalidOperationException("Driver is not initialized.");
            }
            return _driver;
        }
        public void NavigateToUrl(string url)
        {
            if (_driver == null)
            {
                throw new InvalidOperationException("WebDriver is not initialized.");
            }
            _driver.Navigate().GoToUrl(url);
        }
        public void TearDown()
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
                // Wait for Cookies modal to appear using ExpectedConditions
                WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(5));
                var cookiesModal = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(text(), 'Allow all Cookies')]")));

                if (cookiesModal.Displayed)
                {
                    var acceptCookiesButton = cookiesModal.FindElement(By.XPath("//button[contains(text(), 'Allow all Cookies')]"));
                    acceptCookiesButton.Click();
                    Logger.Info("Cookies modal closed.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Cookies modal not found within the wait time.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error while handling cookies modal: {ex.Message}");
            }
            // Scroll down to the end of the page to make the discount modal appears
            Logger.Info("Scrolling down to the end of the page");
            _driver.ExecuteJavaScript("window.scrollTo(0, document.body.scrollHeight);");

            try
            {
                // Wait for Discount modal to appear
                WebDriverWait wait = new(_driver, TimeSpan.FromSeconds(5));
                var discountModalCloseButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(text(), 'No Thanks. Close Form')]")));

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
            }
        }

      
        
    }

}