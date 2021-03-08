namespace FoodDeliveryWebApi.Constants
{
    public static class ErrorCodes
    {
        public const string UNKNOWN_ERROR = "UNKNOWN_ERROR";
        public const string ACCOUNT_EXISTS = "ACCOUNT_EXISTS";
        public const string PASSWORDS_DONT_MATCH = "PASSWORDS_DONT_MATCH"; 
        public const string WRONG_ACCOUNT = "WRONG_ACCOUNT";
        public const string WRONG_PASSWORD = "WRONG_PASSWORD";
        public const string INVALID_EMAIL = "INVALID_EMAIL"; 
        public const string MISSING_PASSWORD = "MISSING_PASSWORD"; 
        public const string MISSING_EMAIL = "MISSING_EMAIL"; 
        public const string WEAK_PASSWORD = "WEAK_PASSWORD"; 
        public const string INVALID_TOKEN = "INVALID_TOKEN";
        public const string FORBIDEN = "FORBIDEN";
        public const string INVALID_NAME = "INVALID_NAME";
        public const string NOT_FOUND = "NOT_FOUND";
        public const string INVALID_ROLE = "INVALID_ROLE";
    }
}