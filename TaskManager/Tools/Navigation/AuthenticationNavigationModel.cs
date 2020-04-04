using System;
using TaskManager.View;
//using TaskView = TaskManager.View.TaskView;

namespace TaskManager.Tools.Navigation
{
    internal class AuthenticationNavigationModel : BaseNavigationModel
    {


        //private readonly IContentOwner _contentOwner;//

        public AuthenticationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {
            //this._contentOwner = _contentOwner;//
        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Task:
                    AddView(ViewType.Task, new TaskView());
                    break;
                case ViewType.Modules:
                    AddView(ViewType.Modules, new ModulesView());
                    break;
                case ViewType.Threads:
                    AddView(ViewType.Threads, new ThreadsView());
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }


    }
}
