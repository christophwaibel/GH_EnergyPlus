using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunDetailedSobol : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GHEPlusRunDetailedSobol()
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
            pManager.AddTextParameter("idf file", "idf file", "idf file. has to be in C:\\eplus\\SimAud17\\Input\\", GH_ParamAccess.item);
            pManager.AddTextParameter("weather file", "weather file", "weather file. has to be in WeatherData folder of your Energyplus folder", GH_ParamAccess.item);
            pManager.AddTextParameter("Path LPt sequence", "Path LPt sequence",
                "Path to Sobol sequence csv file. That should be a matrix with samples per row and parameters in columns. Create sequence e.g. in matlab.",
                GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "run EnergyPlus", GH_ParamAccess.item);
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
            string path_in = @"c:\eplus\EPOpti17\Input\";
            string path_out = @"c:\eplus\EPOpti17\Output\";
            string eplusbat = @"C:\EnergyPlusV8-5-0\RunEPlusEPOpti17.bat";


            string idffile = @"detailed_template";
            if (!DA.GetData(0, ref idffile)) { return; }

            string weatherfile = @"C:\EnergyPlusV8-5-0\RunEPlusSimAud17.bat";
            if (!DA.GetData(1, ref weatherfile)) { return; }

            string sobolpath = @"C:\eplus\SimAud17\Input\sobol.csv";
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
                    string now = j.ToString();
                    string idfmodified = idffile + "_" + now;

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
                    replacers[7] = (0.6).ToString();  
                    replacers[8] = (12.0 - (ww / 2.0)-0.5).ToString();
                    replacers[9] = (12.0 + (ww / 2.0) + 0.5).ToString();
                    replacers[10] = ow.ToString();

                    double we = sobparameterset[j][3] * 18.324 + 7.344;
                    double oe = sobparameterset[j][4] + 8.05;
                    replacers[11] = (12.0 - (we / 2.0)).ToString();      //x0
                    replacers[12] = (12.0 + (we / 2.0)).ToString();      //x1
                    replacers[13] = (0.6).ToString();
                    replacers[14] = (12.0 - (we / 2.0) - 0.5).ToString();
                    replacers[15] = (12.0 + (we / 2.0) + 0.5).ToString();
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




                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //run eplus
                    var outt = System.Diagnostics.Process.Start(eplusbat, idfmodified + " " + weatherfile);









                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    while (!File.Exists(path_out + idfmodified + "Table.csv"))
                    {
                        Console.WriteLine("waiting");
                    }
                    System.Threading.Thread.Sleep(1500);


                    //output result (kWh/m2a) 
                    double result = double.NaN;
                    //identify correct result file. load it. get the right numbers from it
                    lines = new string[] { };
                    list = new List<string>();
                    fileStream = new FileStream(path_out + idfmodified + "Table.csv", FileMode.Open, FileAccess.ReadWrite);
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
                    //split = System.Text.RegularExpressions.Regex.Split(lines[49], "\r\n");
                    char delimiter = ',';
                    split = lines[49].Split(delimiter);
                    string heat = split[3];
                    double dblHeat = Convert.ToDouble(heat) * primEnGas;

                    split = lines[50].Split(delimiter);
                    string cool = split[2];
                    double dblCool = Convert.ToDouble(cool) * primEnElec;

                    split = lines[51].Split(delimiter);
                    string light = split[2];
                    double dblLight = Convert.ToDouble(light) * primEnElec;

                    split = lines[55].Split(delimiter);
                    string fan = split[2];
                    double dblFan = Convert.ToDouble(fan) * primEnElec;


                    result = (dblHeat + dblCool + dblLight + dblFan) / 1104;                  

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
                return null;
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
