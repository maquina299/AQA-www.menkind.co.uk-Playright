using www.menkind.co.uk.Pages;
using System.Collections.Generic;


namespace www.menkind.co.uk.Tests
{
    [TestFixture]
    [AllureNUnit]
    [AllureSuite("Product Page")]
    [Obsolete]
    public class ProductPageTests
    {
        private BasePage? _basePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [SetUp]
        public void SetUp()
        {
            _basePage = new BasePage(false);
            _basePage.NavigateToUrl(TestData.ProductPageURL);
            _basePage.HandleModals();
        }

        [Test]
        [Category("Smoke")]
        [AllureSubSuite("Add to Cart")]
        public void AddItemToCart_ShouldUpdateCartIcon()
        {
            var productPage = new ProductPageObject();

            Logger.Debug("Starting test: AddItemToCart_ShouldUpdateCartIcon");

            // Step 1: Add item to basket
            productPage.AddToBasket();
            // Step 2: Verify cart icon updates
            Assert.That(productPage.IsCartUpdated(), Is.True, "Cart icon did not update to show 1 item.");
            // Step 3: Verify the item in the cart-summary API
            var cartSummaryResponse = productPage.GetCartSummary();
            // Access ItemQuantities dictionary explicitly
            Assert.That(cartSummaryResponse.ItemQuantities["33709"], Is.EqualTo(1), "Expected item not found in the cart.");
            Logger.Debug("Test passed: Cart icon updated successfully.");
        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public void SoldItem_ShouldCauseOOSmessage()
        {
            if (_basePage == null)
            {
                _basePage = new BasePage(false);  // Ensure _basePage is initialized
            }
            _basePage.NavigateToUrl(TestData.SoldProductPageURL);

            var productPage = new ProductPageObject();
            Logger.Debug("Starting test: SoldItem_ShouldCauseOOSMessage");
            Assert.Multiple(() =>
            {
                Assert.That(productPage.IsOOSMessage(), Is.True, "OOS is not displayed");
                Assert.That(productPage.IsPriceAmountPresent(), Is.False, "Price amount is displayed");
            });
            Logger.Debug("Test passed: OOS is displayed.");
        }
        
        [TearDown]
        public void TearDown()
        {
            BasePage.QuitDriver();
        }
    }
}
