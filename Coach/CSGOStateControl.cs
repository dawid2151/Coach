using System.Diagnostics;

namespace Coach
{
    public class CSGOStateControl
    {
        public int CheckDelay { get; set; } = 3000;

        public CSGOStateControl(MainProgram main)
        {
            while (true)
            {
                if (!CheckIfCSGOOpened() && main.HasOpenSession)
                    main.EndSession();
                System.Threading.Thread.Sleep(CheckDelay);
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
