using www.menkind.co.uk.Pages;


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
            var productPage = new ProductPageObject(_basePage!.Driver);

            Logger.Debug("Starting test: AddItemToCart_ShouldUpdateCartIcon");
            #region test
            // Step 1: Add item to basket
            productPage.AddToBasket();
            #endregion
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
            //OOS message should be displayed, price amount is absent, add to the cart button is disabled with the Out of Stock label
        {
            if (_basePage == null)
            {
                _basePage = new BasePage(false);
            }
            _basePage.NavigateToUrl(TestData.SoldProductPageURL);

            var productPage = new ProductPageObject(_basePage.Driver);
            Logger.Debug("Starting test: SoldItem_ShouldCauseOOSMessage");
            Assert.Multiple(() =>
            {
                Assert.That(productPage.IsOOSMessage(), Is.True, "OOS is not displayed");
                Assert.That(productPage.IsPriceAmountPresent(), Is.False, "Price amount is displayed");
                Assert.That(productPage.IsAddToCartDisabled(), Is.True, "Add to Cart button is not disabled");
                Assert.That(productPage.GetAddToCartText(), Does.Contain("Out of Stock"), "Add to Cart button does not show 'Out of Stock'");

            });
            Logger.Debug("Test passed: OOS is displayed.");
        }
        
        [TearDown]
        public void TearDown()
        {
            _basePage?.Driver.Dispose();
        }
    }
}
