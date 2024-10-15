using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBudget_WPF.Models
{
    internal class PersonDatagridViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PersonDatagridViewModel(Person person)
        {
            Id = person.Id;
            Name = person.Name;
            FirstName = person.FirstName;
            LastName = person.LastName;
        }
    }
}
