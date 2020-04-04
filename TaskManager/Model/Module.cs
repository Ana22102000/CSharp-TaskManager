using System.Diagnostics;

namespace TaskManager.Model
{
    internal class Module
    {

        public Module(ProcessModule module)
        {
            //todo threads
            Name = module.ModuleName;
            FilePath = module.FileName;
        }

        public string Name { get; set; }
        public string FilePath { get; set; }

    }
}
