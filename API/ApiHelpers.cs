using System.Text;
using NUnit.Framework.Constraints;

using www.menkind.co.uk.Tests;

namespace www.menkind.co.uk.Helpers
{
    public static class ApiHelpers
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> SendLoginRequest(string email, string password)
        {
            var postData = new StringContent(
                $"login_email={email}&login_pass={password}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            // Add headers
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
            client.DefaultRequestHeaders.Add("referer", "https://www.menkind.co.uk/login.php");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

            // Send POST request
            var response = await client.PostAsync(TestData.ApiLoginURL, postData);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Ensure the request didn't fail at a network level
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Login request failed");

            return responseContent;
        }

        /// <summary>
        /// Validates if the login was successful based on response content.
        /// </summary>
        public static void ValidateSuccessfulLogin(string responseContent)
        {
            Assert.That(responseContent, Does.Contain(TestData.SuccesfullyLoggedInPageHeader),
                "Login failed - 'Orders' page is not displayed.");
        }
    }
}
