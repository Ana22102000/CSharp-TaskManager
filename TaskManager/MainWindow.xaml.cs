using System.Windows;
using TaskManager.ViewModel;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }


    }
}