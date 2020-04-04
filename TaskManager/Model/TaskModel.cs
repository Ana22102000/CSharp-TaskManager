using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TaskManager.Model
{
    internal class TaskModel
    {

        public TaskModel(Process process)
        {

            try
            {
                ProcessModule m = process.MainModule;
            }
            catch (Win32Exception e)
            {
                throw e;
            }
            catch (System.InvalidOperationException e)
            {
                throw e;
            }
            //todo threads
            Name = process.ProcessName;
            Id = process.Id;
           ThreadCount = process.Threads.Count;

           Time = process.StartTime;

           FilePath = process.MainModule.FileName;//?

           try
           {
               Process.GetProcessById(process.Id);
               IsActive = true;
           }
           catch (Exception e)
           {
               IsActive = false;
           }

           //Task task = Task.Run(() =>
           //    UserName = GetUsername(process.SessionId).Result
           //    );
           UserName = GetUsername(process.SessionId);


            Task task2 = Task.Run(() =>
                Cpu = GetCpuUsageForProcess().Result
            );
            // Cpu = await GetCpuUsageForProcess();

            //Task task3 = Task.Run(() =>

            //    Ram = GetRam(process).Result
            //);
            Ram = GetRam(process);

        }


        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public double Cpu { get; set; }//todo or int?
        public double Ram { get; set; }
        public int ThreadCount { get; set; }
        public string UserName { get; set; }
        public string FilePath { get; set; }
        public DateTime Time { get; set; }

        public async void Renew()
        {
            try
            {
                Process process = Process.GetProcessById(this.Id);
                ThreadCount = process.Threads.Count;
                IsActive = true;

                Task task = Task.Run(() =>
                 Cpu = GetCpuUsageForProcess().Result
                );
                Ram = GetRam(process);
                // Cpu = await GetCpuUsageForProcess();
                //Ram = await GetRam(process);

            }
            catch (ArgumentException e)
            {
                IsActive = false;
                //process ended
            }
        }


        [DllImport("Wtsapi32.dll")]
        private static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WtsInfoClass wtsInfoClass, out IntPtr ppBuffer, out int pBytesReturned);
        [DllImport("Wtsapi32.dll")]
        private static extern void WTSFreeMemory(IntPtr pointer);

        private enum WtsInfoClass
        {
            WTSUserName = 5,
            WTSDomainName = 7,
        }
        private static string/* async Task<string>*/ GetUsername(int sessionId, bool prependDomain = true)
        {
            IntPtr buffer;
            int strLen;
            string username = "SYSTEM";
            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WtsInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer);
                WTSFreeMemory(buffer);
                if (prependDomain)
                {
                    if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WtsInfoClass.WTSDomainName, out buffer, out strLen) && strLen > 1)
                    {
                        username = Marshal.PtrToStringAnsi(buffer) + "\\" + username;
                        WTSFreeMemory(buffer);
                    }
                }
            }
            return username;
        }


        private double/*async Task<double>*/ GetRam(Process process)
        {
            double memsize = 0; // memsize in Megabyte
            PerformanceCounter PC = new PerformanceCounter();
            PC.CategoryName = "Process";
            PC.CounterName = "Working Set - Private";
            PC.InstanceName = process.ProcessName;
            memsize = Math.Round(Convert.ToInt32(PC.NextValue()) / (double)(1024*1024),2);
            PC.Close();
            PC.Dispose();
            return memsize;
        }

        private async Task<double> GetCpuUsageForProcess()
        {
            DateTime startTime = DateTime.UtcNow;
            TimeSpan startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            await Task.Delay(500);

            DateTime endTime = DateTime.UtcNow;
            TimeSpan endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            double cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            double totalMsPassed = (endTime - startTime).TotalMilliseconds;
            double cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return cpuUsageTotal * 100;
        }

        
    }
}
