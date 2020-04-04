using TaskManager.Tools.Navigation;
using TaskManager.ViewModel;

namespace TaskManager.View
{
    /// <summary>
    /// Логика взаимодействия для ModulesView.xaml
    /// </summary>
    public partial class ModulesView : INavigatable
    {
        public ModulesView()
        {
            InitializeComponent();
            ModulesViewModel _modulesViewModel = new ModulesViewModel();
            DataContext = _modulesViewModel;
        }
    }
}
