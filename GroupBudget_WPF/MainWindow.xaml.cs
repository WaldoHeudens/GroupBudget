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


        private void dgPersons_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PersonDatagridViewModel person = (PersonDatagridViewModel)dgPersons.CurrentItem;
            if (person != null)
            {
                int id = person.Id;
            }
        }
    }
}
