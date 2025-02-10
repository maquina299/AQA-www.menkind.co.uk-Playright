using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [AllureNUnit]
    [AllureSuite("Search Functionality")]
    [Obsolete]
    public class SearchTests
    {
        private BasePage _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Search searchPage;
        [SetUp]
        public void SetUp()
        {
        _basePage = DriverFactory.SetupDriver(false, TestData.SearchPageURL);
        searchPage = new Search(_basePage.Driver);

        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public void SearchFrame_ShouldDisplayResults_CloseAfterOutsideClick()
        {
            var searchQuery = TestData.SearchQuery;
            Logger.Debug("Starting test: SearchBox_ShouldDisplayResults_WhenSearchingForBeer");

            // Step 1: Enter search query "beer"
            searchPage.EnterSearchQuery(searchQuery);

            // Step 2: Verify search results frame appears
            Assert.Multiple(() =>
            {
                Assert.That(searchPage.IsSearchFrameVisible(), "Search results frame did not appear.");
            // Step 3: Verify that both tabs exist
                Assert.That(searchPage.AreTabsPresent(), "Expected search tabs (products/non-products) are missing.");
            // Step 4: Verify search results contain the keyword "beer"
                Assert.That(searchPage.AreSearchResultsRelevant(searchQuery), "Search results do not contain the expected keyword.");
            });
            // Step 5: Close the search frame by clicking outside
            Logger.Debug("Starting test: Search frame is closed after clicking outside the frame.");
                searchPage.ClickOutsideSearchFrame();
                Assert.That(searchPage.IsSearchFrameVisible(), Is.False, "Failed to click outside search box.");
                Logger.Debug("Test passed: Search frame is closed after clicking outside the frame.");
            
        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public void VerifySearchFilters()
        {
            Logger.Info("Starting test: Verify Search Filters");
            if (_basePage == null)
            {
                throw new InvalidOperationException("BasePage is not initialized.");
            }
            // Step 2: Open the filters panel
            searchPage.OpenFiltersPanel();

            // Step 3: Capture filter details
            var (expectedItemCount, maxPrice) = searchPage.GetFirstFilterDetails();
            // Step 4: Apply the selected filter
            searchPage.WaitForViewButtonUpdate();
            searchPage.ApplyFilter();

            int actualItemCount = searchPage.GetDisplayedProductCount();

            Assert.Multiple(() =>
            { 
            // Step 5: Validate the number of results
            Assert.That(actualItemCount, Is.EqualTo(expectedItemCount),
                $"Expected {expectedItemCount} items but found {actualItemCount}.");

            // Step 6: Validate all product prices
            bool allPricesValid = searchPage.ValidateProductPrices(maxPrice);
            Assert.That(allPricesValid, Is.True, "Some items exceed the selected price range.");

            Logger.Info("Test passed: Filter applied correctly.");
            });
        }
        [TearDown]
        public void TearDown()
        {
            DriverFactory.DisposeCurrentDriver(searchPage);
        }
    }
}
