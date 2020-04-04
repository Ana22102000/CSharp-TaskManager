using System.ComponentModel;
using System.Windows.Controls;
using TaskManager.Tools.Navigation;
using TaskManager.ViewModel;

namespace TaskManager.View
{
    /// <summary>
    /// Логика взаимодействия для TaskView.xaml
    /// </summary>
    public partial class TaskView : INavigatable// Window
    {
        public TaskView()
        {
            InitializeComponent();
            TaskViewModel _taskViewModel = new TaskViewModel();
            DataContext = _taskViewModel;
        }

        /*public static */private DataGridColumn currentSortColumn;

        private ListSortDirection currentSortDirection;


        private static bool _sortAscending=true;
        private static string _sortField="Name";


        public static bool SortAccending
        {
            get { return _sortAscending; }
            set { _sortAscending = value; }
        }

        public static string SortField
        {
            get { return _sortField; }
            set { _sortField = value; }
        }

        private void dg_Sorting(object sender, DataGridSortingEventArgs e)
        {




            e.Handled = true;



            TaskViewModel taskViewModel = (TaskViewModel)DataContext;


            _sortField = e.Column.SortMemberPath;


            ListSortDirection direction = ListSortDirection.Ascending;


            if (currentSortColumn != null)
                if (currentSortColumn == e.Column)

                    direction = (currentSortDirection != ListSortDirection.Ascending) ?

                        ListSortDirection.Ascending : ListSortDirection.Descending;


            _sortAscending = direction == ListSortDirection.Ascending;

            taskViewModel.Sort(_sortField, _sortAscending);


            e.Column.SortDirection = direction;

            currentSortColumn = e.Column;

            currentSortDirection = direction;

        }


    }
}
