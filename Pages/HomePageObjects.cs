using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Pages
{
    public class HomePageObject
    {
       /* private readonly IWebDriver _driver;

        public HomePageObject(IWebDriver driver)
        {
            _driver = driver;
        }
       */
        public class HomePage : BasePage
        {
            public HomePage(IWebDriver driver) : base(driver)
            {
            }

            // Другие методы, специфичные для HomePage
        }
        /*   public void HandleModals()
           {
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


           }
        */
        public bool IsLogoDisplayed()
        {
            return _driver.FindElement(By.CssSelector("a.header__logo")).Displayed;
        }

        public string GetTitle()
        {
            return _driver.Title;
        }
    }
}
