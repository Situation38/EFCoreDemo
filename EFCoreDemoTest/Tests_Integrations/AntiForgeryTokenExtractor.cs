
//Le jeton anti-contrefaçon peut être utilisé pour aider à protéger
//notre application contre la falsification des requêtes intersites

//Je crée une nouvelle classe appelée AntiForgeryTokenExtractor.cs
//pour mon projet de test d'integration ,ayant pour objectif 
//d' extraire le cookie et le champ anti-contrefaçon

using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFCoreDemoTest.Tests_Integrations
{
    public static class AntiForgeryTokenExtractor
    //Une exception est levée si elle n'est pas trouvée.
    //Cette méthode récupère la valeur de la propriété Set-Cookie
    //à partir de l'en-tête de notre réponse
    //Il lève une exception si le cookie n'est pas présent.

    {
        public static string Field { get; } = "AntiForgeryTokenField";
        public static string Cookie { get; } = "AntiForgeryTokenCookie";

        private static string ExtractCookieValue(HttpResponseMessage response)
        {
            string antiForgeryCookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault(x => x.Contains(Cookie));

            if (antiForgeryCookie is null)
                throw new ArgumentException($"Cookie '{Cookie}' not found in HTTP response", nameof(response));

            string antiForgeryCookieValue = SetCookieHeaderValue.Parse(antiForgeryCookie).Value.ToString();

            return antiForgeryCookieValue;
        }

        private static string ExtractAntiForgeryToken(string htmlBody)
        ////Cette méthode utilise l'expression regex pour extraire le contrôle HTML de la chaîne html Body
        //qui contient la valeur du champ anti-falsification
       
        {
            var requestVerificationTokenMatch = Regex.Match(htmlBody, $@"\<input name=""{Field}"" type=""hidden"" value=""([^""]+)"" \/\>");
            if (requestVerificationTokenMatch.Success)
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            throw new ArgumentException($"Anti forgery token '{Field}' not found", nameof(htmlBody));
        }

        public static async Task<(string field, string cookie)> ExtractAntiForgeryValues(HttpResponseMessage response)
        //Cette méthode collecte les résultats des 2 méthodes
        //ci-dessus et les renvoie sous la forme d'un objet Tuple
        
        {
            var cookie = ExtractCookieValue(response);
            var token = ExtractAntiForgeryToken(await response.Content.ReadAsStringAsync());

            return (token, cookie);
        }




    }
}



