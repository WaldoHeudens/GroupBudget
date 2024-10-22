using GroupBudget_WPF.Migrations;
using GroupBudget_WPF.Models;
using GroupBudget_WPF.Resources;    // needed to acces resource strings
using Microsoft.EntityFrameworkCore;
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
        GB_Context context = App.Context;
        Boolean textChanged = false;
        List<Project> projectList { get; set; } = new List<Project>();    // Contains the datasource of the Listbox
        Project selectedProject { get; set; } = null;

        ToolTip toolTip = new ToolTip();   // use only this tooltip instead of the control's tooltip


        public MainWindow()
        {
            // Interpretation of Xaml
            InitializeComponent();
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
            dgPersons.Items.Clear();
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
            // Next block in comment has been moved to tb_TextLostFocus(sender, e)
            //if (textChanged)
            //{
            //    // Example of using sender
            //    //tbFirstName.Background = new SolidColorBrush(Colors.White);
            //    //tbLastName.Background = new SolidColorBrush(Colors.White);
            //    //tbUserName.Background = new SolidColorBrush(Colors.White);
            //    //((TextBox)sender).Background = new SolidColorBrush(Colors.Gray);

            //    if (!(tbUserName.Text == "" || tbFirstName.Text == "" || tbLastName.Text == ""))
            //    {
            //        btSave.IsEnabled = true;
            //    }
            //    textChanged = false;
            //}
            textChanged = true;
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {

            string s = string.Format(Strings.ResourceManager.GetString("ConfirmDelete"),tbUserName.Text);
            if (System.Windows.MessageBox.Show(s, Strings.ResourceManager.GetString("Delete?"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

        private void tiProjects_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!lbProjectSelect.HasItems)
            {
                InitTiProjects();
            }
        }

        private void InitTiProjects()
        {
            //projectList = (from project in context.Projects
            //               where project.Deleted > DateTime.Now
            //                   && (project.Name.Contains(tbProjectSelect.Text) || tbProjectSelect.Text == "")
            //               orderby project.Name
            //               select project).ToList();

            projectList = context.Projects
                            .Where(p => p.Deleted > DateTime.Now
                                    && (tbProjectSelect.Text == "" || p.Name.Contains(tbProjectSelect.Text)))
                            .OrderBy(p => p.Name)
                            .Include(p => p.ProjectPersons)
                                .ThenInclude(pp => pp.Person)
                            .ToList();

            lbProjectSelect.ItemsSource = (from project in projectList
                                            select new ListBoxItem {Content= project.Name + "   - " + (project.Description.Length > 30 ? project.Description.Substring(0, 30) + " ..." : project.Description) });
            
            // add a MouseEnter event to every single listbox item
            foreach (ListBoxItem item in lbProjectSelect.Items)
            {
                item.MouseEnter += new MouseEventHandler(lbProjectSelectItem_MouseEnter);
            }
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textChanged)
            {
                // Example of using sender
                //tbFirstName.Background = new SolidColorBrush(Colors.White);
                //tbLastName.Background = new SolidColorBrush(Colors.White);
                //tbUserName.Background = new SolidColorBrush(Colors.White);
                //((TextBox)sender).Background = new SolidColorBrush(Colors.Gray);

                if (!(tbUserName.Text == "" || tbFirstName.Text == "" || tbLastName.Text == ""))
                {
                    btSave.IsEnabled = true;
                }
                textChanged = false;
            }
        }

        //private void dgProjects_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    Project project = projectList[((ListBox)sender).SelectedIndex];
        //    tbProjectStartDate.Text = project.StartDate.ToString("dd/mm/jj");
        //    tbProjectName.Text = project.Name;
        //    tbProjectDescription.Text = project.Description;
        //    lbMembers.ItemsSource = (from member in project.ProjectPersons select member.Person.Name).ToList();
        //}

        private void lbProjectSelectItem_MouseEnter(object sender, MouseEventArgs e)
        {
            lbProjectSelect.ToolTip = null;

            ListBoxItem item = (ListBoxItem)sender;
            int index = lbProjectSelect.Items.IndexOf(sender);
            if (projectList[index].Description.Length > 30)
            {
                lbProjectSelect.ToolTip = projectList[index].Description;
            }
        }

        private void lbProjectSelect_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            // no tooltip should be shown if the tooltip == null
            e.Handled = ((ListBox)sender).ToolTip == null;
        }

        private void lbProjectSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProject = projectList[((ListBox)sender).SelectedIndex];
            tiProjects.DataContext = selectedProject;
        }

        private void tbProjectSelect_TextChanged(object sender, TextChangedEventArgs e)
        {
            InitTiProjects();
        }
    }
}
