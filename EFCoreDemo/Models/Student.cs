using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreDemo.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        
        public String Name { get; set; }


        [Required]
        
        public string Email { get; set; }

        public ICollection<Course> Courses { get; set;}
        public ICollection<EmailAddress> Emails { get; set; }

    }
}
