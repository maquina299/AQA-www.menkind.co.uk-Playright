using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [AllureNUnit]
    [AllureSuite("Product Page")]
    [Obsolete]
    public class ProductPageTests
    {
        private BasePage _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private ProductPageObject productPage;

        [SetUp]
        public void SetUp()
        {
            _basePage = DriverFactory.SetupDriver(false, TestData.ProductPageURL);
            productPage = new ProductPageObject(_basePage.Driver);
        }

        [Test]
        [Category("Smoke")]
        [AllureSubSuite("Add to Cart")]
        public void AddItemToCart_ShouldUpdateCartIcon()
        {
            Logger.Debug("Starting test: AddItemToCart_ShouldUpdateCartIcon");

            // Step 1: Add item to basket
            productPage.AddToBasket();

            // Step 2: Verify cart icon updates
            Assert.That(productPage.IsCartUpdated(), Is.True, "Cart icon did not update to show 1 item.");

            // Step 3: Verify the item in the cart-summary API
            var cartSummaryResponse = productPage.GetCartSummary();
            Assert.That(cartSummaryResponse.ItemQuantities[TestData.AddedToTheCardProductId], Is.EqualTo(1), "Expected item not found in the cart.");
          //  Assert.That(false, Is.True, "Failed assert for debugging");
            Logger.Debug("Test passed: Cart icon updated successfully.");
        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public void SoldItem_ShouldCauseOOSmessage()
        {
            Logger.Debug("Starting test: SoldItem_ShouldCauseOOSMessage");

            // ✅ No need for `new BasePage(false)`, just navigate to the sold-out product
            _basePage.NavigateToUrl(TestData.SoldProductPageURL);
            Assert.Multiple(() =>
            {
                Assert.That(productPage.IsOOSMessage(), Is.True, "OOS is not displayed");
                Assert.That(productPage.IsPriceAmountPresent(), Is.False, "Price amount is displayed");
                Assert.That(productPage.IsAddToCartDisabled(), Is.True, "Add to Cart button is not disabled");
                Assert.That(productPage.GetAddToCartText(), Does.Contain("Out of Stock"), "Add to Cart button does not show 'Out of Stock'");
            });

            Logger.Debug("Test passed: OOS is displayed.");
        }
        [Test]
        [Category("Smoke")]
        [AllureSubSuite("Add to Cart")]
        public void ScreenshotTest()
        {
            productPage.TakeScreenshot();
        }

            [TearDown]
        public void TearDown()
        {
            DriverFactory.DisposeCurrentDriver(productPage); // ✅ Centralized cleanup for all drivers
        }
    }
}
