
using www.menkind.co.uk.Helpers;

namespace www.menkind.co.uk.Tests
{
    [AllureNUnit]
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [AllureSuite("Main suite")]
    [AllureSubSuite("API tests")]
    [Obsolete]
    public class ApiLoginTests
    {
        [Category("Regression")]
        [AllureSubSuite("Smoke")]
        public async Task LoginWithValidCredentials_ShouldShowOrdersPage()
        {
            // Step 1: Send API login request
            string responseContent = await ApiHelpers.SendLoginRequest(TestData.ValidEmail, TestData.ValidPassword);

            // Step 2: Validate login success
            ApiHelpers.ValidateSuccessfulLogin(responseContent);
        }

    }
}
