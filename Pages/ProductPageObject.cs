using Newtonsoft.Json;
using OpenQA.Selenium.Support.Extensions;
using RestSharp;
using www.menkind.co.uk.Tests;

namespace www.menkind.co.uk.Pages
{
    public class ProductPageObject : BasePage
    {
        public ProductPageObject(IWebDriver driver) : base() { }

        // Selectors
        private By AddToBasketButton => By.Id("form-action-addToCart");
        private By BasketIcon => By.CssSelector("span.countPill.cart-quantity.countPill--positive"); // Icon showing item count
        private By SubmitAdding => By.XPath("//a[contains(@class, 'button--primaryGhost') and contains(text(), 'Continue Shopping')]"); // Icon showing item count
        private By OOSMessage => By.XPath("//h3[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'out of stock')]");
        //Out of stock messgae
        private By PriceAmount => By.XPath("span.product-price__amount");

        // Methods
        public void AddToBasket()
        {
            IWebElement addToCartButton = WaitForElementToBeVisible(AddToBasketButton);
            Driver.ExecuteJavaScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", addToCartButton);
            //((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'nearest'});", addToCartButton);

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

        /* public bool IsPriceAmountPresent()
         {
             return Driver.FindElements(PriceAmount).Count > 0;
         }*/
        public bool IsPriceAmountPresent()
        {
            bool isPresent = Driver.FindElements(PriceAmount).Count > 0;
            Logger.Debug($"IsPriceAmountPresent returned: {isPresent}");
            return isPresent;
        }
        // Fetch and validate the cart-summary API response
        public CartSummaryResponse GetCartSummary()
        {

            // Create the client and request
            var client = new RestClient(TestData.GetCartSummary);
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("User-Agent", "Mozilla/5.0");
            request.AddHeader("Accept", "application/json");
            AddCookiesToRequest(request);

            var response = client.Execute(request);

            if (!response.IsSuccessful)
            {
                Logger.Error($"Cart summary API call failed: {response.ErrorMessage}");
                throw new InvalidOperationException("Failed to fetch cart summary.");
            }

            Logger.Debug($"Cart summary response: {response?.Content}");
            return JsonConvert.DeserializeObject<CartSummaryResponse>(response?.Content);
        }

        // Model for the Cart Summary API response
        public class CartSummaryResponse
        {
            [JsonProperty("line_item_count")]
            public int LineItemCount { get; set; }

            [JsonProperty("item_quantities")]
            public Dictionary<string, int> ItemQuantities { get; set; } = new();

            [JsonProperty("total_price")]
            public decimal TotalPrice { get; set; }
        }
        public class AddResponse //Added product response data
        {
          
            public class CartSummaryResponse
            {
                [JsonProperty("data")]
                public CartData? Data { get; set; }
            }

            public class CartData
            {
                [JsonProperty("line_items")]
                public List<LineItem>? LineItems { get; set; }
            }

            public class LineItem
            {
                [JsonProperty("product_id")]
                public int ProductId { get; set; }
                
                [JsonProperty("product_name")]
                public string? ProductName { get; set; }
            }
        }
        private void AddCookiesToRequest(RestRequest request)
        {
            // Get all cookies from the current WebDriver session
            ICollection<Cookie> cookies = Driver.Manage().Cookies.AllCookies;

            foreach (var cookie in cookies)
            {
                // Add each cookie to the request
                request.AddCookie(cookie.Name, cookie.Value, null, "www.menkind.co.uk");
                Logger.Debug($"Added cookie: {cookie.Name} = {cookie.Value}");
            }
        }
        #region OOS tests
        public bool IsOOSMessage()
        {
            return WaitForElementToBeVisible(OOSMessage).Displayed;
        }
        public bool IsAddToCartDisabled()
        {
            var addToCartButton = WaitForElementToBeVisible(AddToBasketButton);
            return !addToCartButton.Enabled;
        }
        public string GetAddToCartText()
        {
            var addToCartButton = WaitForElementToBeVisible(AddToBasketButton);
            return addToCartButton.GetDomAttribute("value")?.Trim() ?? "";
        }
        #endregion
    }

}
    

