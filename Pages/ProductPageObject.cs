using OpenQA.Selenium.Support.Extensions;

namespace www.menkind.co.uk.Pages
{
    public class ProductPageObject : BasePage
    {
        public ProductPageObject() { }

        // Selectors
        private By AddToBasketButton => By.Id("form-action-addToCart");
        private By BasketIcon => By.CssSelector("span.countPill.cart-quantity.countPill--positive"); // Icon showing item count
        private By SubmitAdding => By.XPath("//a[contains(@class, 'button--primaryGhost') and contains(text(), 'Continue Shopping')]"); // Icon showing item count

        // Methods
        public void AddToBasket()
        {
            IWebElement addToCartButton = WaitForElementToBeVisible(AddToBasketButton);
            _driver.ExecuteJavaScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", addToCartButton);
            //((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", addToCartButton);

            WaitForElementToBeClickable(AddToBasketButton).Click();
            WaitForElementToBeClickable(SubmitAdding).Click();


            WaitForElementToBeVisible(BasketIcon);

            Logger.Debug("Clicked 'Add to Basket' button.");
        }

        public bool IsCartUpdated()
        {
            try
            {
                return WaitForElementToBeVisible(BasketIcon).Text.Trim() == "1";
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Cart count did not update in time.");
                return false;
            }
        }
    }
}
