using TaskManager.Tools;
using System.Windows;
using TaskManager.Tools.Managers;
using TaskManager.Tools.Navigation;

namespace TaskManager.ViewModel
{
    internal class MainWindowViewModel : BaseViewModel, ILoaderOwner, IContentOwner
    {

        #region Fields
        private Visibility _loaderVisibility = Visibility.Hidden;
        private bool _isControlEnabled = true;
        private INavigatable _content;
        #endregion

        #region Properties
        public INavigatable Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }
        public Visibility LoaderVisibility
        {
            get { return _loaderVisibility; }
            set
            {
                _loaderVisibility = value;
                OnPropertyChanged();
            }
        }
        public bool IsControlEnabled
        {
            get { return _isControlEnabled; }
            set
            {
                _isControlEnabled = value;
                OnPropertyChanged();
            }
        }
        #endregion


        internal MainWindowViewModel()
        {
            LoaderManager.Instance.Initialize(this);
            NavigationManager.Instance.Initialize(new AuthenticationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.Task);

        }

    }
}