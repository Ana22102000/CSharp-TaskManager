namespace TaskManager.Tools.Navigation
{
    internal enum ViewType
    {
        Task=0,
        Modules=1,
        Threads=2,
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
