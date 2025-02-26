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
        [Test]
        [Category("Regression")]
        [AllureSubSuite("Smoke")]
        public async Task LoginWithValidCredentials_ShouldShowOrdersPage()
        {
            // Step 1: Send API login request
            string responseContent = await ApiHelpers.SendLoginRequest(TestData.ValidEmail, TestData.ValidPassword);

            // Step 2: Validate login success
            ApiHelpers.ValidateSuccessfulLogin(responseContent);
        }

        [Test]
        [Category("Regression")]
        [AllureSubSuite("Regression")]
        public async Task CartSummaryResponse_ShouldHaveExpectedStructure()
        {
            ApiHelpers.SendLoginRequest(TestData.ValidEmail, TestData.ValidPassword);
            // Step 1: Fetch API response
            string responseContent = await ApiHelpers.GetCartSummaryResponse();

            // Step 2: Validate response structure
            ApiHelpers.ValidateCartSummaryStructure(responseContent);
        }
    }
}
