using System.Configuration;
using System.Data;
using System.Windows;

namespace GroupBudget_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public GB_Context Context { get; private set; }

        public App()
        {
            // Prepare the DbContext
            Context = new GB_Context();
            new Seeder(Context);
        }

    }

}
