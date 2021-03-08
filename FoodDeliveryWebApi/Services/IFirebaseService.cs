using Firebase.Auth;
using Firebase.Database;

namespace FoodDeliveryWebApi.Services
{
    public interface IFirebaseService
    {
        FirebaseAuthProvider GetFirebaseAuthProvider();
        FirebaseClient GetFirebaseClient(string token);
        FirebaseClient GetFirebaseClient();
        string ConvertErrorCode(AuthErrorReason reason);
    }
}