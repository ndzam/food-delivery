using Firebase.Auth;
using Google.Cloud.Firestore;

namespace FoodDeliveryWebApi.Services
{
    public interface IFirebaseService
    {
        FirebaseAuthProvider GetFirebaseAuthProvider();
        FirestoreDb GetFirestoreDb();
        string ConvertErrorCode(AuthErrorReason reason);
    }
}