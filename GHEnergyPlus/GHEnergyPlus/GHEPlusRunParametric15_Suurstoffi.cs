using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunParametric15_Suurstoffi : GH_Component
    {
         /// <summary>
        /// Initializes a new instance of the GHEPlusRunParametric15 class.
        /// </summary>
        public GHEPlusRunParametric15_Suurstoffi()
            : base("Prob15_Suurstoffi", "Prob15_Suurstoffi",
                "Problem 15 Waibel et al 2016, four office buildings, daylight, nat vent. in Suurstoffi",
                "EnergyHubs", "Thesis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            //0 - 5
            pManager.AddTextParameter("idf", "idf", "idf file name. has to be in C:\\eplus\\EPOpti17\\Input\\", GH_ParamAccess.item);
            pManager.AddTextParameter("weather", "weather", "weather file name. has to be in \\WeatherData of your Energyplus folder", GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "Run the simulation", GH_ParamAccess.item);
            pManager.AddIntegerParameter("sleep", "sleep", "sleep. default is 1500", GH_ParamAccess.item);
            pManager[3].Optional = true;
            pManager.AddIntegerParameter("folder", "folder", "folder number, like 1,2,3, for parallel runs", GH_ParamAccess.item);
            pManager[4].Optional = true;
            pManager.AddNumberParameter("------------", "------------", "------------", GH_ParamAccess.item);
            pManager[5].Optional = true;

            //6 - 41
            //35 variables
            pManager.AddIntegerParameter("BldAfloors", "x[0]", "Building A number of floors ∈ {1,...,6}. Floor height is 3.3m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldBfloors", "x[1]", "Building B number of floors ∈ {1,...,6}. Floor height is 3.3m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldCfloors", "x[2]", "Building C number of floors ∈ {1,...,6}. Floor height is 3.3m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldDfloors", "x[3]", "Building D number of floors ∈ {1,...,6}. Floor height is 3.3m.", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldA_X1", "x[4]", "Building A x-coordinate of cornerpoint 1, ∈ [0, 6.2].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y1", "x[5]", "Building A y-coordinate of cornerpoint 1, ∈ [0, 13.09].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X2", "x[6]", "Building A x-coordinate of cornerpoint 2, ∈ [0, 10.38].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y2", "x[7]", "Building A y-coordinate of cornerpoint 2, ∈ [0, 4.92].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X3", "x[8]", "Building A x-coordinate of cornerpoint 3, ∈ [0, 5.08].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y3", "x[9]", "Building A y-coordinate of cornerpoint 3, ∈ [0, 12.54].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X4", "x[10]", "Building A x-coordinate of cornerpoint 4, ∈ [0, 11.17].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y4", "x[11]", "Building A y-coordinate of cornerpoint 4, ∈ [0, 4.53].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldB_X1", "x[12]", "Building B x-coordinate of cornerpoint 1, ∈ [0, 6.2].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y1", "x[13]", "Building B y-coordinate of cornerpoint 1, ∈ [0, 13.09].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X2", "x[14]", "Building B x-coordinate of cornerpoint 2, ∈ [0, 10.39].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y2", "x[15]", "Building B y-coordinate of cornerpoint 2, ∈ [0, 4.92].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X3", "x[16]", "Building B x-coordinate of cornerpoint 3, ∈ [0, 5.08].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y3", "x[17]", "Building B y-coordinate of cornerpoint 3, ∈ [0, 12.54].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X4", "x[18]", "Building B x-coordinate of cornerpoint 4, ∈ [0, 11.18].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y4", "x[19]", "Building B y-coordinate of cornerpoint 4, ∈ [0, 4.52].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldC_X1", "x[20]", "Building C x-coordinate of cornerpoint 1, ∈ [0, 5.08].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y1", "x[21]", "Building C y-coordinate of cornerpoint 1, ∈ [0, 12.54].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X2", "x[22]", "Building C x-coordinate of cornerpoint 2, ∈ [0, 11.17].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y2", "x[23]", "Building C y-coordinate of cornerpoint 2, ∈ [0, 4.52].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X3", "x[24]", "Building C x-coordinate of cornerpoint 3, ∈ [0, 4.07].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y3", "x[25]", "Building C y-coordinate of cornerpoint 3, ∈ [0, 12.04].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X4", "x[26]", "Building C x-coordinate of cornerpoint 4, ∈ [0, 12.07].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y4", "x[27]", "Building C y-coordinate of cornerpoint 4, ∈ [0, 4.08].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldD_X1", "x[28]", "Building D x-coordinate of cornerpoint 1, ∈ [0, 5.09].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y1", "x[29]", "Building D y-coordinate of cornerpoint 1, ∈ [0, 12.54].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X2", "x[30]", "Building D x-coordinate of cornerpoint 2, ∈ [0, 11.18].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y2", "x[31]", "Building D y-coordinate of cornerpoint 2, ∈ [0, 4.53].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X3", "x[32]", "Building D x-coordinate of cornerpoint 3, ∈ [0, 4.07].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y3", "x[33]", "Building D y-coordinate of cornerpoint 3, ∈ [0, 12.04].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X4", "x[34]", "Building D x-coordinate of cornerpoint 4, ∈ [0, 12.07].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y4", "x[35]", "Building D y-coordinate of cornerpoint 4, ∈ [0, 4.09].", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Cost", "Cost", "Cost for energy system minus rent (65CHF/m2).", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int sleeptime = 1500;
            if (!DA.GetData(3, ref sleeptime)) { sleeptime = 1500; }

            int folderint = 0;
            if (!DA.GetData(4, ref folderint)) { folderint = 0; }
            string path_in = @"c:\eplus\EPOpti17\Input" + folderint + @"\";
            string path_out = @"c:\eplus\EPOpti17\Output" + folderint + @"\";
            string eplusexe = @"c:\eplus\EPOpti17\Input" + folderint + @"\ep\energyplus.exe";

            //get idf and weather files
            string idffile = @"blabla";
            if (!DA.GetData(0, ref idffile)) { return; }
            string weatherfile = @"blabla";
            if (!DA.GetData(1, ref weatherfile)) { return; }


            //RUN SIMU
            bool runit = false;
            if (!DA.GetData(2, ref runit)) { return; }



            //get input parameters
            int dvar = 36;
            int[] xint = new int[4];
            double[] x = new double[dvar];
            for (int i = 0; i < 4; i++)
            {
                if (!DA.GetData(i + 6, ref xint[i])) return;
                else x[i] = Convert.ToDouble(xint[i]);
            }
            for (int i = 4; i < x.Length; i++)
                if (!DA.GetData(i + 6, ref x[i])) { return; };


            if (runit == true)
            {
                double lvlHeight = 3.3; // height per level
                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // 1___4  1___4
                // | C |  | D |
                // 2---3  2---3
                //
                // 1___4  1___4
                // | A |  | B |
                // 2---3  2---3            


                // PROJECTIVE TRANSFORMATION MATRICES - using Matlab script C=ProjectiveTransformation(Ain,Bin)
                // design variables are in domain x \in [0,1]
                double[,] CTransf_C1 = new double[,]
                {
                    {8.3240,    2.0830,     25.7800},
                    {-3.9866,   8.9832,     97.7800},
                    {0.0010,    -0.0003,    1.000}
                };
                double[,] CTransf_C4 = new double[,]
                {
                    {8.0889,    2.9790,   55.2700},
                    {-4.0389,   8.4340,   83.2600},
                    {-0.0005,   -0.0010,  1.0000}
                };
                double[,] CTransf_C3 = new double[,]
                {
                    {8.1016,    3.1226,   48.0700},
                    {-4.0399,    8.6318,   63.1000},
                    {-0.0005,    0.0014,    1.0000}
                };
                double[,] CTransf_C2 = new double[,]
                {
                    {8.2900,    2.0900,   20.8400},
                    {-4.0800,    9.0000,   76.5100},
                    {0,    0.0000,    1.0000}
                };

                double[,] CTransf_D1 = new double[,]
                {
                    {8.2058,    3.0135,   67.9100},
                    {-3.9272,    8.5081,   77.0300},
                    {0.0010,   -0.0004,    1.0000}
                };
                double[,] CTransf_D4 = new double[,]
                {
                    {8.1434,    3.8196,   97.4500},
                    {-3.9504,    7.9754,   62.4800},
                    {0.0005,   -0.0015,    1.0000}
                };
                double[,] CTransf_D3 = new double[,]
                {
                    {7.9743,    4.0454,   88.0000},
                    {-4.0193,    8.0854,   43.4300},
                    {-0.0010,    0.0005,    1.0000}
                };
                double[,] CTransf_D2 = new double[,]
                {
                    {9.0858,    2.7179,   60.7100},
                    {-3.2659,    8.3286,   56.8700},
                    {0.0139,   -0.0052,    1.0000}
                };

                double[,] CTransf_B1 = new double[,]
                {
                    {8.0247,    3.0003,   55.9800},
                    {-4.0688,    8.4949,   43.6000},
                    {-0.0015,   -0.0007,    1.0000}
                };
                double[,] CTransf_B4 = new double[,]
                {
                    {8.0272,    3.6726,   81.7800},
                    {-3.9928,    7.9349,   30.8900},
                    {-0.0005,   -0.0035,    1.0000}
                };
                double[,] CTransf_B3 = new double[,]
                {
                    {8.0700,    4.0000,   72.3300},
                    {-3.9800,    8.0600,   11.8400},
                    {-0.0000,   -0.0000,    1.0000}
                };
                double[,] CTransf_B2 = new double[,]
                {
                    {8.1580,    3.1043,   48.7800},
                    {-4.0004,    8.5635,   23.4400},
                    {0.0005,    0.0010,    1.0000}
                };

                double[,] CTransf_A1 = new double[,]
                {
                    {8.2642,    2.0949,   17.5900},
                    {-4.1482,    9.0180,   62.5100},
                    {-0.0010,    0.0003,    1.0000}
                };
                double[,] CTransf_A4 = new double[,]
                {
                    {8.1300,    3.0500,   43.3300},
                    {-4.0100,    8.5300,   49.8300},
                    {0.0000,    0.0000,    1.0000}
                };
                double[,] CTransf_A3 = new double[,]
                {
                    {8.1424,    2.9844,   36.1400},
                    {-3.9870,    8.4858,   29.6600},
                    {0.0005,   -0.0014,    1.0000}
                };
                double[,] CTransf_A2 = new double[,]
                {
                    {8.3312,    2.1122,   12.6400},
                    {-4.0247,    9.0517,   41.2300},
                    {0.0015,    0.0008,    1.0000}
                };


                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //FROM HERE ON change building coordinates creation... use mapping first:
                // 
                // pin should be a point in the 0,1 domain, i.e. the design variable for a building.
                //double[] newp = Misc.TransformPoints(CTransf_A1, pin);




                //heights
                double A_z = x[0] * lvlHeight;
                double B_z = x[1] * lvlHeight;
                double C_z = x[2] * lvlHeight;
                double D_z = x[3] * lvlHeight;
                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // EXTERNAL WALLS
                // Bld A
                // pt 1, x/y bounds. x: [27.87, 34.07]; y: [93.70, 106.79]
                // pt 2, x/y bounds. x: [20.84, 31.22]; y: [76.51, 81.43]
                // pt 3, x/y bounds. x: [51.12, 56.20]; y: [59.09, 71.63]
                // pt 4, x/y bounds. x: [55.27, 66.44]; y: [83.26, 87.79]
                double[] A_px_lb = new double[4];
                double[] A_py_lb = new double[4];
                A_px_lb[0] = 27.87;
                A_px_lb[1] = 20.84;
                A_px_lb[2] = 51.12;
                A_px_lb[3] = 55.27;
                A_py_lb[0] = 93.70;
                A_py_lb[1] = 76.51;
                A_py_lb[2] = 59.09;
                A_py_lb[3] = 83.26;


                // Bld B
                // pt 1, x/y bounds. x: [19.68, 25.88]; y: [58.42, 71.51]
                // pt 2, x/y bounds. x: [12.64, 23.03]; y: [41.23, 46.15]
                // pt 3, x/y bounds. x: [39.18, 44.26]; y: [25.66, 38.20]
                // pt 4, x/y bounds. x: [43.33, 54.51]; y: [49.83, 54.35]
                double[] B_px_lb = new double[4];
                double[] B_py_lb = new double[4];
                B_px_lb[0] = 19.68;
                B_px_lb[1] = 12.64;
                B_px_lb[2] = 39.18;
                B_px_lb[3] = 43.33;
                B_py_lb[0] = 58.42;
                B_py_lb[1] = 41.23;
                B_py_lb[2] = 25.66;
                B_py_lb[3] = 49.83;

                // Bld C
                // pt 1, x/y bounds. x: [59.02, 64.10]; y: [39.59, 52.13]
                // pt 2, x/y bounds. x: [48.78, 59.95]; y: [23.44, 27.96]
                // pt 3, x/y bounds. x: [76.33, 80.40]; y: [7.86, 19.90]
                // pt 4, x/y bounds. x: [81.78, 93.85]; y: [30.89, 34.97]
                double[] C_px_lb = new double[4];
                double[] C_py_lb = new double[4];
                C_px_lb[0] = 59.02;
                C_px_lb[1] = 48.78;
                C_px_lb[2] = 76.33;
                C_px_lb[3] = 81.78;
                C_py_lb[0] = 39.59;
                C_py_lb[1] = 23.44;
                C_py_lb[2] = 7.86;
                C_py_lb[3] = 30.89;

                // Bld D
                // pt 1, x/y bounds. x: [70.95, 76.04]; y: [73.03, 85.57]
                // pt 2, x/y bounds. x: [60.71, 71.89]; y: [56.87, 61.40]
                // pt 3, x/y bounds. x: [92.00, 96.07]; y: [39.45, 51.49]
                // pt 4, x/y bounds. x: [97.45, 109.52]; y: [62.48, 66.57]
                double[] D_px_lb = new double[4];
                double[] D_py_lb = new double[4];
                D_px_lb[0] = 70.95;
                D_px_lb[1] = 60.71;
                D_px_lb[2] = 92.00;
                D_px_lb[3] = 97.45;
                D_py_lb[0] = 73.03;
                D_py_lb[1] = 56.87;
                D_py_lb[2] = 39.45;
                D_py_lb[3] = 62.48;


                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // INTERNAL WALLS
                // Bld A
                // pt 1, x/y bounds. x: [31.32, 37.52]; y: [86.43, 99.52]
                // pt 2, x/y bounds. x: [26.61, 36.99]; y: [79.24, 84.16]
                // pt 3, x/y bounds. x: [48.29, 53.37]; y: [66.06, 78.6]
                // pt 4, x/y bounds. x: [49.06, 60.23]; y: [80.74, 85.27]
                double[] Ain_px_lb = new double[4];
                double[] Ain_py_lb = new double[4];
                Ain_px_lb[0] = 31.32;
                Ain_px_lb[1] = 26.61;
                Ain_px_lb[2] = 48.29;
                Ain_px_lb[3] = 49.06;
                Ain_py_lb[0] = 86.43;
                Ain_py_lb[1] = 79.24;
                Ain_py_lb[2] = 66.06;
                Ain_py_lb[3] = 80.74;

                // Bld B
                // pt 1, x/y bounds. x: [23.12, 29.32]; y: [51.15, 64.24]
                // pt 2, x/y bounds. x: [18.41, 28.79]; y: [43.97, 48.89]
                // pt 3, x/y bounds. x: [36.36, 41.44]; y: [32.62, 45.16]
                // pt 4, x/y bounds. x: [37.13, 48.30]; y: [47.31, 51.84]
                double[] Bin_px_lb = new double[4];
                double[] Bin_py_lb = new double[4];
                Bin_px_lb[0] = 23.12;
                Bin_px_lb[1] = 18.41;
                Bin_px_lb[2] = 36.36;
                Bin_px_lb[3] = 37.13;
                Bin_py_lb[0] = 51.15;
                Bin_py_lb[1] = 43.97;
                Bin_py_lb[2] = 32.62;
                Bin_py_lb[3] = 47.31;

                // Bld C
                // pt 1, x/y bounds. x: [61.84, 66.93]; y: [32.63, 45.17]
                // pt 2, x/y bounds. x: [54.99, 66.16]; y: [25.95, 30.48]
                // pt 3, x/y bounds. x: [74.06, 78.14]; y: [14.55, 26.59]
                // pt 4, x/y bounds. x: [75.07, 87.14]; y: [28.62, 32.70]
                double[] Cin_px_lb = new double[4];
                double[] Cin_py_lb = new double[4];
                Cin_px_lb[0] = 61.84;
                Cin_px_lb[1] = 54.99;
                Cin_px_lb[2] = 74.06;
                Cin_px_lb[3] = 75.07;
                Cin_py_lb[0] = 32.63;
                Cin_py_lb[1] = 25.95;
                Cin_py_lb[2] = 14.55;
                Cin_py_lb[3] = 28.62;

                // Bld D
                // pt 1, x/y bounds. x: [73.78, 78.86]; y: [66.07, 78.60]
                // pt 2, x/y bounds. x: [66.92, 78.09]; y: [59.39, 63.92]
                // pt 3, x/y bounds. x: [89.73, 93.81]; y: [46.14, 58.18]
                // pt 4, x/y bounds. x: [90.74, 102.81]; y: [60.21, 64.30]
                double[] Din_px_lb = new double[4];
                double[] Din_py_lb = new double[4];
                Din_px_lb[0] = 73.78;
                Din_px_lb[1] = 66.92;
                Din_px_lb[2] = 89.73;
                Din_px_lb[3] = 90.74;
                Din_py_lb[0] = 66.07;
                Din_py_lb[1] = 59.39;
                Din_py_lb[2] = 46.14;
                Din_py_lb[3] = 60.21;


                double[] A_px = new double[4];
                double[] A_py = new double[4];
                double[] B_px = new double[4];
                double[] B_py = new double[4];
                double[] C_px = new double[4];
                double[] C_py = new double[4];
                double[] D_px = new double[4];
                double[] D_py = new double[4];
                double[] Ain_px = new double[4];
                double[] Ain_py = new double[4];
                double[] Bin_px = new double[4];
                double[] Bin_py = new double[4];
                double[] Cin_px = new double[4];
                double[] Cin_py = new double[4];
                double[] Din_px = new double[4];
                double[] Din_py = new double[4];
                int step = 0;
                for (int i = 0; i < 4; i++)
                {
                    A_px[i] = x[4 + step] + A_px_lb[i];
                    A_py[i] = x[4 + step + 1] + A_py_lb[i];
                    B_px[i] = x[12 + step] + B_px_lb[i];
                    B_py[i] = x[12 + step + 1] + B_py_lb[i];
                    C_px[i] = x[20 + step] + C_px_lb[i];
                    C_py[i] = x[20 + step + 1] + C_py_lb[i];
                    D_px[i] = x[28 + step] + D_px_lb[i];
                    D_py[i] = x[28 + step + 1] + D_py_lb[i];

                    //Ain_px[i] = x[4 + step] + Ain_px_lb[i];
                    //Ain_py[i] = x[4 + step + 1] + Ain_py_lb[i];
                    //Bin_px[i] = x[12 + step] + Bin_px_lb[i];
                    //Bin_py[i] = x[12 + step + 1] + Bin_py_lb[i];
                    //Cin_px[i] = x[20 + step] + Cin_px_lb[i];
                    //Cin_py[i] = x[20 + step + 1] + Cin_py_lb[i];
                    //Din_px[i] = x[28 + step] + Din_px_lb[i];
                    //Din_py[i] = x[28 + step + 1] + Din_py_lb[i];
                    step += 2;
                }


                List<Point3d> plist = new List<Point3d>();
                plist.Add(new Point3d(A_px[0], A_py[0], 0));
                plist.Add(new Point3d(A_px[1], A_py[1], 0));
                plist.Add(new Point3d(A_px[2], A_py[2], 0));
                plist.Add(new Point3d(A_px[3], A_py[3], 0));
                double[][] A_pts_offset = Misc.PtsFromOffsetRectangle(plist,5);

                plist = new List<Point3d>();
                plist.Add(new Point3d(B_px[0], B_py[0], 0));
                plist.Add(new Point3d(B_px[1], B_py[1], 0));
                plist.Add(new Point3d(B_px[2], B_py[2], 0));
                plist.Add(new Point3d(B_px[3], B_py[3], 0));
                double[][] B_pts_offset = Misc.PtsFromOffsetRectangle(plist,5);

                plist = new List<Point3d>();
                plist.Add(new Point3d(C_px[0], C_py[0], 0));
                plist.Add(new Point3d(C_px[1], C_py[1], 0));
                plist.Add(new Point3d(C_px[2], C_py[2], 0));
                plist.Add(new Point3d(C_px[3], C_py[3], 0));
                double[][] C_pts_offset = Misc.PtsFromOffsetRectangle(plist,5);

                plist = new List<Point3d>();
                plist.Add(new Point3d(D_px[0], D_py[0], 0));
                plist.Add(new Point3d(D_px[1], D_py[1], 0));
                plist.Add(new Point3d(D_px[2], D_py[2], 0));
                plist.Add(new Point3d(D_px[3], D_py[3], 0));
                double[][] D_pts_offset = Misc.PtsFromOffsetRectangle(plist,5);

                for (int i = 0; i < 4; i++)
                {
                    Ain_px[i] = A_pts_offset[i][0];
                    Ain_py[i] = A_pts_offset[i][1];
                    Bin_px[i] = B_pts_offset[i][0];
                    Bin_py[i] = B_pts_offset[i][1];
                    Cin_px[i] = C_pts_offset[i][0];
                    Cin_py[i] = C_pts_offset[i][1];
                    Din_px[i] = D_pts_offset[i][0];
                    Din_py[i] = D_pts_offset[i][1];
                }




                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // CORE WALLS
                //
                // 1_____4  
                // |  _  |
                // | |_| |
                // |_____|
                // 2     3 
                //
                // Identical points 1, 2, 3, 4 from internal



                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // CONNECTING WALLS internal to external
                //
                // 1_____4  
                // | \_/ |
                // | |_| |
                // |_/_\_|
                // 2     3 
                //
                // connecting point 1 from internal to point 1 from external

                // All point coordinates
                //A_z     : total building height building A
                //B_z
                //C_z
                //D_z

                //A_px	: four outer corner points building A, x-coordinate
                //A_py	: four outer corner points building A, y-coordinate	
                //B_px
                //B_py
                //C_px
                //C_py
                //D_px
                //D_py

                //Ain_px	: four inner corner points building A, x-coordinate
                //Ain_py	: four inner corner points building A, y-coordinate
                //Bin_px
                //Bin_py
                //Cin_px
                //Cin_py
                //Din_px
                //Din_py

                double[] z = new double[4];
                double[][] px = new double[4][];
                double[][] py = new double[4][];
                double[][] pinx = new double[4][];
                double[][] piny = new double[4][];
                z[0] = A_z;
                z[1] = B_z;
                z[2] = C_z;
                z[3] = D_z;
                px[0] = A_px;
                px[1] = B_px;
                px[2] = C_px;
                px[3] = D_px;
                py[0] = A_py;
                py[1] = B_py;
                py[2] = C_py;
                py[3] = D_py;
                pinx[0] = Ain_px;
                pinx[1] = Bin_px;
                pinx[2] = Cin_px;
                pinx[3] = Din_px;
                piny[0] = Ain_py;
                piny[1] = Bin_py;
                piny[2] = Cin_py;
                piny[3] = Din_py;


                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // Volume box for shading objects, bld A, B, C, D
                // 
                //       ___
                //      /__/|
                //      |__|/
                //
                Point3d[][] Boxes = new Point3d[4][];  // [BLD][0-8]
                for (int b = 0; b < 4; b++)
                {
                    Boxes[b] = new Point3d[8];
                    int pcnt = 0;
                    for (int p = 0; p < 8; p++)
                    {
                        if (pcnt == 4) pcnt = 0;
                        if (p > 3) Boxes[b][p] = new Point3d(px[b][pcnt], py[b][pcnt], z[b]);
                        else Boxes[b][p] = new Point3d(px[b][pcnt], py[b][pcnt], 0);
                        pcnt++;
                    }
                }





                //***********************************************************************************
                //***********************************************************************************
                //***********************************************************************************
                //modify idf file with parameters and save as new idf file
                //string idfmodified = idffile + "_modi";


                double[] totElec = new double[4];
                double[] totCool = new double[4];
                double[] totHeat = new double[4];
                double[] totsqm = new double[4];
                double[] peakElec = new double[4];
                double[] peakCool = new double[4];
                double[] peakHeat = new double[4];

                for (int BLD = 0; BLD < 4; BLD++)
                {
                    // BUILDING A, B, C, or D
                    int levels = Convert.ToInt16(x[BLD]);
                    string idfmodified = idffile + "_L" + x[BLD].ToString() + "_modi";

                    //load idf into a huge string
                    string[] lines;
                    var list = new List<string>();
                    var fileStream = new FileStream(path_in + idffile + "_L" + levels.ToString() + ".idf", FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            list.Add(line);
                        }
                    }
                    lines = list.ToArray();
                    fileStream.Close();




                    //to be replaced

                    // Daylighting:Controls:                    %DLx_p0%, %DLy_p0%,    : center of each DL, same for each floow (0-3, each zone)
                    //                                          %DLz_p1%               : per floow, 1-levels. %pz_1% + 0.85
                    //
                    // ZoneVentilation:WindandStackOpenArea:    %Vent_f0%                       : opening area. 50% of window area (0-3, each zone)
                    //
                    // BuildingSurface:Detailed
                    // outer and inner  x points:               %px_0%, %pxin_0%,               : px[BLD][0-3], pinx[BLD][0-3]
                    //                  y points:               %py_0%, %pyin_0%
                    //                  z points:               %pz_1%                          : 0 unused. 1 to levels. levelHeight*currentlevel. how many levels? : levels
                    //
                    // FenestrationSurface:Detailed:            %winAx_l%,      %winAx_r%
                    //                                          %winAy_l%,      %winAy_r% 
                    //
                    //                                          %winBx_l%,      %winBx_r%
                    //                                          %winBy_l%,      %winBy_r% 
                    //
                    //                                          %winCx_l%,      %winCx_r%
                    //                                          %winCy_l%,      %winCy_r% 
                    //
                    //                                          %winDx_l%,      %winDx_r%
                    //                                          %winDy_l%,      %winDy_r% 
                    //
                    //                                          %win_btm_1%,  %win_top_1% 

                    // add shading objects from three other buildings


                    // 8 DL(xy) + z per levels, 4 window areas, 16 corner pts, level heights, 16 window corner pts, 2*levels window heights
                    string[] replacethis = new string[8 + levels + 4 + 16 + levels + 16 + levels * 2];       //variables that need to be replaced

                    int xvar_count = 0;
                    for (int i = 0; i < 4; i++)        // DL xy
                    {
                        replacethis[xvar_count] = @"%DLx_p" + i.ToString() + @"%";
                        replacethis[xvar_count + 1] = @"%DLy_p" + i.ToString() + @"%";
                        xvar_count += 2;
                    }
                    for (int i = 0; i < levels; i++)    // DL z
                    {
                        replacethis[xvar_count] = @"%DLz_p" + (i + 1).ToString() + @"%";
                        xvar_count++;
                    }

                    for (int i = 0; i < 4; i++)         // window areas
                    {
                        replacethis[xvar_count] = @"%Vent_f" + i.ToString() + @"%";
                        xvar_count++;
                    }

                    for (int i = 0; i < 4; i++)         // corner points, inside and outside
                    {
                        replacethis[xvar_count] = @"%px_" + i.ToString() + @"%";
                        replacethis[xvar_count + 1] = @"%pxin_" + i.ToString() + @"%";
                        replacethis[xvar_count + 2] = @"%py_" + i.ToString() + @"%";
                        replacethis[xvar_count + 3] = @"%pyin_" + i.ToString() + @"%";
                        xvar_count += 4;
                    }

                    for (int i = 0; i < levels; i++)    //floor heights
                    {
                        replacethis[xvar_count] = @"%pz_" + (i + 1).ToString() + @"%";
                        xvar_count++;
                    }

                    //window corner points per zone
                    replacethis[xvar_count] = @"%winAx_l%";
                    replacethis[xvar_count + 1] = @"%winAx_r%";
                    replacethis[xvar_count + 2] = @"%winAy_l%";
                    replacethis[xvar_count + 3] = @"%winAy_r%";
                    replacethis[xvar_count + 4] = @"%winBx_l%";
                    replacethis[xvar_count + 5] = @"%winBx_r%";
                    replacethis[xvar_count + 6] = @"%winBy_l%";
                    replacethis[xvar_count + 7] = @"%winBy_r%";
                    replacethis[xvar_count + 8] = @"%winCx_l%";
                    replacethis[xvar_count + 9] = @"%winCx_r%";
                    replacethis[xvar_count + 10] = @"%winCy_l%";
                    replacethis[xvar_count + 11] = @"%winCy_r%";
                    replacethis[xvar_count + 12] = @"%winDx_l%";
                    replacethis[xvar_count + 13] = @"%winDx_r%";
                    replacethis[xvar_count + 14] = @"%winDy_l%";
                    replacethis[xvar_count + 15] = @"%winDy_r%";
                    xvar_count += 16;
                    for (int i = 0; i < levels; i++)    //window heights per floor
                    {
                        replacethis[xvar_count] = @"%win_btm_" + (i + 1).ToString() + @"%";
                        replacethis[xvar_count + 1] = @"%win_top_" + (i + 1).ToString() + @"%";
                        xvar_count += 2;
                    }




                    //replacers
                    string[] replacers = new string[replacethis.Length];

                    double[][][] CP_zones = new double[4][][];
                    for (int zo = 0; zo < 4; zo++)
                    {
                        CP_zones[zo] = new double[4][];
                        for (int i = 0; i < 4; i++)
                        {
                            CP_zones[zo][i] = new double[2];
                        }
                    }

                    //floor zone 0
                    CP_zones[0][0][0] = px[BLD][1];
                    CP_zones[0][1][0] = px[BLD][0];
                    CP_zones[0][2][0] = pinx[BLD][0];
                    CP_zones[0][3][0] = pinx[BLD][1];
                    CP_zones[0][0][1] = py[BLD][1];
                    CP_zones[0][1][1] = py[BLD][0];
                    CP_zones[0][2][1] = piny[BLD][0];
                    CP_zones[0][3][1] = piny[BLD][1];
                    //floor zone 1
                    CP_zones[1][0][0] = px[BLD][2];
                    CP_zones[1][1][0] = px[BLD][1];
                    CP_zones[1][2][0] = pinx[BLD][1];
                    CP_zones[1][3][0] = pinx[BLD][2];
                    CP_zones[1][0][1] = py[BLD][2];
                    CP_zones[1][1][1] = py[BLD][1];
                    CP_zones[1][2][1] = piny[BLD][1];
                    CP_zones[1][3][1] = piny[BLD][2];
                    //floor zone 2
                    CP_zones[2][0][0] = px[BLD][3];
                    CP_zones[2][1][0] = px[BLD][2];
                    CP_zones[2][2][0] = pinx[BLD][2];
                    CP_zones[2][3][0] = pinx[BLD][3];
                    CP_zones[2][0][1] = py[BLD][3];
                    CP_zones[2][1][1] = py[BLD][2];
                    CP_zones[2][2][1] = piny[BLD][2];
                    CP_zones[2][3][1] = piny[BLD][3];
                    //floor zone 3
                    CP_zones[3][0][0] = px[BLD][3];
                    CP_zones[3][1][0] = pinx[BLD][3];
                    CP_zones[3][2][0] = pinx[BLD][0];
                    CP_zones[3][3][0] = px[BLD][0];
                    CP_zones[3][0][1] = py[BLD][3];
                    CP_zones[3][1][1] = piny[BLD][3];
                    CP_zones[3][2][1] = piny[BLD][0];
                    CP_zones[3][3][1] = py[BLD][0];


                    //using corner points, calc centroid per zone for Daylight. x/y 
                    double[] cen0 = Misc.Centroid(CP_zones[0]);
                    double[] cen1 = Misc.Centroid(CP_zones[1]);
                    double[] cen2 = Misc.Centroid(CP_zones[2]);
                    double[] cen3 = Misc.Centroid(CP_zones[3]);
                    replacers[0] = cen0[0].ToString();
                    replacers[1] = cen0[1].ToString();
                    replacers[2] = cen1[0].ToString();
                    replacers[3] = cen1[1].ToString();
                    replacers[4] = cen2[0].ToString();
                    replacers[5] = cen2[1].ToString();
                    replacers[6] = cen3[0].ToString();
                    replacers[7] = cen3[1].ToString();
                    xvar_count = 8;
                    //Daylight z. it's a replacer, so I can change ceilight height later...
                    for (int i = 0; i < levels; i++)
                    {
                        replacers[xvar_count] = (lvlHeight * i + 0.85).ToString();
                        xvar_count++;
                    }



                    //window area. first I need window points.
                    //window points

                    Point3d[] win_L = new Point3d[4]; //per zone
                    Point3d[] win_R = new Point3d[4];

                    //take corner point outer wall left, and create vector to the right corner point.
                    //rhino add point using that unit vector scaled up to 1m (constant 1m offset from walls),
                    Vector3d cpzone0R = new Vector3d(px[BLD][1], py[BLD][1], 0);
                    Vector3d cpzone0L = new Vector3d(px[BLD][0], py[BLD][0], 0);
                    Vector3d vecZ0 = Vector3d.Subtract(cpzone0L, cpzone0R);
                    vecZ0.Unitize();
                    win_L[0] = Point3d.Add(new Point3d(cpzone0L), Vector3d.Multiply(-1.0, vecZ0));
                    win_R[0] = Point3d.Add(new Point3d(cpzone0R), Vector3d.Multiply(1.0, vecZ0));

                    Vector3d cpzone1R = new Vector3d(px[BLD][2], py[BLD][2], 0);
                    Vector3d cpzone1L = new Vector3d(px[BLD][1], py[BLD][1], 0);
                    Vector3d vecZ1 = Vector3d.Subtract(cpzone1L, cpzone1R);
                    vecZ1.Unitize();
                    win_L[1] = Point3d.Add(new Point3d(cpzone1L), Vector3d.Multiply(-1.0, vecZ1));
                    win_R[1] = Point3d.Add(new Point3d(cpzone1R), Vector3d.Multiply(1.0, vecZ1));

                    Vector3d cpzone2R = new Vector3d(px[BLD][3], py[BLD][3], 0);
                    Vector3d cpzone2L = new Vector3d(px[BLD][2], py[BLD][2], 0);
                    Vector3d vecZ2 = Vector3d.Subtract(cpzone2L, cpzone2R);
                    vecZ2.Unitize();
                    win_L[2] = Point3d.Add(new Point3d(cpzone2L), Vector3d.Multiply(-1.0, vecZ2));
                    win_R[2] = Point3d.Add(new Point3d(cpzone2R), Vector3d.Multiply(1.0, vecZ2));

                    Vector3d cpzone3R = new Vector3d(px[BLD][0], py[BLD][0], 0);
                    Vector3d cpzone3L = new Vector3d(px[BLD][3], py[BLD][3], 0);
                    Vector3d vecZ3 = Vector3d.Subtract(cpzone3L, cpzone3R);
                    vecZ3.Unitize();
                    win_L[3] = Point3d.Add(new Point3d(cpzone3L), Vector3d.Multiply(-1.0, vecZ3));
                    win_R[3] = Point3d.Add(new Point3d(cpzone3R), Vector3d.Multiply(1.0, vecZ3));


                    Point3d[] pts = new Point3d[16];
                    //win zone 0
                    pts[0] = new Point3d(cpzone0L);
                    pts[1] = new Point3d(cpzone0R);
                    pts[2] = new Point3d(cpzone0R);
                    pts[3] = new Point3d(cpzone0L);
                    pts[2][2] = 2.5; //const win height
                    pts[3][2] = 2.5;

                    //win zone 1
                    pts[4] = new Point3d(cpzone1L);
                    pts[5] = new Point3d(cpzone1R);
                    pts[6] = new Point3d(cpzone1R);
                    pts[7] = new Point3d(cpzone1L);
                    pts[6][2] = 2.5; //const win height
                    pts[7][2] = 2.5;

                    //win zone 2
                    pts[8] = new Point3d(cpzone2L);
                    pts[9] = new Point3d(cpzone2R);
                    pts[10] = new Point3d(cpzone2R);
                    pts[11] = new Point3d(cpzone2L);
                    pts[10][2] = 2.5; //const win height
                    pts[11][2] = 2.5;

                    //win zone 3
                    pts[12] = new Point3d(cpzone3L);
                    pts[13] = new Point3d(cpzone3R);
                    pts[14] = new Point3d(cpzone3R);
                    pts[15] = new Point3d(cpzone3L);
                    pts[14][2] = 2.5; //const win height
                    pts[15][2] = 2.5;

                    int count = 0;
                    for (int i = 0; i < 4; i++) //for 4 zones
                    {
                        double a = pts[count].DistanceTo(pts[count + 1]);
                        double b = pts[count + 1].DistanceTo(pts[count + 2]);
                        double c = pts[count + 2].DistanceTo(pts[count]);
                        double p = 0.5 * (a + b + c);
                        double area = (Math.Sqrt(p * (p - a) * (p - b) * (p - c))) * 2; // two triangles is the quad
                        count += 4;
                        replacers[xvar_count] = (area * 0.5).ToString();    // 50% of the window is openable
                        xvar_count++;
                    }

                    // zone corner points x/y
                    for (int i = 0; i < 4; i++)
                    {
                        replacers[xvar_count] = px[BLD][i].ToString();
                        replacers[xvar_count + 1] = pinx[BLD][i].ToString();
                        replacers[xvar_count + 2] = py[BLD][i].ToString();
                        replacers[xvar_count + 3] = piny[BLD][i].ToString();
                        xvar_count += 4;
                    }
                    //z, so I can later replace floor height
                    for (int i = 0; i < levels; i++)
                    {
                        replacers[xvar_count] = ((i + 1) * lvlHeight).ToString();
                        xvar_count++;
                    }


                    //window ponits. created before, because i need window area
                    replacers[xvar_count] = win_L[0][0].ToString();
                    replacers[xvar_count + 1] = win_R[0][0].ToString();
                    replacers[xvar_count + 2] = win_L[0][1].ToString();
                    replacers[xvar_count + 3] = win_R[0][1].ToString();
                    replacers[xvar_count + 4] = win_L[1][0].ToString();
                    replacers[xvar_count + 5] = win_R[1][0].ToString();
                    replacers[xvar_count + 6] = win_L[1][1].ToString();
                    replacers[xvar_count + 7] = win_R[1][1].ToString();
                    replacers[xvar_count + 8] = win_L[2][0].ToString();
                    replacers[xvar_count + 9] = win_R[2][0].ToString();
                    replacers[xvar_count + 10] = win_L[2][1].ToString();
                    replacers[xvar_count + 11] = win_R[2][1].ToString();
                    replacers[xvar_count + 12] = win_L[3][0].ToString();
                    replacers[xvar_count + 13] = win_R[3][0].ToString();
                    replacers[xvar_count + 14] = win_L[3][1].ToString();
                    replacers[xvar_count + 15] = win_R[3][1].ToString();
                    xvar_count += 16;

                    for (int i = 0; i < levels; i++)
                    {
                        replacers[xvar_count] = (i * lvlHeight + 1.0).ToString();
                        replacers[xvar_count + 1] = (i * lvlHeight + lvlHeight - 0.5).ToString();
                        xvar_count += 2;
                    }





                    //scan string for keywords and replace them with parameters
                    for (int i = 0; i < lines.Length; i++)
                        for (int u = 0; u < replacethis.Length; u++)
                            lines[i] = lines[i].Replace(replacethis[u], replacers[u]);



                    //write a new idf file
                    string idffilenew = path_in + idfmodified + ".idf";
                    File.WriteAllLines(idffilenew, lines);


                    // add shading objects from three other buildings.
                    // 4 corner points of b != BLD
                    List<string> addtext = new List<string>();
                    for (int b = 0; b < 4; b++)
                    {
                        if (b != BLD)
                        {
                            // CounterClockWise important
                            //       ___        
                            //      /__/|      1__4
                            //      |__|/      |__|    points towards you
                            //                 2  3     
                            //
                            //         N            N
                            //      0_____3      4_____7  
                            //      |     |      |     |
                            //     W| btm |E    W| top |E
                            //      |_____|      |_____|
                            //      1     2      5     6
                            //         S            S

                            //srf south
                            addtext.Add(@"  Shading:Building:Detailed,");
                            addtext.Add(@"    BLDobstacle" + b + "_South,              !- Name");
                            addtext.Add(@"    ,                        !- Transmittance Schedule Name");
                            addtext.Add(@"    4,                       !- Number of Vertices");
                            addtext.Add(@"    " + Boxes[b][5].X + "," + Boxes[b][5].Y + "," + Boxes[b][5].Z + ",  !- X,Y,Z ==> Vertex 1 {m}");
                            addtext.Add(@"    " + Boxes[b][1].X + "," + Boxes[b][1].Y + "," + Boxes[b][1].Z + ",  !- X,Y,Z ==> Vertex 2 {m}");
                            addtext.Add(@"    " + Boxes[b][2].X + "," + Boxes[b][2].Y + "," + Boxes[b][2].Z + ",  !- X,Y,Z ==> Vertex 3 {m}");
                            addtext.Add(@"    " + Boxes[b][6].X + "," + Boxes[b][6].Y + "," + Boxes[b][6].Z + ";  !- X,Y,Z ==> Vertex 4 {m}");
                            addtext.Add(@" ");
                            //srf East
                            addtext.Add(@"  Shading:Building:Detailed,");
                            addtext.Add(@"    BLDobstacle" + b + "_East,              !- Name");
                            addtext.Add(@"    ,                        !- Transmittance Schedule Name");
                            addtext.Add(@"    4,                       !- Number of Vertices");
                            addtext.Add(@"    " + Boxes[b][6].X + "," + Boxes[b][6].Y + "," + Boxes[b][6].Z + ",  !- X,Y,Z ==> Vertex 1 {m}");
                            addtext.Add(@"    " + Boxes[b][2].X + "," + Boxes[b][2].Y + "," + Boxes[b][2].Z + ",  !- X,Y,Z ==> Vertex 2 {m}");
                            addtext.Add(@"    " + Boxes[b][3].X + "," + Boxes[b][3].Y + "," + Boxes[b][3].Z + ",  !- X,Y,Z ==> Vertex 3 {m}");
                            addtext.Add(@"    " + Boxes[b][7].X + "," + Boxes[b][7].Y + "," + Boxes[b][7].Z + ";  !- X,Y,Z ==> Vertex 4 {m}");
                            addtext.Add(@" ");
                            //srf North
                            addtext.Add(@"  Shading:Building:Detailed,");
                            addtext.Add(@"    BLDobstacle" + b + "_North,              !- Name");
                            addtext.Add(@"    ,                        !- Transmittance Schedule Name");
                            addtext.Add(@"    4,                       !- Number of Vertices");
                            addtext.Add(@"    " + Boxes[b][7].X + "," + Boxes[b][7].Y + "," + Boxes[b][7].Z + ",  !- X,Y,Z ==> Vertex 1 {m}");
                            addtext.Add(@"    " + Boxes[b][3].X + "," + Boxes[b][3].Y + "," + Boxes[b][3].Z + ",  !- X,Y,Z ==> Vertex 2 {m}");
                            addtext.Add(@"    " + Boxes[b][0].X + "," + Boxes[b][0].Y + "," + Boxes[b][0].Z + ",  !- X,Y,Z ==> Vertex 3 {m}");
                            addtext.Add(@"    " + Boxes[b][4].X + "," + Boxes[b][4].Y + "," + Boxes[b][4].Z + ";  !- X,Y,Z ==> Vertex 4 {m}");
                            addtext.Add(@" ");
                            //srf West
                            addtext.Add(@"  Shading:Building:Detailed,");
                            addtext.Add(@"    BLDobstacle" + b + "_West,              !- Name");
                            addtext.Add(@"    ,                        !- Transmittance Schedule Name");
                            addtext.Add(@"    4,                       !- Number of Vertices");
                            addtext.Add(@"    " + Boxes[b][4].X + "," + Boxes[b][4].Y + "," + Boxes[b][4].Z + ",  !- X,Y,Z ==> Vertex 1 {m}");
                            addtext.Add(@"    " + Boxes[b][0].X + "," + Boxes[b][0].Y + "," + Boxes[b][0].Z + ",  !- X,Y,Z ==> Vertex 2 {m}");
                            addtext.Add(@"    " + Boxes[b][1].X + "," + Boxes[b][1].Y + "," + Boxes[b][1].Z + ",  !- X,Y,Z ==> Vertex 3 {m}");
                            addtext.Add(@"    " + Boxes[b][5].X + "," + Boxes[b][5].Y + "," + Boxes[b][5].Z + ";  !- X,Y,Z ==> Vertex 4 {m}");
                            addtext.Add(@" ");
                            //srf Roof
                            addtext.Add(@"  Shading:Building:Detailed,");
                            addtext.Add(@"    BLDobstacle" + b + "_Roof,              !- Name");
                            addtext.Add(@"    ,                        !- Transmittance Schedule Name");
                            addtext.Add(@"    4,                       !- Number of Vertices");
                            addtext.Add(@"    " + Boxes[b][5].X + "," + Boxes[b][5].Y + "," + Boxes[b][5].Z + ",  !- X,Y,Z ==> Vertex 1 {m}");
                            addtext.Add(@"    " + Boxes[b][6].X + "," + Boxes[b][6].Y + "," + Boxes[b][6].Z + ",  !- X,Y,Z ==> Vertex 2 {m}");
                            addtext.Add(@"    " + Boxes[b][7].X + "," + Boxes[b][7].Y + "," + Boxes[b][7].Z + ",  !- X,Y,Z ==> Vertex 3 {m}");
                            addtext.Add(@"    " + Boxes[b][4].X + "," + Boxes[b][4].Y + "," + Boxes[b][4].Z + ";  !- X,Y,Z ==> Vertex 4 {m}");
                            addtext.Add(@" ");
                        }
                    }
                    using (StreamWriter sw = File.AppendText(idffilenew))
                    {
                        foreach (string addt in addtext)
                        {
                            sw.WriteLine(addt);
                        }
                    }








                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //run eplus
                    string weatherfilein = path_in + @"ep\WeatherData\" + weatherfile + ".epw";
                    string command = @" -w " + weatherfilein + @" -x -d " + path_out + @" -i " + path_in + @"ep\Energy+.idd " + idffilenew;
                    string directory = path_out;
                    Misc.RunEplus(eplusexe, command, directory);






                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //process Outputs
                    string outputfile = "eplustbl.csv";
                    while (!File.Exists(path_out + outputfile))
                    {
                        Console.WriteLine("waiting");
                    }



                    //output result 

                    //identify correct result file. load it. get the right numbers from it
                    lines = new string[] { };
                    list = new List<string>();
                    fileStream = new FileStream(path_out + outputfile, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream))
                    {
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            list.Add(line);
                        }
                    }
                    lines = list.ToArray();
                    fileStream.Close();

                    //GJ
                    //elec: line[64][2]
                    //cool: line[64][5]
                    //heat: line[64][6]

                    //m2: [41][2]

                    string[] split;
                    char delimiter = ',';

                    split = lines[41].Split(delimiter);
                    string bldarea = split[2];
                    split = lines[64].Split(delimiter);
                    string elec = split[2];
                    string cool = split[5];
                    string heat = split[6];
                    totElec[BLD] = Convert.ToDouble(elec) / 0.0036; //GJ to kWh
                    totHeat[BLD] = Convert.ToDouble(heat) / 0.0036;
                    totCool[BLD] = Convert.ToDouble(cool) / 0.0036;
                    totsqm[BLD] = Convert.ToDouble(bldarea);

                    //get peak cool and heat
                    // elec peak in W: lines[280][2]
                    // cool peak in W: lines[280][5]
                    // heat peak in W: lines[280][6]
                    split = lines[250 + levels * 5].Split(delimiter);
                    string elecpeak = split[2];
                    string coolpeak = split[5];
                    string heatpeak = split[6];
                    peakElec[BLD] = Convert.ToDouble(elecpeak) * 0.001; //kW
                    peakCool[BLD] = Convert.ToDouble(coolpeak) * 0.001;
                    peakHeat[BLD] = Convert.ToDouble(heatpeak) * 0.001;


                    System.Threading.Thread.Sleep(sleeptime);
                    System.IO.File.Delete(path_in + idfmodified + ".idf");
                    System.IO.DirectoryInfo di = new DirectoryInfo(path_out);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }


                //what's specific emission of fully maximized neighbourhood. take that, but reduced by quite a bit (enforcing daylight)


                //add system installation cost for boiler and AC, annualized
                double intrate = 0.08;
                double LifeBoi = 30;
                double LifeAC = 20;
                double CostBoi = 200; //per kW
                double CostAC = 360;  //per kW
                double annuityBoi = intrate / (1 - (1 / (Math.Pow((1 + intrate), (LifeBoi)))));
                double annuityAC = intrate / (1 - (1 / (Math.Pow((1 + intrate), (LifeAC)))));
                double c_Boi = CostBoi * annuityBoi;
                double c_AC = CostAC * annuityAC;




                double elecprice = 0.24;
                double gascost = 0.09;
                double coolConversion = 3;
                double boilereff = 0.94;
                double rent = 65; //chf per sqm


                //measure opeational carbon emissions. if it exceeds a certain value / m2, then penalty
                //ref heat: 100kWh/m2a
                //ref elec: 120kWh/m2a
                double carb_gas = 0.228; //per kWh gas
                double carb_grid = 0.594; //UCTE-mix, per kWh elec
                double sumEmissionsElec = 0;
                double sumEmissionsHeat = 0;
                double sumHeat = 0;
                double sumCool = 0;
                double sumElec = 0;
                double sumArea = 0;
                double sumPeakHeat = 0;
                double sumPeakCool = 0;

                double[] costperm2 = new double[4];


                for (int i = 0; i < 4; i++)
                {
                    sumEmissionsElec += carb_grid * totElec[i] + carb_grid * totCool[i] * coolConversion;
                    sumEmissionsHeat += carb_gas * (totHeat[i] / boilereff);
                    sumElec += totElec[i];
                    sumCool += totCool[i];
                    sumHeat += totHeat[i];
                    sumPeakCool += peakCool[i];
                    sumPeakHeat += peakHeat[i];
                    sumArea += totsqm[i];

                    costperm2[i] = (totElec[i] * elecprice + totCool[i] * coolConversion * elecprice + (totHeat[i] / boilereff) * gascost +
                        peakHeat[i] * c_Boi + peakCool[i] * c_AC) /
                        totsqm[i];

                }
                double specEmElec = sumEmissionsElec / sumArea;
                double specEmHeat = sumEmissionsHeat / sumArea;
                double specElec = sumElec / sumArea;
                double specCool = sumCool / sumArea;
                double specHeat = sumHeat / sumArea;





                double totCost =
                    sumElec * elecprice +
                    sumCool * coolConversion * elecprice +
                    (sumHeat / boilereff) * gascost +
                    sumPeakHeat * c_Boi +
                    sumPeakCool * c_AC
                    - sumArea * rent;

                //double carblimit = 96.5; //from SBE paper ,p.73
                //if (specEmElec + specEmHeat > carblimit) totCost += 1000000;



                DA.SetData(0, totCost);
                //DA.SetData(0, specEmHeat + specEmElec); //minimize per sqm carbon consumption
            }
        }



        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return GHEnergyPlus.Properties.Resources.opti_15;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{668c737e-b0c6-4fc6-be51-93f61fb3932b}"); }
        }
    }
}