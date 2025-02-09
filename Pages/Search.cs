using System.Text.RegularExpressions;
using www.menkind.co.uk.Tests;

namespace www.menkind.co.uk.Pages
{
    public class Search : BasePage
    {
        public Search(IWebDriver driver) : base() { }

#region Locators
        private By SearchBox => By.Id("nav-quick-search");
        private By SearchResultsFrame => By.CssSelector("section.quickSearchResults.is-open");
        private By ProductsTab => By.CssSelector("div.quick-search__products"); // Ensure this exists
        private By NonProductsTab => By.CssSelector("div.quick-search__non-products"); // Ensure this exists
        private By SearchResultTitles => By.CssSelector("div.quickSearchResults a");
        private By PageBackground => By.CssSelector("div.quick-search__underlay.is-open");
        private By PageBackgroundAlt => (By.TagName("body"));
        private By ViewAllFiltersButton = By.XPath("//button[contains(text(), 'View all filters')]");
        private By FirstFilterLabel = By.CssSelector("li.facet-list__item:nth-child(3) a.facet-list__action");
        private By ViewButton = By.XPath("//button[contains(text(), 'View (')]");
        private By ProductItems = By.CssSelector("li.product article.product-card");
        private By ProductPrices = By.CssSelector("ul.products li.product .product-price span");
        private By PriceFilter = By.CssSelector("button[aria-controls='facet-price']");
        private By PaginationNextLink = By.CssSelector("li.pagination-item--next a.pagination-link");
        private By PaginationNextItem = By.CssSelector("li.pagination-item--current + li:not(.pagination-item--next) a.pagination-link");
        private By PaginationElementsAfterCurrentNotNextButton = By.CssSelector("li.pagination-item--current ~ li.pagination-item:not(.pagination-item--next)");


        #endregion
        #region Search frame tests
        public void EnterSearchQuery(string query)
        {
            //Actions actions = new Actions(Driver);
            //actions.SendKeys(Keys.Home).Perform();
            var searchBox = WaitForElementToBeVisible(SearchBox);
            searchBox.Clear();
            searchBox.SendKeys(query);
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
            // Errors list in case of the test failure
            List<string> errors = new List<string>();
            
            List<string> invalidSearchResults = new();

            // Locators for search results in both sections
            var searchResultLinks = Driver.FindElements(By.CssSelector("ul li a.quick-search__results-link")); // Non-product results
            var productTitles = Driver.FindElements(By.CssSelector("ul.products li.product h3.card-title a")); // Product results

            // Combine all results into one list
            var allResults = searchResultLinks.Concat(productTitles).ToList();

            if (allResults.Count == 0)
            {
                errors.Add("No data in the search frame");
            }
            if (productTitles.Count == 0)
            {
                Logger.Warn("No product found.");
                try
                {
                    var noItemsElement = Driver.FindElement(By.CssSelector("ul.products li"));

                    if (noItemsElement.Text == "No items found.")
                    {
                        Logger.Debug("The message 'No items found.' is displayed.");
                    }

                }
                catch (NoSuchElementException)
                {
                errors.Add("The 'No items found.' element is not present.");
                }
            }
            // Validate each result
            foreach (var result in allResults)
            {
                string title = result.Text.ToLower();
                Logger.Debug($"Result: {title}");
                if (!title.Contains(keyword.ToLower()) && !title.Equals("new", StringComparison.OrdinalIgnoreCase))
                {
                    invalidSearchResults.Add(title);
                }
            }

            if (invalidSearchResults.Any() && !invalidSearchResults.SequenceEqual(TestData.ValidResultTitles) && productTitles.Count == 0)
            {
                Logger.Warn($"Some search results did not match: {string.Join(", ", invalidSearchResults)}");
                Logger.Debug($"Total invalid results: {invalidSearchResults.Count}");
                foreach (var item in invalidSearchResults)
                {
                Logger.Debug($"Invalid result: {item}");
                }
                errors.Add("Invalid results were found");
            }
            if (errors.Any())
            {
                Assert.Fail("Test failed due to the following issues:\n" + string.Join("\n", errors));
                return false;
            }
            else
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
        #endregion
#region Search filters tests
        public void OpenFiltersPanel()
        {
            WaitForElementToBeClickable(ViewAllFiltersButton).Click();
            Logger.Info("Opened filter panel.");
        }

        // ✅ Step 3: Capture first filter details (returns item count & max price)
        public (int itemCount, decimal? maxPrice) GetFirstFilterDetails()
        {
            WaitForElementToBeVisible(PriceFilter);
            WaitForElementToBeClickable(PriceFilter).Click();
            // Get the filter label text (e.g., "£10 and under (7)")
            IWebElement filter = WaitForElementToBeVisible(FirstFilterLabel);
            string filterText = filter.Text.Trim();

            // Extract the number of results (e.g., "(7)" → 7)
            int itemCount = int.Parse(Regex.Match(filterText, @"\((\d+)\)").Groups[1].Value);

            // Extract the price range (e.g., "£10 and under" → 10.00)
            decimal? maxPrice = ExtractPrice(filterText);

            // Click the filter
            filter.Click();
            Logger.Info($"Selected filter: {filterText} (Expecting {itemCount} items under £{maxPrice}).");
            return (itemCount, maxPrice);
        }

        // ✅ Step 4: Apply filter
        public void WaitForViewButtonUpdate()
        {
            // Capture the initial text of the "View" button
            var initialText = Driver.FindElement(ViewButton).Text.Trim();

            // Use the existing _wait instance to wait for the text to change
            Wait.Until(driver =>
            {
                var currentText = driver.FindElement(ViewButton).Text.Trim();
                return !currentText.Equals(initialText, StringComparison.OrdinalIgnoreCase);
            });

            Logger.Info("The 'View' button has updated with new data.");
        }



        public void ApplyFilter()
        {
           // Thread.Sleep(2000);
            WaitForElementToBeClickable(ViewButton).Click();
            Logger.Info("Filter applied.");
        }

        // ✅ Step 5: Get number of displayed items
        public int GetDisplayedProductCount()
        {
            int totalProductCount = 0;
            int currentPage = 1;
            bool isNextPageAvailable=false;

            do
            {
                // Find all product items on the current page and add them to the total count
                var products = Driver.FindElements(ProductItems);
                totalProductCount += products.Count;

                Logger.Info($"Page {currentPage}: Found {products.Count} products. Total so far: {totalProductCount}.");

                // Check if the "Next" button exists and is the immediate next to the first page

                if (Driver.FindElements(PaginationElementsAfterCurrentNotNextButton).Any())
                {
                    // Click the "Next" button to go to the next page
                    GoToNextPage();
                    WaitForProductsToLoad();
                    currentPage++;
                    isNextPageAvailable = true;
                }
                else isNextPageAvailable = false;
     

            } while (isNextPageAvailable);

            Logger.Info($"Total products found after filtering across all pages: {totalProductCount}.");
            return totalProductCount;
        }
        private void WaitForProductsToLoad()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(driver => driver.FindElements(ProductItems).Any()); // Wait until products are visible
            Logger.Info("Page fully loaded with products.");
        }


        private void GoToNextPage()
        {
            var nextButton = WaitForElementToBeClickable(PaginationNextItem);
            nextButton.Click();

            Logger.Info("Navigated to the next page.");
        }


        // ✅ Step 6: Validate product prices
        public bool ValidateProductPrices(decimal? maxPrice)
        {
            // Extract prices using the ExtractPrice method
            var prices = Driver.FindElements(ProductPrices)
                               .Select(e => ExtractPrice(e.Text))
                               .ToList();

            // Validate that all prices are not null and less than or equal to the maxPrice
            bool allPricesValid = prices.All(price =>
                price.HasValue && price.Value <= maxPrice);  // Ensure price is not null and is within the max price

            if (!allPricesValid)
            {
                // Log which prices are invalid
                var invalidPrices = prices
                    .Where(price => !price.HasValue || price.Value > maxPrice)
                    .Select(price => price.HasValue ? price.Value.ToString("C") : "null")
                    .ToList();

                Logger.Warn($"Invalid prices detected: {string.Join(", ", invalidPrices)}");
            }

            // Log the overall result
            Logger.Info($"Price validation: All items ≤ £{maxPrice}: {allPricesValid}");

            return allPricesValid;
        }


        // 🔹 Extracts price from text (e.g., "£10 and under" → 10.00)
        private decimal? ExtractPrice(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;  // If empty, return null
            }

            var match = Regex.Match(text, @"£(\d+(\.\d{1,2})?)");

            return match.Success ? decimal.Parse(match.Groups[1].Value) : (decimal?)null;  // Return null if not a valid price
        }
        #endregion

    }
}
