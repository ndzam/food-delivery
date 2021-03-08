namespace FoodDeliveryWebApi.Requests
{
    public class UserPostRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
