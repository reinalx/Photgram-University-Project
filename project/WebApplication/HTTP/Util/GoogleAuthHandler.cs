using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Es.Udc.DotNet.PracticaMaD.Web.Properties;
using Newtonsoft.Json;

namespace Es.Udc.DotNet.PracticaMaD.Web.HTTP.Util
{

    public class GoogleAuthHandler
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<(string accessToken, string idToken)> ExchangeCodeForTokens(string authorizationCode)
        {
            // Configura los parámetros para la solicitud POST
            var tokenEndpoint = "https://oauth2.googleapis.com/token";
            var clientId = Settings.Default.PracticaMaD_googleClientId;
            var clientSecret = Settings.Default.PracticaMaD_googleClientSecret;
            var redirectUri = "https://localhost:44384/Pages/User/SigninGoogle.aspx";

            // Configura los datos del formulario para la solicitud POST
            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", authorizationCode),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            // Realiza la solicitud POST
            var response = await httpClient.PostAsync(tokenEndpoint, formData);

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenData = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            // Devuelve el token de acceso y el token de identificación
            return (tokenData.access_token, tokenData.id_token);
        }

        // Clase para deserializar la respuesta JSON
        private class TokenResponse
        {
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public int expires_in { get; set; }
            public string id_token { get; set; }

            // Otros campos si es necesario
        }
    }
}
