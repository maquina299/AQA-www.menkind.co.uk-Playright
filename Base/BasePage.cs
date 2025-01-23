namespace www.menkind.co.uk.Base
{
    public class BasePage
    {
        protected IWebDriver _driver;

        public BasePage(IWebDriver driver)
        {
            _driver = driver;
        }
        protected static ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ();
            options.AddArgument("--headless");  options.AddArgument("--no-sandbox"); options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--remote-debugging-port=9222");
            return options;
        }

        public void HandleModals()
        {
            try
            {
                // Закрытие окна Cookies
                var cookiesModal = _driver.FindElement(By.XPath("//button[contains(text(), 'Allow all Cookies')]"));
                if (cookiesModal.Displayed)
                {
                    var acceptCookiesButton = cookiesModal.FindElement(By.XPath("//button[contains(text(), 'Allow all Cookies')]"));
                    acceptCookiesButton.Click();
                    Console.WriteLine("Cookies modal closed.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Cookies modal not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while handling cookies modal: {ex.Message}");
            }
            try
            {
                // Закрытие окна Discount
                var discountModalCloseButton = _driver.FindElement(By.XPath("//button[contains(text(), 'No Thanks. Close Form')]"));
                if (discountModalCloseButton.Displayed)
                {
                    discountModalCloseButton.Click();
                    Console.WriteLine("Discount modal closed.");
                }
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Discount modal not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while handling discount modal: {ex.Message}");
            }
        }

        /*
         * public void TearDown()
        {
            if (_driver != null)
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
        }
        */
    }

}