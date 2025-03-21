using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using www.menkind.co.uk.Tests;
using Cookie = System.Net.Cookie;

public static class ApiHelpers
{
    private static readonly Logger Logger = DriverFactory.GetLogger();
    private static readonly HttpClient client = new HttpClient();
    private static readonly CookieContainer cookieContainer = new CookieContainer();


    public static async Task<string> SendApiRequest(
        string endpoint,
        HttpMethod method = null,
        object body = null,
        string cookie = null)
    {
        method ??= HttpMethod.Get;
        string url = TestData.ApiBaseURL + endpoint;

        Logger.Info($"🔄 Sending {method} request to: {url}"); // [CHANGED] Log request details

        using var request = new HttpRequestMessage(method, url);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

        if (body != null && (method == HttpMethod.Post || method == HttpMethod.Put))
        {
            string jsonBody = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            Logger.Debug($"📤 Request Body: {jsonBody}"); // [CHANGED] Log request body
        }

        if (!string.IsNullOrEmpty(cookie))
        {
            request.Headers.Add("Cookie", cookie);
            Logger.Debug($"🍪 Using Cookie: {cookie}"); // [CHANGED] Log cookie usage
        }

        try
        {
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            Logger.Info($"Received Response: {response.StatusCode}"); // [CHANGED] Log response status
            //Logger.Debug($"Response Body: {responseContent}"); // [CHANGED] Log full response

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
                $"API request failed - Expected 200 OK but got {response.StatusCode}.");

            return responseContent;
        }
        catch (Exception ex)
        {
            Logger.Error($"❌ API Request failed: {ex.Message}"); // [CHANGED] Log errors
            throw;
        }
    }

    public static async Task<(string responseContent, string cookies)> SendLoginRequest(string email, string password)
    {
        Logger.Info("Sending Login Request...");

        var postData = new StringContent(
            $"login_email={email}&login_pass={password}",
            Encoding.UTF8,
            "application/x-www-form-urlencoded"
        );

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("accept", "text/html,application/xhtml+xml,application/xml;q=0.9");
        client.DefaultRequestHeaders.Add("referer", "https://www.menkind.co.uk/login.php");
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        try
        {
            var response = await client.PostAsync(TestData.ApiLoginURL, postData);
            var responseContent = await response.Content.ReadAsStringAsync();
            //string cookies = string.Join("; ", cookieContainer.GetCookies(new Uri(TestData.ApiLoginURL))
            //    .Cast<Cookie>().Select(c => $"{c.Name}={c.Value}"));
            // Capture cookies from response headers
            var cookies = response.Headers.Contains("Set-Cookie") ? string.Join("; ", response.Headers.GetValues("Set-Cookie")) : string.Empty;
            /*var cookies = response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders)
            ? string.Join("; ", cookieHeaders)
            : string.Empty;
            Logger.Info($"Login Response: {response.StatusCode}");*/
            Logger.Info($"Got cookie: {cookies}");

            //Logger.Debug($"Response Body: {responseContent}");

            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Login request failed");

            return (responseContent, cookies);  // Return both responseContent and cookies
        }
        catch (Exception ex)
        {
            Logger.Error($"❌ Login Request failed: {ex.Message}");
            throw;
        }
    }


    public static void ValidateSuccessfulLogin(string responseContent)
    {
        Logger.Info("🔍 Validating Login Success..."); // [CHANGED] Log validation start

        Assert.That(responseContent, Does.Contain(TestData.SuccesfullyLoggedInPageHeader),
            "Login failed - 'Orders' page is not displayed.");

        Logger.Info("✅ Login Validation Passed!"); // [CHANGED] Log success
    }

    public static async Task<string> GetCartSummaryResponse(string cookies = null)
    {
        Logger.Info("Fetching Cart Summary...");

        using var request = new HttpRequestMessage(HttpMethod.Get, TestData.CartSummaryAPI);
        client.DefaultRequestHeaders.Clear();

        if (cookies != null)
        {
            client.DefaultRequestHeaders.Add("Cookie", cookies);  // Only add cookies if they are not null
            Logger.Info($"Added cookie to the cart-summary request:{cookies}");
        }

        client.DefaultRequestHeaders.Add("accept", "application/json");
        client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");

        try
        {
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            Logger.Info($"Cart Summary Response: {response.StatusCode}");
            //Logger.Debug($"Response Body: {responseContent}");

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK),
                $"API request failed - Expected 200 OK but got {response.StatusCode}.");

            return responseContent;
        }
        catch (Exception ex)
        {
            Logger.Error($"Cart Summary Request failed: {ex.Message}");
            throw;
        }
    }


    public static void ValidateCartSummaryStructure(string responseContent)
    {
        Logger.Info("🔍 Validating Cart Summary JSON Structure..."); // [CHANGED] Log validation start

        JToken jsonToken;
        Logger.Info($"responseContent: {responseContent}");
        try
        {
            jsonToken = JToken.Parse(responseContent);
        }
        catch (JsonReaderException ex)
        {
            Logger.Error($"❌ Invalid JSON response: {ex.Message}"); // [CHANGED] Log JSON errors
            throw new Exception($"Invalid JSON response: {ex.Message}");
        }

        if (jsonToken is JArray jsonArray)
        {
            Assert.That(jsonArray.Count, Is.EqualTo(0), "Cart summary response is an empty array.");
            //jsonToken = jsonArray.First;
            Logger.Debug("📂 Response is an array, using first object."); // [CHANGED] Log array handling
        }

        else if (jsonToken is JObject cartSummary)
        {
            Assert.Multiple(() =>
            {
                foreach (var key in TestData.ExpectedCartSummaryKeys)
                {
                    Assert.That(cartSummary.ContainsKey(key), $"Missing '{key}' field.");
                }
            });
            Logger.Info("✅ JSON structure validated successfully."); // [CHANGED] Log success
        }
        else
        {
            Logger.Error("❌ Cart summary response is neither an object nor a valid array."); // [CHANGED] Log failure
            throw new Exception("Cart summary response is neither an object nor a valid array.");
        }
    }
}
