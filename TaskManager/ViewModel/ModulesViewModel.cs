using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskManager.Model;
using TaskManager.Tools;
using TaskManager.Tools.Managers;
using TaskManager.Tools.Navigation;

namespace TaskManager.ViewModel
{
    class ModulesViewModel
    {

        #region Fields

        private static ObservableCollection<Module> _modules;

        #region Commands
        private RelayCommand<object>_backCommand;


        #endregion
        #endregion

        #region Properties

        public static ObservableCollection<Module> Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
            }
        }
        

        #region Commands


        public RelayCommand<Object> BackCommand
        {
            get
            {
                return _backCommand ?? (_backCommand = new RelayCommand<object>(o =>
                {

                    NavigationManager.Instance.Navigate(ViewType.Task);

                }));
            }
        }
        #endregion

        #endregion

       

        private static Process _myProcess;
        public static Process MyProcess
        {
            get { return _myProcess; }
            internal set
            {
                _myProcess = value;
                SetFields();
            }
        }

        private static void SetFields()
        {
            if(Modules==null)
                Modules = new ObservableCollection<Module>();

            Modules.Clear();
            try
            {
                foreach (ProcessModule module in _myProcess.Modules)
                {
                    Modules.Add(new Module(module));
                }
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                Console.WriteLine(e);
                
            }
            
            
        }

        public ModulesViewModel()
        {
            if(Modules==null)
                Modules=new ObservableCollection<Module>();
        }
    }
}
