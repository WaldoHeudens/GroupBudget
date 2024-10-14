using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBudget_WPF.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Nick name
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Deleted { get; set; } = DateTime.MaxValue;

    }

    public class PersonProject
    {
        public int Id { get; set; }

        [ForeignKey ("Person")]
        public int PersonId { get; set; }

        [ForeignKey ("Project")]
        public int ProjectId { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public DateTime Deleted { get; set; } = DateTime.MaxValue;
    }
}
