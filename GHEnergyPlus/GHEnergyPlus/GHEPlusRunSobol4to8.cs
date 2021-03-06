﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunSobol4to8 : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GHEPlusRunSobol4to8()
            : base("WetterDetailedSobol", "WetterDetailedSobol",
                "Run Sobol Sequence on WetterDetailed, save results as text file",
                "EnergyHubs", "BuildingSimulation")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("idf file", "idf file", "idf file. has to be in C:\\eplus\\EPOpti17\\Input\\", GH_ParamAccess.item);
            pManager.AddTextParameter("weather file", "weather file", "weather file. has to be in WeatherData folder of your Energyplus folder", GH_ParamAccess.item);
            pManager.AddTextParameter("Path LPt sequence", "Path LPt sequence",
                "Path to Sobol sequence csv file. That should be a matrix with samples per row and parameters in columns. Create sequence e.g. in matlab.",
                GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "run EnergyPlus", GH_ParamAccess.item);

            pManager.AddIntegerParameter("folder", "folder", "folder number, like 1,2,3, for parallel runs", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {


        }

        /// <summary>
        /// Loads the xls file, applies that to given idf file and completes all samples. Outputs a text file with kWh/m2a for each sample.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int folderint = 0;
            if (!DA.GetData(4, ref folderint)) { folderint = 0; }
            string path_in = @"c:\eplus\EPOpti17\Input" + folderint + @"\";
            string path_out = @"c:\eplus\EPOpti17\Output" + folderint + @"\";
            string eplusexe = @"c:\eplus\EPOpti17\Input" + folderint + @"\ep\energyplus.exe";


            string idffile = @"blabla";
            if (!DA.GetData(0, ref idffile)) { return; }

            string weatherfile = @"blabla";
            if (!DA.GetData(1, ref weatherfile)) { return; }

            string sobolpath = @"blabla";
            if (!DA.GetData(2, ref sobolpath)) { return; }

            bool runit = false;
            if (!DA.GetData(3, ref runit)) { return; }


            if (runit == true)
            {
                //load sobol sequence xls into a list
                //first into a string
                string[] lines;
                List<double[]> sobparameterset = new List<double[]>();

                var list = new List<string>();
                var fileStream = new FileStream(sobolpath, FileMode.Open, FileAccess.Read);
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

                //now split every line with ; as delimiter
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] split;
                    double[] numbers = new double[13];
                    char delimiter = ';';
                    split = lines[i].Split(delimiter);
                    for (int u = 0; u < split.Length; u++)
                    {
                        numbers[u] = Convert.ToDouble(split[u]);
                    }
                    sobparameterset.Add(numbers);
                }

                int sobSamples = sobparameterset.Count;

                double[] sobResults = new double[sobSamples];
                string[] sobResultsStr = new string[sobSamples];
                //for each sample
                for (int j = 0; j < sobparameterset.Count; j++)
                {
                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //modify idf file with parameters and save as new idf file
                    //string now = DateTime.Now.ToString("h:mm:ss");
                    //now = now.Replace(':', '_');
                    //string now = j.ToString();
                    string idfmodified = idffile + "_modi";

                    //load idf into a huge string
                    lines = new string[]{};
                    list = new List<string>();
                    fileStream = new FileStream(path_in + idffile + ".idf", FileMode.Open, FileAccess.Read);
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
                    string[] replacethis = new string[29];
                    replacethis[0] = @"%x0WinNor%";
                    replacethis[1] = @"%x1WinNor%";
                    replacethis[2] = @"%y0WinNor%";
                    replacethis[3] = @"%x0OveNor%";
                    replacethis[4] = @"%x1OveNor%";

                    replacethis[5] = @"%x0WinWes%";
                    replacethis[6] = @"%x1WinWes%";
                    replacethis[7] = @"%y0WinWes%";
                    replacethis[8] = @"%x0OveWes%";
                    replacethis[9] = @"%x1OveWes%";
                    replacethis[10] = @"%yOveWes%";

                    replacethis[11] = @"%x0WinEas%";
                    replacethis[12] = @"%x1WinEas%";
                    replacethis[13] = @"%y0WinEas%";
                    replacethis[14] = @"%x0OveEas%";
                    replacethis[15] = @"%x1OveEas%";
                    replacethis[16] = @"%yOveEas%";

                    replacethis[17] = @"%x0WinSou%";
                    replacethis[18] = @"%x1WinSou%";
                    replacethis[19] = @"%y0WinSou%";
                    replacethis[20] = @"%x0OveSou%";
                    replacethis[21] = @"%x1OveSou%";
                    replacethis[22] = @"%yOveSou%";

                    replacethis[23] = @"%shaWes%";
                    replacethis[24] = @"%shaEas%";
                    replacethis[25] = @"%shaSou%";

                    replacethis[26] = @"%TZonCooNigSum%";
                    replacethis[27] = @"%TZonCooNigWin%";
                    replacethis[28] = @"%CooDesSupTem%";


                    //replacers
                    string[] replacers = new string[29];
                    double wn = sobparameterset[j][0] * 4.608 + 1.224;
                    replacers[0] = (3.0 - (wn / 2.0)).ToString();       //x0
                    replacers[1] = (3.0 + (wn / 2.0)).ToString();       //x1
                    replacers[2] = (0.6).ToString();                    //y0
                    replacers[3] = (3.0 - (wn / 2.0) - 0.5).ToString(); //x0 over
                    replacers[4] = (3.0 + (wn / 2.0) + 0.5).ToString(); //x1 over

                    double ww = sobparameterset[j][1] * 18.324 + 7.344;
                    double ow = sobparameterset[j][2] + 8.05;
                    replacers[5] = (12.0 - (ww / 2.0)).ToString();      //x0
                    replacers[6] = (12.0 + (ww / 2.0)).ToString();      //x1
                    replacers[7] = (0.6).ToString();                    //y0
                    replacers[8] = (12.0 - (ww / 2.0) - 0.5).ToString();//x0 over
                    replacers[9] = (12.0 + (ww / 2.0) + 0.5).ToString();//x1 over
                    replacers[10] = ow.ToString();

                    double we = sobparameterset[j][3] * 18.324 + 7.344;
                    double oe = sobparameterset[j][4] + 8.05;
                    replacers[11] = (12.0 - (we / 2.0)).ToString();      //x0
                    replacers[12] = (12.0 + (we / 2.0)).ToString();      //x1
                    replacers[13] = (0.6).ToString();                    //y0
                    replacers[14] = (12.0 - (we / 2.0) - 0.5).ToString();//x0 over
                    replacers[15] = (12.0 + (we / 2.0) + 0.5).ToString();//x1 over
                    replacers[16] = oe.ToString();

                    double ws = sobparameterset[j][5] * 4.608 + 1.224;
                    double os = sobparameterset[j][6] + 8.05;
                    replacers[17] = (3.0 - (ws / 2.0)).ToString();       //x0
                    replacers[18] = (3.0 + (ws / 2.0)).ToString();       //x1
                    replacers[19] = (0.6).ToString();                    //y0
                    replacers[20] = (3.0 - (ws / 2.0) - 0.5).ToString(); //x0 over
                    replacers[21] = (3.0 + (ws / 2.0) + 0.5).ToString(); //x1 over
                    replacers[22] = os.ToString();

                    double sw = sobparameterset[j][7] * 500.0 + 100.0;
                    double se = sobparameterset[j][8] * 500.0 + 100.0;
                    double ss = sobparameterset[j][9] * 500.0 + 100.0;
                    replacers[23] = sw.ToString();
                    replacers[24] = se.ToString();
                    replacers[25] = ss.ToString();

                    double tu = sobparameterset[j][10] * 5.0 + 20.0;
                    double ti = sobparameterset[j][11] * 5.0 + 20.0;
                    double td = sobparameterset[j][12] * 6.0 + 12.0;
                    replacers[26] = tu.ToString();
                    replacers[27] = ti.ToString();
                    replacers[28] = td.ToString();


                    //scan string for keywords and replace them with parameters
                    for (int i = 0; i < lines.Length; i++)
                    {
                        for (int u = 0; u < replacethis.Length; u++)
                        {
                            lines[i] = lines[i].Replace(replacethis[u], replacers[u]);
                        }
                    }


                    //write a new idf file
                    File.WriteAllLines(path_in + idfmodified + ".idf", lines);
                    string idffilenew = path_in + idfmodified + ".idf";
                    string weatherfilein = path_in + @"ep\WeatherData\" + weatherfile + ".epw";




                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //run eplus
                    string command = @" -w " + weatherfilein + @" -d " + path_out + @" " + idffilenew;
                    Misc.RunEplus(eplusexe, command);









                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //while (!File.Exists(path_out + idfmodified + "Table.csv"))
                    while (!File.Exists(path_out + "eplusout.eso"))
                    {
                        Console.WriteLine("waiting");
                    }
                    //System.Threading.Thread.Sleep(1500);


                    //output result (kWh/m2a) 
                    double result = double.NaN;
                    //identify correct result file. load it. get the right numbers from it
                    lines = new string[] { };
                    list = new List<string>();
                    //fileStream = new FileStream(path_out + idfmodified + "Table.csv", FileMode.Open, FileAccess.ReadWrite);
                    fileStream = new FileStream(path_out + "eplusout.eso", FileMode.Open, FileAccess.ReadWrite); //reading the eso file to be conform with what WetterWright used. somehow, there is a difference in lighting energy .eso comapred to table.csv
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


                    double primEnElec = 3.0;
                    double primEnGas = 1.0;

                    string[] split;
                    char delimiter = ',';
                    //split = lines[49].Split(delimiter);
                    //string heat = split[3];
                    //double dblHeat = Convert.ToDouble(heat) * primEnGas;

                    //split = lines[50].Split(delimiter);
                    //string cool = split[2];
                    //double dblCool = Convert.ToDouble(cool) * primEnElec;

                    //split = lines[51].Split(delimiter);
                    //string light = split[2];
                    //double dblLight = Convert.ToDouble(light) * primEnElec;

                    //split = lines[55].Split(delimiter);
                    //string fan = split[2];
                    //double dblFan = Convert.ToDouble(fan) * primEnElec;
                    split = lines[28].Split(delimiter);
                    string heat_north = split[1];
                    split = lines[29].Split(delimiter);
                    string heat_west = split[1];
                    split = lines[30].Split(delimiter);
                    string heat_east = split[1];
                    split = lines[31].Split(delimiter);
                    string heat_south = split[1];
                    split = lines[32].Split(delimiter);
                    string heat_interior = split[1];
                    split = lines[33].Split(delimiter);
                    string heat_main = split[1];
                    double dblHeat = (Convert.ToDouble(heat_north) + Convert.ToDouble(heat_west)
                        + Convert.ToDouble(heat_east) + Convert.ToDouble(heat_south)
                        + Convert.ToDouble(heat_interior) + Convert.ToDouble(heat_main))
                        * primEnGas / 3600000;

                    split = lines[36].Split(delimiter);
                    string cool = split[1];
                    double dblCool = Convert.ToDouble(cool) * primEnElec / 3600000;

                    split = lines[23].Split(delimiter);
                    string light_north = split[1];
                    split = lines[24].Split(delimiter);
                    string light_west = split[1];
                    split = lines[25].Split(delimiter);
                    string light_east = split[1];
                    split = lines[26].Split(delimiter);
                    string light_south = split[1];
                    split = lines[27].Split(delimiter);
                    string light_interior = split[1];
                    double dblLight = (Convert.ToDouble(light_north) * 5.0 + Convert.ToDouble(light_west)
                        + Convert.ToDouble(light_east) + Convert.ToDouble(light_south) * 5.0 + Convert.ToDouble(light_interior))
                        * primEnElec / 3600000;     //zone north and south need to be multiplied with 5 (5 rooms). This is not considered in the .eso

                    split = lines[34].Split(delimiter);
                    string fan_supply = split[1];
                    split = lines[34].Split(delimiter);
                    string fan_return = split[1];
                    double dblFan = (Convert.ToDouble(fan_supply) + Convert.ToDouble(fan_return)) * primEnElec / 3600000;

                    result = (dblHeat + dblCool + dblLight + dblFan) / 1104;   //1104 is the square meter               

                    //DA.SetData(0, result);
                    //save result (kWh/m2a) in an array
                    sobResults[j] = result;
                    sobResultsStr[j] = result.ToString();

                    System.Threading.Thread.Sleep(1500);
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
                    System.Threading.Thread.Sleep(1500);
                }


                //save sobResults as a text file
                File.WriteAllLines(path_out + "SobolResults.csv", sobResultsStr);





            }

        }






        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return GHEnergyPlus.Properties.Resources.sobol_4to8;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{986bf7e7-2f19-4c0e-af5e-a992da891839}"); }
        }
    }
}
