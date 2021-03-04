namespace FoodDeliveryWebApi.Response
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }
    }
}
