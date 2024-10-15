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
using GroupBudget_WPF.Resources;             // needed to acces resource strings

namespace GroupBudget_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GB_Context context = App.Context;

        public MainWindow()
        {
            // Interpretation of Xaml
            InitializeComponent();
            //dgPersons.ItemsSource = context.Persons.Where(p => p.Deleted > DateTime.Now).ToList();
        }

        private void tiPeople_GotFocus(object sender, RoutedEventArgs e)
        {
            if (dgPersons.ItemsSource == null)  // Only perform InitTiPeople when tiPeople got its first focus
            {
                tbSelecting.Text = "";
                InitTiPeople();
            }
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
                tbUserName.IsEnabled = false;
                btDelete.IsEnabled = true;
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbUserName.Text = "";
            tbUserName.IsEnabled = true;
            btDelete.IsEnabled = false;
        }

        private void InitTiPeople()
        {
            dgPersons.ItemsSource = (from person in App.Context.Persons
                                     where  person.Deleted > DateTime.Now
                                            && (tbSelecting.Text == ""
                                                || person.Name.Contains(tbSelecting.Text)
                                                || person.FirstName.Contains(tbSelecting.Text)
                                                || person.LastName.Contains(tbSelecting.Text))
                                     orderby person.LastName, person.FirstName
                                     select new PersonDatagridViewModel(person)
                                     ).ToList();
            dgPersons.Columns[0].Visibility = Visibility.Collapsed;
            dgPersons.Columns[1].Width = 100;
            dgPersons.Columns[2].Width = 200;
            dgPersons.Columns[3].Width = 200;
            dgPersons.Columns[2].Header = "First name";
            dgPersons.Columns[3].Header = "Last name";
            btSave.IsEnabled = false;
            tbUserName.IsEnabled = false;
            btDelete.IsEnabled = false;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            Person person = context.Persons.FirstOrDefault(p => p.Name == tbUserName.Text);
            if (person==null)  // no person found, so this person has to be added
                person = new Person();
            //Person person = App.Context.Persons.FirstOrDefault(p => p.Id == ((Person)dgPersons.SelectedItem).Id);
            if (person != null)
            {
                person.Name = tbUserName.Text;
                person.FirstName = tbFirstName.Text;
                person.LastName = tbLastName.Text;
                if (person.Id > 0)
                //if (!tbUserName.IsEnabled)      // valid alternative
                    context.Update(person);
                else
                    context.Add(person);
                context.SaveChanges();
                InitTiPeople();
            }
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(tbUserName.Text=="" || tbFirstName.Text=="" || tbLastName.Text==""))
            {
                btSave.IsEnabled = true;
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            string s = string.Format(Strings.ResourceManager.GetString("ConfirmDelete"),tbUserName.Text);
            if (MessageBox.Show(s, Strings.ResourceManager.GetString("Delete?"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Person person = App.Context.Persons.FirstOrDefault(p => p.Name == tbUserName.Text);
                person.Deleted = DateTime.Now;
                context.Update(person);
                context.SaveChanges();
                InitTiPeople();
            }
        }

        private void tbSelecting_KeyUp(object sender, KeyEventArgs e)
        {
            InitTiPeople();
        }
    }
}
