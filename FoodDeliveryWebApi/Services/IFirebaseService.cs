using Firebase.Auth;
using Firebase.Database;
using Google.Cloud.Firestore;

namespace FoodDeliveryWebApi.Services
{
    public interface IFirebaseService
    {
        FirebaseAuthProvider GetFirebaseAuthProvider();
        FirebaseClient GetFirebaseClient(string token);
        FirebaseClient GetFirebaseClient();
        FirestoreDb GetFirestoreDb();
        string ConvertErrorCode(AuthErrorReason reason);
    }
}