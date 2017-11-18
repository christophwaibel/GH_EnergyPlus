using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GHEnergyPlus
{
    internal static class Misc
    {
        internal static void RunEplus(string FileName, string command)
        {
            string eplusexe = FileName;
            System.Diagnostics.Process P = new System.Diagnostics.Process();
            P.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            P.StartInfo.FileName = eplusexe;
            P.StartInfo.Arguments = command;
            P.Start();
            P.WaitForExit();
        }

    }
}
