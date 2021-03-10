using Firebase.Auth;
using Firebase.Database;
using FoodDeliveryWebApi.Configs;
using FoodDeliveryWebApi.Constants;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace FoodDeliveryWebApi.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseAuthProvider _firebaseAuthProvider;
        private readonly string _connectionString;
        

        public FirebaseService(IOptions<APIConfigs> options)
        {
            _firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(options.Value.ApiKey));
            _connectionString = options.Value.ConnectionString;
        }

        public FirebaseAuthProvider GetFirebaseAuthProvider()
        {
            return _firebaseAuthProvider;
        }

        public FirebaseClient GetFirebaseClient(string token)
        {
            return new FirebaseClient(_connectionString, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token)
            });
        }

        public FirebaseClient GetFirebaseClient()
        {
            return new FirebaseClient(_connectionString);
        }

        public FirestoreDb GetFirestoreDb()
        {
            var project = "food-delivery-66d65";
            FirestoreDb db = FirestoreDb.Create(project);
            return db;
        }
        

        public string ConvertErrorCode(AuthErrorReason reason)
        {
            switch (reason)
            {
                case AuthErrorReason.Undefined:
                    break;
                case AuthErrorReason.OperationNotAllowed:
                    break;
                case AuthErrorReason.UserDisabled:
                    break;
                case AuthErrorReason.UserNotFound:
                    break;
                case AuthErrorReason.InvalidProviderID:
                    break;
                case AuthErrorReason.InvalidAccessToken:
                    break;
                case AuthErrorReason.LoginCredentialsTooOld:
                    break;
                case AuthErrorReason.MissingRequestURI:
                    break;
                case AuthErrorReason.SystemError:
                    break;
                case AuthErrorReason.InvalidEmailAddress:
                    return ErrorCodes.INVALID_EMAIL;
                case AuthErrorReason.MissingPassword:
                    return ErrorCodes.MISSING_PASSWORD;
                case AuthErrorReason.WeakPassword:
                    return ErrorCodes.WEAK_PASSWORD;
                case AuthErrorReason.EmailExists:
                    return ErrorCodes.ACCOUNT_EXISTS;
                case AuthErrorReason.MissingEmail:
                    return ErrorCodes.MISSING_EMAIL;
                case AuthErrorReason.UnknownEmailAddress:
                    return ErrorCodes.WRONG_ACCOUNT;
                case AuthErrorReason.WrongPassword:
                    return ErrorCodes.WRONG_PASSWORD;
                case AuthErrorReason.TooManyAttemptsTryLater:
                    break;
                case AuthErrorReason.MissingRequestType:
                    break;
                case AuthErrorReason.ResetPasswordExceedLimit:
                    break;
                case AuthErrorReason.InvalidIDToken:
                    return ErrorCodes.INVALID_TOKEN;
                case AuthErrorReason.MissingIdentifier:
                    break;
                case AuthErrorReason.InvalidIdentifier:
                    break;
                case AuthErrorReason.AlreadyLinked:
                    break;
                default:
                    break;
            }
            return ErrorCodes.UNKNOWN_ERROR;
        }
    }
}