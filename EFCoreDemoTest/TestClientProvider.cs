 using EFCoreDemo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Text;


namespace EFCoreDemoTest
{
    public class TestClientProvider
    {

        public HttpClient Client { get; private set; }
        public TestClientProvider()
        {
            //On instancie une variable de type serveur pour la creation
            // de notre serveur en fournissant un nouveau cosntructeur d'hote web
            var server = new TestServer(new WebHostBuilder().UseStartup<Startup>());



            //On cree un clientHttp grace a la methode CreateClient() de la classse serveur
            Client = server.CreateClient();



        }
 
    }




}

