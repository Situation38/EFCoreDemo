
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EFCoreDemo;
using EFCoreDemo.Models;
using System;
using System.Linq;


namespace EFCoreDemoTest.Tests_Integrations
{
    //Création d'un serveur de test avec la classe WebApplicationFactory<T>
    public class TestingEFCore<T> : WebApplicationFactory<Startup>
    {

        //Notre classe implémente la classe WebApplicationFactory<Startup>
        //et remplace la méthode ConfigureWebHost
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //On supprime EFCoreContext de Startup.cs et ajouté la base de données en mémoire Entity Framework
                var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EFCoreDemoContext>));

                if (dbContext != null)
                    services.Remove(dbContext);

                var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();


                //*********************************************************************************************************


                //On ajoute la base de données en mémoire Entity Framework Core, en tant que service, à IServiceCollection
                services.AddDbContext<EFCoreDemoContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryEmployeeTest");
                    options.UseInternalServiceProvider(serviceProvider);
                });


                //********************************************************************************************************

                // On créé le fournisseur de services et la portée de ce service. Ceci est fait pour que le nouveau service
                // (base de données en mémoire Entity Framework Core) puisse être
                // fourni à une autre classe via Dependency Injection
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    using (var appContext = scope.ServiceProvider.GetRequiredService<EFCoreDemoContext>())
                    {
                        try
                        {
                            //garantit que la base de données du contexte existe. Si elle existe,
                            //aucune action n'est entreprise, si elle n'existe pas alors
                            //la base de données et tout son schéma sont créés
                            appContext.Database.EnsureCreated();
                        }
                        catch (Exception ex)
                        {
                            //Log errors
                            throw;
                        }
                    }
                }
            });
        }
    }
}

