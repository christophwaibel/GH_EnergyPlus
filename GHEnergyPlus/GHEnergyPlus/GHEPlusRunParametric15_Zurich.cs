using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunParametric15_Zurich : GH_Component
    {
         /// <summary>
        /// Initializes a new instance of the GHEPlusRunParametric15 class.
        /// </summary>
        public GHEPlusRunParametric15_Zurich()
            : base("Prob15_Zurich", "Prob15_Zurich",
                "Problem 15 Waibel et al 2016, four office buildings, daylight, nat vent. in Zurich. Case Study 1 and 3 for Thesis and Holistic Paper.",
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
            pManager.AddIntegerParameter("BldAfloors", "x[0]", "Building A number of floors ∈ {1,...,6}. Floor height is 4 m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldBfloors", "x[1]", "Building B number of floors ∈ {1,...,6}. Floor height is 4 m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldCfloors", "x[2]", "Building C number of floors ∈ {1,...,6}. Floor height is 4 m.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("BldDfloors", "x[3]", "Building D number of floors ∈ {1,...,6}. Floor height is 4 m.", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldA_X1", "x[4]", "Building A x-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y1", "x[5]", "Building A y-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X2", "x[6]", "Building A x-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y2", "x[7]", "Building A y-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X3", "x[8]", "Building A x-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y3", "x[9]", "Building A y-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_X4", "x[10]", "Building A x-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldA_Y4", "x[11]", "Building A y-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldB_X1", "x[12]", "Building B x-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y1", "x[13]", "Building B y-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X2", "x[14]", "Building B x-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y2", "x[15]", "Building B y-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X3", "x[16]", "Building B x-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y3", "x[17]", "Building B y-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_X4", "x[18]", "Building B x-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldB_Y4", "x[19]", "Building B y-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldC_X1", "x[20]", "Building C x-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y1", "x[21]", "Building C y-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X2", "x[22]", "Building C x-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y2", "x[23]", "Building C y-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X3", "x[24]", "Building C x-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y3", "x[25]", "Building C y-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_X4", "x[26]", "Building C x-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldC_Y4", "x[27]", "Building C y-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);

            pManager.AddNumberParameter("BldD_X1", "x[28]", "Building D x-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y1", "x[29]", "Building D y-coordinate of cornerpoint 1, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X2", "x[30]", "Building D x-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y2", "x[31]", "Building D y-coordinate of cornerpoint 2, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X3", "x[32]", "Building D x-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y3", "x[33]", "Building D y-coordinate of cornerpoint 3, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_X4", "x[34]", "Building D x-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);
            pManager.AddNumberParameter("BldD_Y4", "x[35]", "Building D y-coordinate of cornerpoint 4, ∈ [0, 1].", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Elec", "Elec", "Electricity demand, 8760 time series, in kW.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Cool", "Cool", "Cooling demand,  8760 time series, in kW.", GH_ParamAccess.list);
            pManager.AddNumberParameter("DHW", "DHW", "Domestic Hot Water demand, 8760 time series, in kW.", GH_ParamAccess.list);
            pManager.AddNumberParameter("SH", "SH", "Space Heating demand, 8760 time series, in kW", GH_ParamAccess.list);
            pManager.AddNumberParameter("Area", "Area", "Total floor area, in square meters.", GH_ParamAccess.item);
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

            // settings
            double lvlHeight = 4.0; // height per level
            double offset = 5.0; // depth of perimeter zones

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

                double[,] CTransf_D1 = new double[,]
                {
                    {94.8077,    0.0000,   42.5000},
                    {120.1538,    3.5000,   71.0000},
                    {1.6923,    0.0000,    1.0000}
                };
                double[,] CTransf_D4 = new double[,]
                {
                    {358.3000,    0.0000,   71.0000},
                    {283.8000,    2.7000,   64.5000},
                    {4.4000,    0.0000,    1.0000}
                };
                double[,] CTransf_D3 = new double[,]
                {
                    {8.5000,    0.0000,   71.0000},
                    {0,    8.5000,   42.5000},
                    {0,    0.0000,    1.0000}
                };
                double[,] CTransf_D2 = new double[,]
                {
                    {8.5000,   -0.0000,   42.5000},
                    {-0.0000,    8.5000,   42.5000},
                    {-0.0000,   -0.0000,    1.0000}
                };


                double[,] CTransf_A1 = new double[,]
                {
                    {8.5,   0.0,    0.5},
                    {0.0,   8.5,    29.0},
                    {0.0,   0.0,    1.0}
                };
                double[,] CTransf_A4 = new double[,]
                {
                    {8.5,   0.0,    29.0},
                    {0.0,   8.5,    29.0},
                    {0.0,   0.0,    1.0}
                };
                double[,] CTransf_A3 = new double[,]
                {
                    {8.5,   -0.0,   29.0},
                    {0.0,   8.5,    0.5},
                    {0.0 ,  -0.0,   1.0}
                };
                double[,] CTransf_A2 = new double[,]
                {
                    {8.5,   0.0,    0.5},
                    {0.0,   8.5,    0.5},
                    {0.0,   0.0,    1.0}
                };


                double[,] CTransf_B1 = new double[,]
                {
                    {8.5000,         0,   42.5000},
                    {0.0000,    8.5000,  29.0000},
                    {0.0000,         0,    1.0000}
                };
                double[,] CTransf_B4 = new double[,]
                {
                    {8.5000,    0.0000,   71.0000},
                    {0.0000,    8.5000,   29.0000},
                    {0.0000,    0.0000,    1.0000}
                };
                double[,] CTransf_B3 = new double[,]
                {
                    {8.5000,   -0.0000,   71.0000},
                    {0,    8.5000,   0.5000},
                    {0,   -0.0000,   1.0000}
                };
                double[,] CTransf_B2 = new double[,]
                {
                    {8.5000,   0.0000,   42.5000},
                    {0.0000,    8.5000,  0.5000},
                    {0.0000,  0.0000,    1.0000}
                };


                double[,] CTransf_C1 = new double[,]
                {
                    {9.7000,   -0.0000,    0.5000},
                    {9.4667,    8.5000,   71.0000},
                    {0.1333,   -0.0000,    1.0000}
                };
                double[,] CTransf_C4 = new double[,]
                {
                    {17.4286,         0,   29.0000},
                    {16.9048,    5.2000,   71.0000},
                    {0.2381,         0,    1.0000}
                };
                double[,] CTransf_C3 = new double[,]
                {
                    {8.5000,    0.0000,   29.0000},
                    {0,    8.5000,   42.5000},
                    {0.0000,    0.0000,    1.0000}
                };
                double[,] CTransf_C2 = new double[,]
                {
                    {8.5000,         0,    0.5000},
                    {0,    8.5000,   42.5000},
                    {0,         0,    1.0000}
                };


                double[][,] CA = new double[4][,];
                double[][,] CB = new double[4][,];
                double[][,] CC = new double[4][,];
                double[][,] CD = new double[4][,];
                CA[0] = CTransf_A1;
                CA[1] = CTransf_A2;
                CA[2] = CTransf_A3;
                CA[3] = CTransf_A4;
                CB[0] = CTransf_B1;
                CB[1] = CTransf_B2;
                CB[2] = CTransf_B3;
                CB[3] = CTransf_B4;
                CC[0] = CTransf_C1;
                CC[1] = CTransf_C2;
                CC[2] = CTransf_C3;
                CC[3] = CTransf_C4;
                CD[0] = CTransf_D1;
                CD[1] = CTransf_D2;
                CD[2] = CTransf_D3;
                CD[3] = CTransf_D4;



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

                double[][] A_p = new double[4][];
                double[][] B_p = new double[4][];
                double[][] C_p = new double[4][];
                double[][] D_p = new double[4][];

                for (int i = 0; i < 4; i++)
                {
                    double[] pin = new double[3];

                    pin[0] = x[4 + step];
                    pin[1] = x[4 + step + 1];
                    pin[2] = 1;
                    A_p[i] = Misc.TransformPoints(CA[i], pin);
                    A_px[i] = A_p[i][0];
                    A_py[i] = A_p[i][1];

                    pin[0] = x[12 + step];
                    pin[1] = x[12 + step + 1];
                    pin[2] = 1;
                    B_p[i] = Misc.TransformPoints(CB[i], pin);
                    B_px[i] = B_p[i][0];
                    B_py[i] = B_p[i][1];

                    pin[0] = x[20 + step];
                    pin[1] = x[20 + step + 1];
                    pin[2] = 1;
                    C_p[i] = Misc.TransformPoints(CC[i], pin);
                    C_px[i] = C_p[i][0];
                    C_py[i] = C_p[i][1];

                    pin[0] = x[28 + step];
                    pin[1] = x[28 + step + 1];
                    pin[2] = 1;
                    D_p[i] = Misc.TransformPoints(CD[i], pin);
                    D_px[i] = D_p[i][0];
                    D_py[i] = D_p[i][1];

                    step += 2;
                }


                List<Point3d> plist = new List<Point3d>();
                plist.Add(new Point3d(A_px[0], A_py[0], 0));
                plist.Add(new Point3d(A_px[1], A_py[1], 0));
                plist.Add(new Point3d(A_px[2], A_py[2], 0));
                plist.Add(new Point3d(A_px[3], A_py[3], 0));
                double[][] A_pts_offset = Misc.PtsFromOffsetRectangle(plist,offset);

                plist = new List<Point3d>();
                plist.Add(new Point3d(B_px[0], B_py[0], 0));
                plist.Add(new Point3d(B_px[1], B_py[1], 0));
                plist.Add(new Point3d(B_px[2], B_py[2], 0));
                plist.Add(new Point3d(B_px[3], B_py[3], 0));
                double[][] B_pts_offset = Misc.PtsFromOffsetRectangle(plist, offset);

                plist = new List<Point3d>();
                plist.Add(new Point3d(C_px[0], C_py[0], 0));
                plist.Add(new Point3d(C_px[1], C_py[1], 0));
                plist.Add(new Point3d(C_px[2], C_py[2], 0));
                plist.Add(new Point3d(C_px[3], C_py[3], 0));
                double[][] C_pts_offset = Misc.PtsFromOffsetRectangle(plist, offset);

                plist = new List<Point3d>();
                plist.Add(new Point3d(D_px[0], D_py[0], 0));
                plist.Add(new Point3d(D_px[1], D_py[1], 0));
                plist.Add(new Point3d(D_px[2], D_py[2], 0));
                plist.Add(new Point3d(D_px[3], D_py[3], 0));
                double[][] D_pts_offset = Misc.PtsFromOffsetRectangle(plist, offset);

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

                List<double> out_elec = new List<double>(8760);
                List<double> out_cool = new List<double>(8760);
                List<double> out_dhw = new List<double>(8760);
                List<double> out_sh = new List<double>(8760);

                //double[] totElec = new double[4];
                //double[] totCool = new double[4];
                //double[] totHeat = new double[4];
                double[] totsqm = new double[4];
                //double[] peakElec = new double[4];
                //double[] peakCool = new double[4];
                //double[] peakHeat = new double[4];

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
                        replacers[xvar_count] = ((area * 0.5) / 5.0).ToString();    // 50% of the window is openable. and since I will have 5 separably controllable windows per facade, divide area by 5
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

                    //m2: [41][2]

                    string[] split;
                    char delimiter = ',';

                    split = lines[41].Split(delimiter);
                    string bldarea = split[2];
                    split = lines[64].Split(delimiter);
                    //string elec = split[2];
                    //string cool = split[5];
                    //string heat = split[6];
                    //totElec[BLD] = Convert.ToDouble(elec) / 0.0036; //GJ to kWh
                    //totHeat[BLD] = Convert.ToDouble(heat) / 0.0036;
                    //totCool[BLD] = Convert.ToDouble(cool) / 0.0036;
                    totsqm[BLD] = Convert.ToDouble(bldarea);

                    ////get peak cool and heat
                    //// elec peak in W: lines[280][2]
                    //// cool peak in W: lines[280][5]
                    //// heat peak in W: lines[280][6]
                    //split = lines[250 + levels * 5].Split(delimiter);
                    //string elecpeak = split[2];
                    //string coolpeak = split[5];
                    //string heatpeak = split[6];
                    //peakElec[BLD] = Convert.ToDouble(elecpeak) * 0.001; //kW
                    //peakCool[BLD] = Convert.ToDouble(coolpeak) * 0.001;
                    //peakHeat[BLD] = Convert.ToDouble(heatpeak) * 0.001;


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


                

                double out_sumArea = 0;
                for (int i = 0; i < 4; i++)
                {
                    out_sumArea += totsqm[i];
                }


                // OUTPUTS FOR ENERGYHUB
                DA.SetDataList(0, out_elec);  //total electricity demand, aggregated for all 4 buildings
                DA.SetDataList(1, out_cool);
                DA.SetDataList(2, out_dhw);     // hot water according to schedule and square meters
                DA.SetDataList(3, out_sh);
                DA.SetData(4, out_sumArea); // total floor area, for rent calcualtion, in ehub module.

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
            get { return new Guid("14bddafd-047a-4290-9752-e93fd38df449"); }
        }
    }
}