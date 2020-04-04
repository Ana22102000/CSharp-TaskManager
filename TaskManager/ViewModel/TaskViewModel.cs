using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using System.Windows.Input;
using TaskManager.Model;
using TaskManager.Tools;
using TaskManager.Tools.Managers;
using TaskManager.Tools.Navigation;
using TaskManager.View;

namespace TaskManager.ViewModel
{
    class TaskViewModel : BaseViewModel
    {

        #region Fields

        private ObservableCollection<TaskModel> _processes;
        private int m_selectedResult;

        private TaskModel _taskModel;

        #region Commands

        private ICommand _moduleCommand;
        private ICommand _threadsCommand;
        private ICommand _stopCommand;
        private ICommand _openCommand;


        #endregion

        #endregion

        #region Properties

        public ObservableCollection<TaskModel> Processes
        {
            get => _processes;
            private set
            {
                _processes = value;
                OnPropertyChanged();
            }
        }

        public int SelectedResult
        {
            get { return m_selectedResult; }
            set
            {
                m_selectedResult = value;
                OnPropertyChanged("SelectedResult");
            }
        }

        #region Commands

        public ICommand ModuleCommand
        {
            get
            {
                if (_moduleCommand == null)
                {
                    _moduleCommand = new RelayCommand<object>(
                        ShowModules ); 
                }

                return _moduleCommand;
            }
        }

        public ICommand ThreadsCommand
        {
            get
            {
                if (_threadsCommand == null)
                {
                    _threadsCommand = new RelayCommand<object>(
                        ShowThreads);
                }

                return _threadsCommand;
            }
        }

        public ICommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand<object>(
                        KillProcess );
                }

                return _stopCommand;
            }
        }

        public ICommand OpenCommand
        {
            get
            {
                if (_openCommand == null)
                {
                    _openCommand = new RelayCommand<object>(
                        OpenFolder );
                }

                return _openCommand;
            }
        }

        #endregion

        #endregion

        #region CommandsRealization

        private void ShowModules(object result)
        {
            ModulesViewModel.MyProcess = Process.GetProcessById(((TaskModel)result).Id);

            NavigationManager.Instance.Navigate(ViewType.Modules);

        }

        private void ShowThreads(object result)
        {
            ThreadsViewModel.MyProcess = Process.GetProcessById(((TaskModel)result).Id);

            NavigationManager.Instance.Navigate(ViewType.Threads);

        }

        private void KillProcess(object result)
        {

            Processes.Remove((TaskModel)result);
            Process.GetProcessById(((TaskModel)result).Id).Kill();

        }

        private void OpenFolder(object result)
        {
            try
            {
                string path = Path.GetDirectoryName(((TaskModel)result).FilePath);
                Process.Start(path);
            }
            catch (Exception e)
            {
            } //todo
        }


        #endregion

        public TaskViewModel()
        {
          
            Processes = new ObservableCollection<TaskModel>();

            //1 initialization
            Parallel.ForEach(Process.GetProcesses(), obj => {
                try
                {
                    Processes.Add(new TaskModel(obj));
                }
                catch (Exception)
                {
                    //it`s system process do nothing
                }
            });

            Sort(TaskView.SortField, TaskView.SortAccending);

            Thread loadTasksThread = new Thread(new ThreadStart(LoadTasks));
            loadTasksThread.Start();

            Thread loadDataThread = new Thread(new ThreadStart(LoadData));
            loadDataThread.Start();

        }

        private void LoadTasks()
        {
            int processInterval = 5;
            while (true)
            {

                Thread.Sleep(processInterval * 1000);


                ObservableCollection<TaskModel> newProcesses = new ObservableCollection<TaskModel>();

                Parallel.ForEach(Process.GetProcesses(), obj => {
                    try
                    {

                        newProcesses.Add(new TaskModel(obj));
                    }
                    catch (Exception)
                    {
                        //it`s system process do nothing
                    }
                });

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    try
                    {
                        _taskModel = Processes.ElementAt(m_selectedResult);
                    }
                    catch (System.ArgumentOutOfRangeException e) { }

                    Processes = new ObservableCollection<TaskModel>(newProcesses);
                    Sort(TaskView.SortField, TaskView.SortAccending);

                    int index = Processes.IndexOf(_taskModel);
                    if (index > 0)
                        SelectedResult = index;

                }));

            }

        }

        private void LoadData()
        {
            int processInterval = 2;
            while (true)
            {
                Thread.Sleep(processInterval * 1000);

                if (Processes != null)//todo remove??
                {
                    ObservableCollection<TaskModel> newProcesses = new ObservableCollection<TaskModel>(Processes);

                    Parallel.ForEach(newProcesses, obj => obj.Renew());


                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                        new Action(() =>
                        {
                            
                            try
                            {
                                _taskModel = Processes.ElementAt(m_selectedResult);
                            }
                            catch (System.ArgumentOutOfRangeException e) { }

                                Processes = new ObservableCollection<TaskModel>(newProcesses);
                                Sort(TaskView.SortField, TaskView.SortAccending);

                                int index = Processes.IndexOf(_taskModel);
                                if (index > 0)
                                    SelectedResult = index;
                        }));

                }
            }

        }


        public void Sort(string sortField, bool sortAscending)
        {
            LoaderManager.Instance.ShowLoader();

            if (sortAscending)
                Processes = new ObservableCollection<TaskModel>(from item in _processes
                                                                orderby typeof(TaskModel).GetProperty(sortField).GetValue(item)
                                                                select item);
            else
            {
                Processes = new ObservableCollection<TaskModel>(from item in _processes
                                                                orderby typeof(TaskModel).GetProperty(sortField).GetValue(item) descending
                                                                select item);

            }

            LoaderManager.Instance.HideLoader();

        }


    }
}



