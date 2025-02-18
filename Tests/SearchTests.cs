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

            searchPage.EnterSearchQuery(searchQuery);
            Assert.Multiple(() =>
            {
                Assert.That(searchPage.IsSearchFrameVisible(), "Search results frame did not appear.");
                Assert.That(searchPage.AreTabsPresent(), "Expected search tabs (products/non-products) are missing.");
                Assert.That(searchPage.AreSearchResultsRelevant(searchQuery), "Search results do not contain the expected keyword.");
            });
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
            searchPage.OpenFiltersPanel();
            var (expectedItemCount, maxPrice) = searchPage.GetFirstFilterDetails();
            searchPage.WaitForViewButtonUpdate();
            searchPage.ApplyFilter();

            var (actualItemCount, allPricesValid) = searchPage.GetDisplayedProductCount(maxPrice);

            Assert.Multiple(() =>
            { 
            Assert.That(actualItemCount, Is.EqualTo(expectedItemCount),
                $"Expected {expectedItemCount} items but found {actualItemCount}.");
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
