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

        [SetUp]
        public void SetUp()
        {
            _basePage = DriverFactory.SetupDriver(false, TestData.HomePageURL); // ✅ Centralized driver setup
        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public void SearchFrame_ShouldDisplayResults_CloseAfterOutsideClick()
        {
            var searchPage = new Search(_basePage.Driver);
            var searchQuery = TestData.SearchQuery;
            Logger.Debug("Starting test: SearchBox_ShouldDisplayResults_WhenSearchingForBeer");

            // Step 1: Enter search query "beer"
            searchPage.EnterSearchQuery(searchQuery);

            // Step 2: Verify search results frame appears
            Assert.That(searchPage.IsSearchFrameVisible(), "Search results frame did not appear.");

            // Step 3: Verify that both tabs exist
            Assert.That(searchPage.AreTabsPresent(), "Expected search tabs (products/non-products) are missing.");

            // Step 4: Verify search results contain the keyword "beer"
            Assert.That(searchPage.AreSearchResultsRelevant(searchQuery), "Search results do not contain the expected keyword.");
            Logger.Debug("Test passed: Search frame displayed relevant results successfully.");

            // Step 5: Close the search frame by clicking outside
            Logger.Debug("Starting test: Search frame is closed after clicking outside the frame.");
            searchPage.ClickOutsideSearchFrame();

            Assert.That(searchPage.IsSearchFrameVisible(), Is.False, "Failed to click outside search box.");
            Logger.Debug("Test passed: Search frame is closed after clicking outside the frame.");
        }

        [TearDown]
        public void TearDown()
        {
            DriverFactory.DisposeCurrentDriver(); // ✅ Centralized WebDriver cleanup
        }
    }
}
