using NLog;
using www.menkind.co.uk.Pages;
using www.menkind.co.uk.Base;

namespace www.menkind.co.uk.Tests

{
    public class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        // Другие методы, специфичные для HomePage
    }
    [TestFixture]
    public class HomePageTests
    {
        private IWebDriver _driver;
        private HomePage _homePage;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


        [SetUp]
        
            public void Setup()
        {


            Console.WriteLine("Initializing ChromeDriver...");
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--remote-debugging-port=9222");
            _driver = new ChromeDriver();
            Console.WriteLine("ChromeDriver initialized.");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Manage().Window.Maximize();
            _homePage = new HomePage(_driver);
            _driver.Navigate().GoToUrl("https://www.menkind.co.uk");
            _homePage.HandleModals();

        }

        [Test]
        public void Test_HomePageLoadsSuccessfully()
        {
            Logger.Info("Executing Test_HomePageLoadsSuccessfully");
            Assert.Multiple(() =>
            {
                Assert.That(_homePageObjects.IsLogoDisplayed(), Is.True, "Logo is not displayed");
                Assert.That(_homePage.GetTitle(), Is.EqualTo("Menkind | Unique Gadgets & Gifts for Men"), "Page title does not contain 'Menkind'");
            });
            Logger.Info("Test completed successfully");
        }


        [TearDown]
        public void TearDown() { _driver?.Quit(); _driver?.Dispose(); }
    }
}
