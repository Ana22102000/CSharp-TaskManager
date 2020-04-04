using TaskManager.Tools.Navigation;
using TaskManager.ViewModel;

namespace TaskManager.View
{
    /// <summary>
    /// Логика взаимодействия для ThreadsView.xaml
    /// </summary>
    public partial class ThreadsView : INavigatable
    {
        public ThreadsView()
        {
            InitializeComponent();
            ThreadsViewModel _threadsViewModel = new ThreadsViewModel();
            DataContext = _threadsViewModel;
        }
    }
}
