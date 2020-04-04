using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TaskManager.Model;
using TaskManager.Tools;
using TaskManager.Tools.Managers;
using TaskManager.Tools.Navigation;

namespace TaskManager.ViewModel
{
    class ThreadsViewModel
    {

        #region Fields

        private static ObservableCollection<MyProcessThread> _threads;

        #region Commands
        private RelayCommand<object> _backCommand;


        #endregion
        #endregion

        #region Properties

        public static ObservableCollection<MyProcessThread> Threads
        {
            get => _threads;
            private set
            {
                _threads = value;
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
            if (Threads == null)
                Threads = new ObservableCollection<MyProcessThread>();

            Threads.Clear();
            try
            {
                foreach (ProcessThread thread in _myProcess.Threads)
                {
                    Threads.Add(new MyProcessThread(thread));
                }
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                Console.WriteLine(e);

            }

        }

        public ThreadsViewModel()
        {
            if (Threads == null)
                Threads = new ObservableCollection<MyProcessThread>();
        }
    }
}
