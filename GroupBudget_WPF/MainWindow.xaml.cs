using GroupBudget_WPF.Migrations;
using GroupBudget_WPF.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroupBudget_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            GB_Context context = App.Context;

            // Interpretation of Xaml
            InitializeComponent();
            //dgPersons.ItemsSource = context.Persons.Where(p => p.Deleted > DateTime.Now).ToList();
        }

        private void tiPeople_GotFocus(object sender, RoutedEventArgs e)
        {
            InitDgPersons();
        }


        private void dgPersons_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PersonDatagridViewModel person = (PersonDatagridViewModel)dgPersons.CurrentItem;
            if (person != null)
            {
                dgPersons.SelectedItem = person; 
                tbFirstName.Text = person.FirstName;
                tbLastName.Text = person.LastName;
                tbUserName.Text = person.Name;
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(tbFirstName.Text)
                || string.IsNullOrEmpty(tbLastName.Text)
                || string.IsNullOrEmpty(tbUserName.Text)))
            {
                Person p = App.Context.Persons.FirstOrDefault(p => p.Name == tbUserName.Text);
                if (p == null)
                {
                    Person person = new Person { FirstName = tbFirstName.Text, LastName = tbLastName.Text, Name = tbUserName.Text };
                    //App.Context.Persons.Add(person);   // You can assign specifically
                    App.Context.Add(person);
                    App.Context.SaveChanges();
                    InitDgPersons();
                }
            }
        }

        private void InitDgPersons()
        {
            dgPersons.ItemsSource = (from person in App.Context.Persons
                                     where person.Deleted > DateTime.Now
                                     orderby person.LastName, person.FirstName
                                     //select new { person.Id, person.Name, person.FirstName, person.LastName })
                                     select new PersonDatagridViewModel(person)
                                     ).ToList();
            dgPersons.Columns[0].Visibility = Visibility.Collapsed;
            dgPersons.Columns[1].Width = 100;
            dgPersons.Columns[2].Width = 200;
            dgPersons.Columns[3].Width = 200;
            dgPersons.Columns[2].Header = "First name";
            dgPersons.Columns[3].Header = "Last name";
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            Person person = App.Context.Persons.FirstOrDefault(p => p.Name == tbUserName.Text);
            //Person person = App.Context.Persons.FirstOrDefault(p => p.Id == ((Person)dgPersons.SelectedItem).Id);
            if (person != null)
            {
                person.FirstName = tbFirstName.Text;
                person.LastName = tbLastName.Text;
                App.Context.Update(person);
                App.Context.SaveChanges();
                InitDgPersons();

            }
        }
    }
}
