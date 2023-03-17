namespace QueueBreaker_UI.WASM.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "https://localhost:44326/";
        public static string LoginEndpoint = $"{BaseUrl}api/user/authenticate/";
        public static string AuthorsEndpoint = $"{BaseUrl}api/authors/";
        public static string CanteenEndpoint = $"{BaseUrl}api/canteen/";
        public static string CategoryEndpoint = $"{BaseUrl}api/category/"; 
        public static string BooksEndpoint = $"{BaseUrl}api/books/";
        public static string UserEndpoint = $"{BaseUrl}api/user/";
        public static string RegisterEndpoint = $"{BaseUrl}api/user/";
        public static string ItemEndpoint = $"{BaseUrl}api/items/"; 
        public static string OrderItemEndpoint = $"{BaseUrl}api/OrderItems/";
        public static string CartEndpoint = $"{BaseUrl}api/cart/";
        public static string UserProfileEndpoint = $"{BaseUrl}api/users/";
        public static string CurrentUserProfileEndpoint = $"{BaseUrl}api/userprofile/CurrentUserProfile";
        public static string OrderplaceEndpoint = $"{BaseUrl}api/Order/";


    }
}
