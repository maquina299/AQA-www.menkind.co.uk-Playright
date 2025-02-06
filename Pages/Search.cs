using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using www.menkind.co.uk.Tests;

namespace www.menkind.co.uk.Pages
{
    public class Search : BasePage
    {
        public Search(IWebDriver driver) : base(driver) { }

        // Locators
        private By SearchBox => By.Id("nav-quick-search");
        private By SearchResultsFrame => By.CssSelector("section.quickSearchResults.is-open");
        private By ProductsTab => By.CssSelector("div.quick-search__products"); // Ensure this exists
        private By NonProductsTab => By.CssSelector("div.quick-search__non-products"); // Ensure this exists
        private By SearchResultTitles => By.CssSelector("div.quickSearchResults a");
        private By PageBackground => By.CssSelector("div.quick-search__underlay.is-open");
        private By PageBackgroundAlt => (By.TagName("body"));


        // Methods
        public void EnterSearchQuery(string query)
        {
            Actions actions = new Actions(Driver);
            actions.SendKeys(Keys.Home).Perform();
            var searchBox = WaitForElementToBeVisible(SearchBox);
            searchBox.Clear();
            searchBox.SendKeys(TestData.query);
            Logger.Debug($"Entered search query: {query}");
        }

        public bool IsSearchFrameVisible()
        {

            try
            {
                Logger.Debug("Waiting for search results frame to appear...");
                if (WaitForElementToBeVisible(SearchResultsFrame).Displayed)
                {
                    Logger.Debug("Search results frame appeared.");
                    return true;
                }
                else return false;
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Warn("Search results frame did not appear within the expected time.");
                return false;
            }
            /*var frames = Driver.FindElements(SearchResultsFrame);
            return frames.Count > 0 && frames[0].Displayed;*/
        }


        public bool AreTabsPresent()
        {
            bool productsTabExists = Driver.FindElements(ProductsTab).Count > 0;
            bool nonProductsTabExists = Driver.FindElements(NonProductsTab).Count > 0;

            if (!productsTabExists) Logger.Warn("Products section is missing.");
            if (!nonProductsTabExists) Logger.Warn("Non-products section is missing.");

            return productsTabExists && nonProductsTabExists;
        }

        public bool AreSearchResultsRelevant(string keyword)
        {
            List<string> invalidResults = new();

            // Locators for search results in both sections
            var searchResultLinks = Driver.FindElements(By.CssSelector("ul li a.quick-search__results-link")); // Non-product results
            var productTitles = Driver.FindElements(By.CssSelector("ul.products li.product h3.card-title a")); // Product results

            // Combine all results into one list
            var allResults = searchResultLinks.Concat(productTitles).ToList();

            if (allResults.Count == 0)
            {
                Logger.Warn("No search results found.");
                return false;
            }

            // Validate each result
            foreach (var result in allResults)
            {
                string title = result.Text.ToLower();
                if (!title.Contains(TestData.query.ToLower()) && !title.Equals("new", StringComparison.OrdinalIgnoreCase))
                {
                    invalidResults.Add(title);
                }
            }

            if (invalidResults.Any())
            {
                Logger.Warn($"Some search results did not match: {string.Join(", ", invalidResults)}");
                Logger.Debug($"Total invalid results: {invalidResults.Count}");
                foreach (var item in invalidResults)
                {
                Logger.Debug($"Invalid result: {item}");
                }
                return false;
            }

            return true;
        }

        public void ClickOutsideSearchFrame()
        {
            try
            {
                var overlay = Driver.FindElement(PageBackground);
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", overlay);
                Logger.Debug("Clicked overlay using JavaScript.");
            }
            catch (NoSuchElementException)
            {
                Logger.Warn("Search overlay not found. Clicking the body instead.");
                Driver.FindElement(PageBackgroundAlt).Click();
            }
        }
    }  
}
