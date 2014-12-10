namespace Photolane.Shared.Helper
{
    public static class Constants
    {
        #region azure mobile services

        public static string MobileServiceUrl = "https://radyalabs.azure-mobile.net/";
        public static string MobileServiceKey = "mOZGveIzwrvGzkjnTzVVdpMLNOjrzl81";

        #endregion

        #region local state

        public static string StateUserId = "userId";
        public static string StateUserToken = "userToken";

        #endregion

        #region GCM

        public const string SenderID = "1071223184347"; // Google API Project Number

        #endregion

        #region azure notification Hub

        public const string ConnectionString = "Endpoint=sb://radyalabshub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=fwK/qzykHaxDFRMkjlTn2PZI4e28UXOHUZGei6w3blY=";
        public const string NotificationHubPath = "radyalabshub";

        #endregion
    }
}