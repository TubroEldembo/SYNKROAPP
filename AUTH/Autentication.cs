using Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SYNKROAPP.AUTH
{
    public class Autentication
    {
        private static FirebaseApp firebaseApp = InitializeFirebase();
        private static FirebaseApp InitializeFirebase()
        {
            var credential = GoogleCredential.FromFile("credencials.json");
            return FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
            });
        }
        private static string apiKey = "AIzaSyApcm4sO8-2QxgUxAUZu2j9PSLjtu9snWo";

        public static async Task<FirebaseAuthLink> SignIn(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);
            return auth;
        }

        public static async Task<FirebaseAuthLink> SignUp(string email, string password)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
            try
            {
                FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
                return auth;
            }
            catch (Firebase.Auth.FirebaseAuthException ex)
            {
                Console.WriteLine($"Error de autenticación: {ex.Reason}");
                throw;
            }
        }
        public async static void SignOut(Firebase.Auth.FirebaseAuth auth)
        {
            if (auth.User != null)
            {
                auth = null;
            }
        }

        #region MODIFICAR DADES USUARI
        public static async Task ChangeUserPasswordAsync(FirebaseAuthLink auth, string newPassword)
        {
            try
            {
                if (auth.User == null)
                {
                    throw new Exception("Usuari no autenticat");
                }
                var user = auth.User;
                string uid = user.LocalId;
                UserRecordArgs userArgs = new UserRecordArgs
                {
                    Uid = uid,
                    Password = newPassword,
                };
                await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(userArgs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al canviar la contrasenya: {ex.Message}");
            }
        }
        public static async Task ChangeUserNameAsync(FirebaseAuthLink auth, string newUserName)
        {
            try
            {
                if (auth.User == null)
                {
                    throw new Exception("Usuari no autenticat");
                }
                var user = auth.User;
                string uid = user.LocalId;
                UserRecordArgs userArgs = new UserRecordArgs
                {
                    Uid = uid,
                    DisplayName = newUserName,
                };
                await FirebaseAdmin.Auth.FirebaseAuth.DefaultInstance.UpdateUserAsync(userArgs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al canviar el nom d'usuari: {ex.Message}");
            }
        }
        #endregion

        #region RECUPERAR PASSWORD
        public static async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                await authProvider.SendPasswordResetEmailAsync(email);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo de recuperación: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
