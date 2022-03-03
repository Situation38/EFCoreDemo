


using EFCoreDemo;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EFCoreDemoTest.Tests_Integrations
{
    [TestFixture]//Cela indique que la classe a des méthodes de test.
    public class EFCoreControllerTests
    {
        private const string RequestUri = "https://localhost:44302/";  //Pour acceder a notre api


        //Test avec une classe heritant de la classe Test Server
        //************************************************************************************************
        //On teste l'acces à l'url de notre api.
        


        [Test]
        public async Task Get_EFCoreDemoAcceuil_Test()
        {
            //ARRANGE
            //On cree un point de fournisseur de client de test
            using (var client = new TestClientProvider().Client)
            {

                //ACT
                //On recupere la reponse face a la requete du client pour l'acces a l'Url
                //et le code detat de la reponse
                var response = await client.GetAsync(requestUri: RequestUri);
                response.EnsureSuccessStatusCode();

                //ASSERT
                //On verifie si le code de la reponse est egale a OK
                //Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                response.StatusCode.Should().Be(HttpStatusCode.OK);//(Grace au package Fluent assertion)

            }


        }


        //Test avec une classe heritant de la classe WebApplicationFactory
        //Nous declarons ces variables ici pour pouvoir réutiliser leurs instances sur plusieurs tests

        private TestingEFCore<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]//La méthode est éclenchée une seule fois avant le début des cas de test
        public void OneTimeSetUp()
        {

            // ARRANGE
            //nous en créons une instance pointant vers la classe Startup
            //de notre application EFCore
            _factory = new TestingEFCore<Startup>();
            
        }

        [SetUp]// est Déclenché avant chaque scénario de test
        public void Setup()
        {
            //Creation de l'instance HttpClient que nous utiliserons pour émettre les requêtes HTTP
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task ShouldReturnExpectedText()
        {
            //Dans cette methode de test
            //on s'assure que lorsque nous faisons une requête à l'application Web, nous recevons le texte attendu.

            //ACT
            var result = await _client.GetStringAsync("/");
            
            //ASSERT
            Assert.That(result, Is.EqualTo("All was good!"));
        }

        [Test]
        public async Task Returns200()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/");

            using var response = await _client.SendAsync(request);

            //ASSERT
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }






        //************************************************************************************************
        //On teste l'action Index (Affichage) (HTTP GET) du contrôleur Student.
        


        [Test]
        public async Task Index_GET_Action()
        {
            // Act
            //On appelle l'action Index de studentcontroller pour consulter la liste des students en
            //faisant une requête GET à son URL qui est /Register/Read
            var response = await _client.GetAsync("https://localhost:44302/Students/");
            var forText = "<h1>List of student</h1>";//chaine a verifier

            // Assert

            //on teste avec le EnsureSuccessStatusCode pour vérifier si l'appel réussit ou non
            //Le test réussira s'il renvoie vrai et échouera s'il renvoie 
            response.EnsureSuccessStatusCode();

            //La réponse HTTP qui est sérialisée en une chaîne est stockée dans la variable "responseString"    
            var responseString = await response.Content.ReadAsStringAsync();

            //on vérifie que le type de contenu des en-têtes HTTP est text/html ; jeu de caractères=utf-8 
            //Ceci est fait par la ligne de code ci-dessous.
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            
            //je verifie que la chaine forText est contenu dans responeString
            //La variable forText contient une balise H1 avec un texte appelé "Create".
            var boul = responseString.Contains(forText);
            //Je verifie que en sortie la methode contains nous renvoie bien true
            Assert.That(boul, Is.EqualTo(true)); ;

        }



        //************************************************************************************************
        //On teste l'action Create (HTTP GET) du contrôleur Student.
        //on Ajoute donc la méthode de test appelée Create_GET_Action()

        [Test]
        public async Task Create_GET_Action()
        {

            // Act
            var response = await _client.GetAsync("https://localhost:44302/Students/Create/");
            var forText = "<h1>Create</h1>";

            //on teste avec le EnsureSuccessStatusCode pour vérifier si l'appel réussit ou non
            //Le test réussira s'il renvoie vrai et échouera s'il renvoie 
            response.EnsureSuccessStatusCode();

            //La réponse HTTP qui est sérialisée en une chaîne est stockée dans la variable "responseString"    
            var responseString = await response.Content.ReadAsStringAsync();

            //on vérifie que le type de contenu des en-têtes HTTP est text/html ; jeu de caractères=utf-8 
            //Ceci est fait par la ligne de code ci-dessous.
            Assert.AreEqual("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
            //je verifie que la chaine forText est contenu dans responeString
            //La variable forText contient une balise H1 avec un texte appelé "Create".
            var boul = responseString.Contains(forText);
            //Je verifie que en sortie la methode contains nous renvoie bien true
            Assert.That(boul, Is.EqualTo(true)); ;


        }

        //************************************************************************************************
        //Test Lorsque l'utilisateur remplit incomplètement le formulaire de cration et essaie de le soumettre
        //Cela conduit à un test de modèle invalide 
        [Test]
        public async Task Create_POST_Action_InvalidModel()
        {
            // Arrange
            //oN crée un message de requête HTTP dans lequel est spécifié l'URL de cette action et le type HTTP à "Post
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44302/Students/Create/");
            var forText = "The Email field is required.";


            //On créé un modèle de formulaire qui est un type de dictionnaire
            var RegisterModel = new Dictionary< string, string>
            {
                
                { "Name", "Person" },
                { "Email", "" }

            };


            //l'action Create est initiée et la valeur du modèle lui est fournie

            postRequest.Content = new FormUrlEncodedContent(RegisterModel);

            // Act
            var response = await _client.SendAsync(postRequest);

            // Assert
            //On sérialise la réponse et vérifie l'assertion
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            
            var boul = responseString.Contains(forText);
            //Je verifie que en sortie la methode contains nous renvoie bien true
            Assert.That(boul, Is.EqualTo(true));
            


        }



        //****************************************************************************
        //Test Lorsque l'utilisateur remplit  le formulaire de creation et essaie de le soumettre
        //Sans erreurs dans le replissage du formulaire

        [Test]
        public async Task Create_POST_Action_ValidModel()
        {
            // Arrange
            //On crée un message de requête HTTP dans lequel on spécifié l'URL de cette action et le type HTTP à "Post"
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44302/Students/Create/");
            string [] val = { "New Person", "personnew@mail.com" };


            // On créé un modèle de formulaire qui est un type de dictionnaire
            var StudentModel = new Dictionary< string, string>

            {
              { "Name", "New Person"},
                { "Email", "personnew@mail.com"}
            };

           
            postRequest.Content = new FormUrlEncodedContent(StudentModel);

            // Act
             //On initie l'action Créer  et la valeur du modèle lui est fournie
            
            var response = await _client.SendAsync(postRequest);

            // Assert
            //On seialise la reponse et verifie l'assertion
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var boul = responseString.Contains(val[0]);
            var boul1 = responseString.Contains(val[1]);
            Assert.That(boul, Is.EqualTo(true));
            Assert.That(boul1, Is.EqualTo(true));
        
        
        }

       


    }


}







