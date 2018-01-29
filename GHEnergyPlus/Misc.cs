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

        internal static void RunEplus(string FileName, string command, string directory)
        {
            string eplusexe = FileName;
            System.Diagnostics.Process P = new System.Diagnostics.Process();
            P.StartInfo.WorkingDirectory = directory;
            P.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            P.StartInfo.FileName = eplusexe;
            P.StartInfo.Arguments = command;
            P.Start();
            P.WaitForExit();
        }


        internal static void insert_surface(out double[][] p, out double[][] pi, double floor_area, double x1)
        {
            double dist2internal = 2;

            p = new double[12][];
            for (int i = 0; i < 12; i++)
            {
                p[i] = new double[3];
                p[i][0] = 0;
                p[i][1] = 0;
                p[i][2] = 0;
            }

            pi = new double[8][];
            for (int i = 0; i < 8; i++)
            {
                pi[i] = new double[3];
                pi[i][0] = 0;
                pi[i][1] = 0;
                pi[i][2] = 0;
            }

            double a = Math.Sqrt(floor_area * x1);
            double b = Math.Sqrt(floor_area / x1);

            for (int i = 0; i < 12; i++)
            {
                if (i < 4)
                    p[i][2] = 0;
                else
                    p[i][2] = 3;

                if (i > 7)
                    p[i][2] = p[i][2] + 0.6;

                if (i == 0 || i == 1 || i == 4 || i == 5 || i == 8 || i == 9)
                    p[i][0] = 0;
                else
                    p[i][0] = a;

                if (i == 2 || i == 1 || i == 6 || i == 5 || i == 9 || i == 10)
                    p[i][1] = 0;
                else
                    p[i][1] = b;
            }

            pi[1][0] = dist2internal;
            pi[1][1] = dist2internal;
            pi[1][2] = 0;

            for (int i = 0; i < 8; i++)
            {
                if (i < 4)
                    pi[i][2] = 0;
                else
                    pi[i][2] = 3;

                if (i == 0 || i == 1 || i == 4 || i == 5)
                    pi[i][0] = dist2internal;
                else
                    pi[i][0] = a - dist2internal;

                if (i == 2 || i == 1 || i == 6 || i == 5)
                    pi[i][1] = dist2internal;
                else
                    pi[i][1] = b - dist2internal;
            }


        }


        internal static void insert_window(out double[] xstart, out double[] zstart, out double[] length, out double[] height,
            double[][] p, double[] x3to6)
        {
            xstart = new double[4]; //N, S, E, W
            zstart = new double[4];
            length = new double[4];
            height = new double[4];

            createWindow(ref xstart[0], ref zstart[0], ref length[0], ref height[0], p[7], p[3], p[0], p[4], x3to6[0]); // N
            createWindow(ref xstart[1], ref zstart[1], ref length[1], ref height[1], p[5], p[1], p[2], p[6], x3to6[1]); // S
            createWindow(ref xstart[2], ref zstart[2], ref length[2], ref height[2], p[6], p[2], p[3], p[7], x3to6[2]); // E
            createWindow(ref xstart[3], ref zstart[3], ref length[3], ref height[3], p[4], p[0], p[1], p[5], x3to6[3]); // W

        }

        internal static void createWindow(ref double xstart, ref double zstart, ref double length, ref double height,
            double[] p1, double[] p2, double[] p3, double[] p4, double x)
        {
            double[] v1 = new double[3];
            double[] v2 = new double[3];
            v1[0] = p4[0] - p1[0];
            v1[1] = p4[1] - p1[1];
            v1[2] = p4[2] - p1[2];

            v2[0] = p2[0] - p1[0];
            v2[1] = p2[1] - p1[1];
            v2[2] = p2[2] - p1[2];

            double mV1 = Math.Sqrt(Math.Pow(v1[0], 2) + Math.Pow(v1[1], 2) + Math.Pow(v1[2], 2));
            double mV2 = Math.Sqrt(Math.Pow(v2[0], 2) + Math.Pow(v2[1], 2) + Math.Pow(v2[2], 2));

            xstart = mV1 / 2.0 - Math.Sqrt(x) * mV1 / 2.0;
            zstart = mV2 / 2.0 - Math.Sqrt(x) * mV2 / 2.0;

            length = mV1 - 2.0 * xstart;
            height = mV2 - 2.0 * zstart;
        }

    }
}
