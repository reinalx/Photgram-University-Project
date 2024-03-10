using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Util;
using Es.Udc.DotNet.PracticaMaD.Web.Properties;
using Newtonsoft.Json;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.User
{
    public partial class SigninGoogle : SpecificCulturePage
    {
        /// <exception cref="DuplicateInstanceException"/>

        protected async void Page_Load(object sender, EventArgs e)
        {
            string code = Request.Params.Get("code");
            if (code == null)
            {
                var clientId = Settings.Default.PracticaMaD_googleClientId;
                var redirectUri = "https://localhost:44384/Pages/User/SigninGoogle.aspx";
                string googleLoginUrl = "https://accounts.google.com/o/oauth2/auth" +
                                        "?client_id=" + clientId +
                                        "&redirect_uri=" + redirectUri +
                                        "&response_type=code" +
                                        "&scope=openid%20profile%20email";

                Response.Redirect(googleLoginUrl, false);

                // Limpia el contenido del búfer actual para asegurarte de que se envíe la redirección al navegador
                Response.Flush();

                // Finaliza el hilo actual
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            else
            {

                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IUserService userService = ioCManager.Resolve<IUserService>();


                GoogleAuthHandler googleAuthHandler = new GoogleAuthHandler();
                var tokenResponse = await googleAuthHandler.ExchangeCodeForTokens(code);
                string accessToken = tokenResponse.accessToken;
                string idToken = tokenResponse.idToken;

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new Exception("Token de acceso no válido o vacío.");
                }

                var UserProfileGoogle = await GetGoogleUserProfile(accessToken);

                // Aquí puedes utilizar la información del perfil del usuario, por ejemplo, para el inicio de sesión en tu aplicación.

                string loginName = UserProfileGoogle.Name;
                string firstName = UserProfileGoogle.Given_name;
                string lastName = UserProfileGoogle.Family_name;
                string email = UserProfileGoogle.Email;
                string language = UserProfileGoogle.Locale;
                string country = UserProfileGoogle.Locale;

                UserProfileDetails userProfileDetailsVO =
                    new UserProfileDetails(firstName, lastName,
                        email, language,
                        country);
                //tengo que hacerlo con register pero primero que funcione solo iniciar sesion
                try
                {

                    SessionManager.RegisterUser(Context, loginName, GetRamdonPassword(15), userProfileDetailsVO);
                    FormsAuthentication.RedirectFromLoginPage(loginName,
                        false);

                }
                catch (DuplicateInstanceException)
                {
                    UserProfile userProfile = userService.GetProfile(loginName);

                    SessionManager.Login(Context, userProfile.loginName,
                        userProfile.enPassword, false, true);

                    FormsAuthentication.RedirectFromLoginPage(userProfile.loginName,
                        false);
                }
            }

        }
        private async Task<UserProfileGoogle> GetGoogleUserProfile(string accessToken)
        {
            // URL de la API de Google para obtener información del perfil del usuario
            string userProfileEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";

            using (HttpClient client = new HttpClient())
            {
                // Agrega el token de acceso al encabezado de la solicitud
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Realiza la solicitud GET para obtener información del perfil del usuario
                HttpResponseMessage response = await client.GetAsync(userProfileEndpoint);

                // Verifica si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Lee y deserializa la respuesta JSON a un objeto UserProfile
                    string userProfileJson = await response.Content.ReadAsStringAsync();
                    UserProfileGoogle userProfileGoogle = JsonConvert.DeserializeObject<UserProfileGoogle>(userProfileJson);

                    return userProfileGoogle;
                }
                else
                {
                    // Manejo de error
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error al obtener el perfil del usuario. Estado: {response.StatusCode}. Contenido del error: {errorContent}. Token de acceso: {accessToken}");
                }
            }
        }

        // Clase para deserializar la respuesta JSON del perfil del usuario
        public class UserProfileGoogle
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Given_name { get; set; } // Nombre
            public string Family_name { get; set; } // Apellido
            public string Email { get; set; }
            public string Locale { get; set; } // Idioma y país

        }

        static string GetRamdonPassword(int longitud)
        {
            const string caracteresPosibles = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] arrayAleatorio = new char[longitud];

            for (int i = 0; i < longitud; i++)
            {
                int indiceAleatorio = random.Next(caracteresPosibles.Length);
                arrayAleatorio[i] = caracteresPosibles[indiceAleatorio];
            }

            return new string(arrayAleatorio);
        }
    }
}