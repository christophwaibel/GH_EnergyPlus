using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunParametric15 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHEPlusRunParametric15 class.
        /// </summary>
        public GHEPlusRunParametric15()
            : base("Prob15Waibel", "Prob15Waibel",
                "Problem 15 Waibel et al 2016, four office buildings, daylight, nat vent.",
                "EnergyHubs", "BuildingSimulation")
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
            pManager.AddIntegerParameter("BldAfloors", "x[0]", "Building A number of floors ∈ {1,...,6}. Floor height is 4m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldBfloors", "x[1]", "Building B number of floors ∈ {1,...,6}. Floor height is 4m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldCfloors", "x[2]", "Building C number of floors ∈ {1,...,6}. Floor height is 4m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldDfloors", "x[3]", "Building D number of floors ∈ {1,...,6}. Floor height is 4m.", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldA_X1", "x[4]", "Building A x-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y1", "x[5]", "Building A y-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X2", "x[6]", "Building A x-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y2", "x[7]", "Building A y-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X3", "x[8]", "Building A x-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y3", "x[9]", "Building A y-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X4", "x[10]", "Building A x-coordinate of cornerpoint 4, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y4", "x[11]", "Building A y-coordinate of cornerpoint 4, ∈ [0, 9.5].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldB_X1", "x[12]", "Building B x-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y1", "x[13]", "Building B y-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X2", "x[14]", "Building B x-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y2", "x[15]", "Building B y-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X3", "x[16]", "Building B x-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y3", "x[17]", "Building B y-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X4", "x[18]", "Building B x-coordinate of cornerpoint 4, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y4", "x[19]", "Building B y-coordinate of cornerpoint 4, ∈ [0, 9.5].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldC_X1", "x[20]", "Building C x-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y1", "x[21]", "Building C y-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X2", "x[22]", "Building C x-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y2", "x[23]", "Building C y-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X3", "x[24]", "Building C x-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y3", "x[25]", "Building C y-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X4", "x[26]", "Building C x-coordinate of cornerpoint 4, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y4", "x[27]", "Building C y-coordinate of cornerpoint 4, ∈ [0, 5.0].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldD_X1", "x[28]", "Building D x-coordinate of cornerpoint 1, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y1", "x[29]", "Building D y-coordinate of cornerpoint 1, ∈ [0, 5.0].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X2", "x[30]", "Building D x-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y2", "x[31]", "Building D y-coordinate of cornerpoint 2, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X3", "x[32]", "Building D x-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y3", "x[33]", "Building D y-coordinate of cornerpoint 3, ∈ [0, 9.5].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X4", "x[34]", "Building D x-coordinate of cornerpoint 4, ∈ [0, 9.5].", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Profit", "Profit", "Profit in CHF. Rent minus cost for decentralized energy system.", GH_ParamAccess.item);
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
            int dvar = 35;
            double[] x = new double[dvar];
            for (int i = 0; i < x.Length; i++)
                if (!DA.GetData(i + 6, ref x[i])) { return; };


            if (runit == true)
            {
                double lvlHeight = 4.0; // height per level
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
                // pt 1, x/y bounds. x: [0.5, 10.0]; y: [30.0, 39.5]
                // pt 2, x/y bounds. x: [0.5, 10.0]; y: [0.5, 10.0]
                // pt 3, x/y bounds. x: [30.0, 39.5]; y: [0.5, 10.0]
                // pt 4, x/y bounds. x: [30.0, 39.5]; y: [30.0, 39.5]
                double[] A_px_lb = new double[4];
                double[] A_py_lb = new double[4];
                A_px_lb[0] = 0.5;
                A_px_lb[1] = 0.5;
                A_px_lb[2] = 30;
                A_px_lb[3] = 30;
                A_py_lb[0] = 30;
                A_py_lb[1] = 0.5;
                A_py_lb[2] = 0.5;
                A_py_lb[3] = 30;


                // Bld B
                // pt 1, x/y bounds. x: [40.5, 50.0]; y: [30.0, 39.5]
                // pt 2, x/y bounds. x: [40.5, 50.0]; y: [0.5, 10.0]
                // pt 3, x/y bounds. x: [70.0, 79.5]; y: [0.5, 10.0]
                // pt 4, x/y bounds. x: [70.0, 79.5]; y: [30.0, 39.5]
                double[] B_px_lb = new double[4];
                double[] B_py_lb = new double[4];
                B_px_lb[0] = 40.5;
                B_px_lb[1] = 40.5;
                B_px_lb[2] = 70;
                B_px_lb[3] = 70;
                B_py_lb[0] = 30;
                B_py_lb[1] = 0.5;
                B_py_lb[2] = 0.5;
                B_py_lb[3] = 30;


                // Bld C
                // pt 1, x/y bounds. x: [0.5, 10.0]; y: [70.0, 79.5]
                // pt 2, x/y bounds. x: [0.5, 10.0]; y: [40.5, 50.0]
                // pt 3, x/y bounds. x: [30.0, 39.5]; y: [40.5, 50.0]
                // pt 4, x/y bounds. x: [30.0, 39.5]; y: [70.0, 75.0]
                double[] C_px_lb = new double[4];
                double[] C_py_lb = new double[4];
                C_px_lb[0] = 0.5;
                C_px_lb[1] = 0.5;
                C_px_lb[2] = 30;
                C_px_lb[3] = 30;
                C_py_lb[0] = 70;
                C_py_lb[1] = 40.5;
                C_py_lb[2] = 40.5;
                C_py_lb[3] = 70;

                // Bld D
                // pt 1, x/y bounds. x: [40.5, 50.0]; y: [70.0, 75.0]
                // pt 2, x/y bounds. x: [40.5, 50.0]; y: [40.5, 50.0]
                // pt 3, x/y bounds. x: [70.0, 79.5]; y: [40.5, 50.0]
                // pt 4, x/y bounds. x: [70.0, 79.5]; y: [65.0, 65.0]
                double[] D_px_lb = new double[4];
                double[] D_py_lb = new double[4];
                D_px_lb[0] = 40.5;
                D_px_lb[1] = 40.5;
                D_px_lb[2] = 70;
                D_px_lb[3] = 70;
                D_py_lb[0] = 70;
                D_py_lb[1] = 50;
                D_py_lb[2] = 50;
                D_py_lb[3] = 65;


                //_________________________________________________________________________
                ///////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////
                // INTERNAL WALLS
                // Bld A
                // pt 1, x/y bounds. x: [5.5, 15.0]; y: [25.0, 34.5]
                // pt 2, x/y bounds. x: [5.5, 15.0]; y: [5.5, 15.0]
                // pt 3, x/y bounds. x: [25.0, 34.5]; y: [5.5, 15.0]
                // pt 4, x/y bounds. x: [25.0, 34.5]; y: [25.0, 34.5]
                double[] Ain_px_lb = new double[4];
                double[] Ain_py_lb = new double[4];
                Ain_px_lb[0] = 5.5;
                Ain_px_lb[1] = 5.5;
                Ain_px_lb[2] = 25;
                Ain_px_lb[3] = 25;
                Ain_py_lb[0] = 25;
                Ain_py_lb[1] = 5.5;
                Ain_py_lb[2] = 5.5;
                Ain_py_lb[3] = 25;

                // Bld B
                // pt 1, x/y bounds. x: [45.5, 55.0]; y: [25.0, 34.5]
                // pt 2, x/y bounds. x: [45.5, 55.0]; y: [5.5, 15.0]
                // pt 3, x/y bounds. x: [65.0, 74.5]; y: [5.5, 15.0]
                // pt 4, x/y bounds. x: [65.0, 74.5]; y: [25.0, 34.5]
                double[] Bin_px_lb = new double[4];
                double[] Bin_py_lb = new double[4];
                Bin_px_lb[0] = 45.5;
                Bin_px_lb[1] = 45.5;
                Bin_px_lb[2] = 65;
                Bin_px_lb[3] = 65;
                Bin_py_lb[0] = 25;
                Bin_py_lb[1] = 5.5;
                Bin_py_lb[2] = 5.5;
                Bin_py_lb[3] = 25;

                // Bld C
                // pt 1, x/y bounds. x: [5.5, 15.0]; y: [64.39, 73.89]
                // pt 2, x/y bounds. x: [5.5, 15.0]; y: [45.5, 55.0]
                // pt 3, x/y bounds. x: [25.0, 34.5]; y: [45.5, 55.0]
                // pt 4, x/y bounds. x: [25.0, 34.5]; y: [65.54, 70.54]
                double[] Cin_px_lb = new double[4];
                double[] Cin_py_lb = new double[4];
                Cin_px_lb[0] = 5.5;
                Cin_px_lb[1] = 5.5;
                Cin_px_lb[2] = 25;
                Cin_px_lb[3] = 25;
                Cin_py_lb[0] = 64.39;
                Cin_py_lb[1] = 45.5;
                Cin_py_lb[2] = 45.5;
                Cin_py_lb[3] = 65.54;

                // Bld D
                // pt 1, x/y bounds. x: [45.5, 55.0]; y: [63.56, 68.56]
                // pt 2, x/y bounds. x: [45.5, 55.0]; y: [45.5, 55.0]
                // pt 3, x/y bounds. x: [65.0, 74.5]; y: [45.5, 55.0]
                // pt 4, x/y bounds. x: [65.0, 74.5]; y: [61.12, 61.12]
                double[] Din_px_lb = new double[4];
                double[] Din_py_lb = new double[4];
                Din_px_lb[0] = 45.5;
                Din_px_lb[1] = 45.5;
                Din_px_lb[2] = 65;
                Din_px_lb[3] = 65;
                Din_py_lb[0] = 63.56;
                Din_py_lb[1] = 45.5;
                Din_py_lb[2] = 45.5;
                Din_py_lb[3] = 61.12;


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
                    if (i < 3) D_py[i] = x[28 + step + 1] + D_py_lb[i];
                    else D_py[i] = D_py_lb[i];

                    Ain_px[i] = x[4 + step] + Ain_px_lb[i];
                    Ain_py[i] = x[4 + step + 1] + Ain_py_lb[i];
                    Bin_px[i] = x[12 + step] + Bin_px_lb[i];
                    Bin_py[i] = x[12 + step + 1] + Bin_py_lb[i];
                    Cin_px[i] = x[20 + step] + Cin_px_lb[i];
                    Cin_py[i] = x[20 + step + 1] + Cin_py_lb[i];
                    Din_px[i] = x[28 + step] + Din_px_lb[i];
                    if (i < 3) Din_py[i] = x[28 + step + 1] + Din_py_lb[i];
                    else Din_py[i] = Din_py_lb[i];
                    step += 2;
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



                //***********************************************************************************
                //***********************************************************************************
                //***********************************************************************************
                //modify idf file with parameters and save as new idf file
                //string idfmodified = idffile + "_modi";

                double rent = 70; //chf per sqm
                double[] totenergy = new double[4];
                double[] totsqm = new double[4];

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

                    for (int l = 0; l < levels; l++)
                    {

                    }
                    // for each zone and level
                    //   
                    // Daylighting:Controls:                    %DLx_p0%, %DLy_p0%, %DLz_p0%,   : center of each floor (Perimeter0 to levels*4-1)
                    // ZoneVentilation:WindandStackOpenArea:    %Vent_f0%                       : opening area. 50% of window area (0 to levels*4-1)
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


                    string[] replacethis = new string[dvar + levels*100];       //35 variables, plus levels times variables that need to be replaced

                    replacethis[0] = @"%x0%"; //infiltration
                    int xvar_count = 1;
                    for (int i = 1; i < 13; i++)        //aspect ratio
                    {
                        replacethis[xvar_count] = @"%x1_p" + i.ToString() + @"_x%";
                        replacethis[xvar_count + 1] = @"%x1_p" + i.ToString() + @"_y%";
                        replacethis[xvar_count + 2] = @"%x1_p" + i.ToString() + @"_z%";
                        xvar_count += 3;
                    }
                    for (int i = 1; i < 9; i++)
                    {
                        replacethis[xvar_count] = @"%x1_pi" + i.ToString() + @"_x%";
                        replacethis[xvar_count + 1] = @"%x1_pi" + i.ToString() + @"_y%";
                        replacethis[xvar_count + 2] = @"%x1_pi" + i.ToString() + @"_z%";
                        xvar_count += 3;
                    }
                    replacethis[xvar_count] = @"%x2%"; // u-value
                    xvar_count++;

                    replacethis[xvar_count] = @"%x3_xstart%";         // fenestration xstart, zstart, length, height. North
                    replacethis[xvar_count + 1] = @"%x3_zstart%";
                    replacethis[xvar_count + 2] = @"%x3_length%";
                    replacethis[xvar_count + 3] = @"%x3_height%";
                    replacethis[xvar_count + 4] = @"%x4_xstart%";     // South
                    replacethis[xvar_count + 5] = @"%x4_zstart%";
                    replacethis[xvar_count + 6] = @"%x4_length%";
                    replacethis[xvar_count + 7] = @"%x4_height%";
                    replacethis[xvar_count + 8] = @"%x5_xstart%";     // East
                    replacethis[xvar_count + 9] = @"%x5_zstart%";
                    replacethis[xvar_count + 10] = @"%x5_length%";
                    replacethis[xvar_count + 11] = @"%x5_height%";
                    replacethis[xvar_count + 12] = @"%x6_xstart%";    // West
                    replacethis[xvar_count + 13] = @"%x6_zstart%";
                    replacethis[xvar_count + 14] = @"%x6_length%";
                    replacethis[xvar_count + 15] = @"%x6_height%";
                    xvar_count += 16;

                    for (int i = 7; i < dvar; i++)
                    {
                        replacethis[xvar_count] = @"%x" + i.ToString() + @"%";
                        xvar_count++;
                    }



                    //replacers
                    string[] replacers = new string[replacethis.Length];
                    replacers[0] = x[0].ToString();

                    double floor_area = 70; //according to paper. but in his file its 100 though
                    double[][] x1_p;
                    double[][] x1_pi;
                    Misc.insert_surface(out x1_p, out x1_pi, floor_area, x[1]);

                    xvar_count = 1;
                    for (int i = 0; i < 12; i++)        //aspect ratio
                    {
                        replacers[xvar_count] = x1_p[i][0].ToString();
                        replacers[xvar_count + 1] = x1_p[i][1].ToString();
                        replacers[xvar_count + 2] = x1_p[i][2].ToString();
                        xvar_count += 3;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        replacers[xvar_count] = x1_pi[i][0].ToString();
                        replacers[xvar_count + 1] = x1_pi[i][1].ToString();
                        replacers[xvar_count + 2] = x1_pi[i][2].ToString();
                        xvar_count += 3;
                    }
                    replacers[xvar_count] = x[2].ToString();
                    xvar_count++;

                    double[] xstart;
                    double[] zstart;
                    double[] length;
                    double[] height;
                    Misc.insert_window(out xstart, out zstart, out length, out height, x1_p, new double[4] { x[3], x[4], x[5], x[6] });

                    replacers[xvar_count] = xstart[0].ToString();         // fenestration xstart, zstart, length, height. North
                    replacers[xvar_count + 1] = zstart[0].ToString();
                    replacers[xvar_count + 2] = length[0].ToString();
                    replacers[xvar_count + 3] = height[0].ToString();
                    replacers[xvar_count + 4] = xstart[1].ToString();     // South
                    replacers[xvar_count + 5] = zstart[1].ToString();
                    replacers[xvar_count + 6] = length[1].ToString();
                    replacers[xvar_count + 7] = height[1].ToString();
                    replacers[xvar_count + 8] = xstart[2].ToString();     // East
                    replacers[xvar_count + 9] = zstart[2].ToString();
                    replacers[xvar_count + 10] = length[2].ToString();
                    replacers[xvar_count + 11] = height[2].ToString();
                    replacers[xvar_count + 12] = xstart[3].ToString();    // West
                    replacers[xvar_count + 13] = zstart[3].ToString();
                    replacers[xvar_count + 14] = length[3].ToString();
                    replacers[xvar_count + 15] = height[3].ToString();
                    xvar_count += 16;

                    switch (Convert.ToInt16(x[7]))
                    {
                        case 1:
                            replacers[xvar_count] = "Wall_1";
                            break;
                        case 2:
                            replacers[xvar_count] = "Wall_2";
                            break;
                        case 3:
                            replacers[xvar_count] = "Wall_3";
                            break;
                        case 4:
                            replacers[xvar_count] = "Wall_4";
                            break;
                    }
                    xvar_count++;
                    replacers[xvar_count] = (x[8] / 1000).ToString();
                    xvar_count++;

                    for (int i = 9; i < dvar; i++)
                    {
                        replacers[xvar_count] = x[i].ToString();
                        xvar_count++;
                    }



                    //scan string for keywords and replace them with parameters
                    for (int i = 0; i < lines.Length; i++)
                        for (int u = 0; u < replacethis.Length; u++)
                            lines[i] = lines[i].Replace(replacethis[u], replacers[u]);




                    //write a new idf file
                    File.WriteAllLines(path_in + idfmodified + ".idf", lines);
                    string idffilenew = path_in + idfmodified + ".idf";
                    string weatherfilein = path_in + @"ep\WeatherData\" + weatherfile + ".epw";





                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //run eplus
                    string command = @" -w " + weatherfilein + @" -x -d " + path_out + @" -i " + path_in + @"ep\Energy+.idd " + idffilenew;
                    string directory = path_out;
                    Misc.RunEplus(eplusexe, command, directory);






                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //process Outputs
                    string outputfile = "eplusout.eso";
                    while (!File.Exists(path_out + outputfile))
                    {
                        Console.WriteLine("waiting");
                    }



                    //output result 
                    totenergy[BLD] = double.NaN;
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

                    double Fx;


                    string[] split;
                    char delimiter = ',';

                    split = lines[11].Split(delimiter);
                    string heat = split[1];
                    split = lines[12].Split(delimiter);
                    string cool = split[1];
                    double dblheat = 0.001 * Convert.ToDouble(heat) / 3600;
                    double dblcool = 0.001 * Convert.ToDouble(cool) / 3600 * 3;

                    Fx = dblheat + dblcool;

                    
                    totenergy[BLD] = Fx;
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


                //get all results of four buildings together
                double result = totenergy[0] + totenergy[1] + totenergy[2] + totenergy[3];

                DA.SetData(0, result);
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
            get { return new Guid("{d9f6638f-2119-481c-a178-46bda59b2e5b}"); }
        }
    }
}