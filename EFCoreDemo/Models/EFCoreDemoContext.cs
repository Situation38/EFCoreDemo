using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreDemo.Models
{
    public class EFCoreDemoContext : DbContext
    {

        public EFCoreDemoContext(DbContextOptions<EFCoreDemoContext> options) : base (options)
        {


        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Course>Courses { get; set; }

       protected override  void OnModelCreating(ModelBuilder modelBuilder)
        {
            //on ajoute des attributs a la propriete name de la classe course (fluentApi)
            modelBuilder.Entity<Course>()
                    .Property(c => c.Name)
                    .HasMaxLength(100)
                    .IsRequired();


        }



    }
}
