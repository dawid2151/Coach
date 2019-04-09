using System.Diagnostics;
using System.Threading.Tasks;

namespace Coach
{
    public class CSGOStateControl
    {
        private const int CHECK_DELAY = 5000; 
        
        public CSGOStateControl(MainProgram main)
        {
            while (true)
            {
                if (!CheckIfCSGOOpened() && main.HasOpenSession)
                    main.EndSession();
                System.Threading.Thread.Sleep(CHECK_DELAY);
            }
        }

        public bool CheckIfCSGOOpened()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process proc in processes)
                if (proc.ProcessName == "csgo")
                    return true;
            return false;
        }
    }
}
