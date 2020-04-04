using System;
using System.Diagnostics;

namespace TaskManager.Model
{
    internal class MyProcessThread
    {

        public MyProcessThread(ProcessThread thread)
        {
            //todo threads

            Id = thread.Id;
            try
            {
                Time = thread.StartTime;

            }
            catch (Exception e)
            {
                
            }

            State = thread.ThreadState.ToString();
        }

        public int Id { get; set; }
        public string State { get; set; }
        public DateTime Time { get; set; }


    }
}