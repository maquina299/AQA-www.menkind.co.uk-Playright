
    namespace www.menkind.co.uk.Tests
    {
        public static class TestData
    {
        public static readonly string ValidEmail = "testuser_cd17ee00-86e6-4e48-acbd-cc5835d8be2b@example.com";
        public static readonly string ValidPassword = "TestPassword123!";
        //public static readonly string HomePageURL = "https://www.google.com";

        public static readonly string HomePageURL = "https://www.menkind.co.uk";
        public static readonly string RegistrationPageURL = "https://www.menkind.co.uk/login.php?action=create_account";
        public static readonly string ProductPageURL = "https://www.menkind.co.uk/draft-wizard-ultrasonic-frothing-beer-dispenser";
        public static readonly string AddedToTheCardProductId = "33709";
        public static readonly string SoldProductPageURL = "https://www.menkind.co.uk/man-points-scratch-off-game";
        public static readonly string GetCartSummary = "https://www.menkind.co.uk/api/storefront/cart-summary";
        public static readonly string SearchQuery = "massage";//gift (581 in filter), massage (88), beer
        public static readonly List<string> ValidResultTitles = new List<string>
        {
        "Top 100 Bestsellers".ToLower(),
        "Gadgets & Tech Gifts".ToLower(),
        "Gifts For Men".ToLower()
        };
        public static readonly string SearchPageURL = $"https://www.menkind.co.uk/search.php?search_query={SearchQuery}";


#region API Data    
        public static readonly string ApiLoginURL = "https://www.menkind.co.uk/login.php?action=check_login";
        public static readonly string SuccesfullyLoggedInPageHeader = "<h1 class=\"page-heading\">Orders</h1>";
        //public static readonly string UnsuccesfullyLoggedInPageHeader = "<h1 class=\"page-heading\">Sign In</h1>";




        #endregion
    }
}
