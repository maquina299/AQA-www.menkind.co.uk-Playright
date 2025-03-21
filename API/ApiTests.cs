namespace www.menkind.co.uk.Tests

{
    [AllureNUnit]
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [AllureSuite("Main suite")]
    [AllureSubSuite("API tests")]
    [Obsolete]
    public class ApiTests
    {
        private static readonly Logger Logger = DriverFactory.GetLogger();

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Smoke")]
        public async Task LoginWithValidCredentials_ShouldShowOrdersPage()
        {
            // Step 1: Send API login request
            var (responseContent,_) = await ApiHelpers.SendLoginRequest(TestData.ValidEmail, TestData.ValidPassword);

            // Step 2: Validate login success
            ApiHelpers.ValidateSuccessfulLogin(responseContent);
        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public async Task CartSummaryResponse_ShouldHaveExpectedStructure()
        {
            // Step 1: Fetch API response
            var (_,cookies) = await ApiHelpers.SendLoginRequest(TestData.ValidEmail, TestData.ValidPassword);
            //string cookies2 = cookies.ToString();
            Logger.Info(cookies);
            string responseContent = await ApiHelpers.GetCartSummaryResponse(cookies);


            // Step 2: Validate response structure
            ApiHelpers.ValidateCartSummaryStructure(responseContent);
        }
    }
}
