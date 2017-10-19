using System;
using System.Collections.Generic;
using System.Threading;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunSimpleSobol : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHEPlusRunSimpleSobol class.
        /// </summary>
        public GHEPlusRunSimpleSobol()
            : base("WetterSimpleSobol", "WetterSimpleSobol",
                "Run Sobol Sequence on WetterSimple, save results as text file",
                "EnergyHubs", "BuildingSimulation")
        {
        }


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("idf file", "idf file", "idf file. has to be in C:\\eplus\\SimAud17\\Input\\", GH_ParamAccess.item);
            pManager.AddTextParameter("weather file", "weather file", "weather file. has to be in WeatherData folder of your Energyplus folder", GH_ParamAccess.item);
            pManager.AddTextParameter("Path LPt sequence", "Path LPt sequence",
                "Path to Sobol sequence csv file. That should be a matrix with samples per row and parameters in columns. Create sequence e.g. in matlab.",
                GH_ParamAccess.item);
            pManager.AddBooleanParameter("run", "run", "run EnergyPlus", GH_ParamAccess.item);

            pManager.AddIntegerParameter("folder", "folder", "folder number, like 1,2,3, for parallel runs", GH_ParamAccess.item);
            pManager[4].Optional = true;
        }



        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
          
        }



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
                    //string idfmodified = idffile + "_" + now;
                    string idfmodified = idffile + "_modi";

                    //load idf into a huge string
                    lines = new string[] { };
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
                    string[] replacethis = new string[4];
                    replacethis[0] = @"%azimuth%";
                    replacethis[1] = @"%w_we_win%";
                    replacethis[2] = @"%w_ea_win%";
                    replacethis[3] = @"%tau%";


                    //replacers
                    string[] replacers = new string[4];
                    double alpha = sobparameterset[j][0] * 360 - 180;   // orientation  [-180, 180]
                    double ww = sobparameterset[j][1] * 5.8 + 0.1;      // window west  [0.1, 5.9]
                    double we = sobparameterset[j][2] * 5.8 + 0.1;      // window east  [0.1, 5.9]
                    double tau = sobparameterset[j][3] * 0.6 + 0.2;     //transmittance [0.2, 0.8]
                    replacers[0] = alpha.ToString();
                    replacers[1] = ww.ToString();
                    replacers[2] = we.ToString();
                    replacers[3] = tau.ToString();


                  


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
                    System.Diagnostics.Process P = System.Diagnostics.Process.Start(eplusexe, command);
                    //System.Diagnostics.Process P = System.Diagnostics.Process.Start(eplusbat, idfmodified + " " + weatherfile);
                    P.WaitForExit();


                    




                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    while (!File.Exists(path_out + "eplusout.eso"))
                    {
                        Console.WriteLine("waiting");
                    }
                    System.Threading.Thread.Sleep(1500);


                    //output result (kWh/m2a) 
                    double result = double.NaN;
                    //identify correct result file. load it. get the right numbers from it
                    lines = new string[] { };
                    list = new List<string>();
                    fileStream = new FileStream(path_out + "eplusout.eso", FileMode.Open, FileAccess.Read);
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
                    double EffHeat = 0.44;
                    double EffCool = 0.77;

                    string[] split;
                    //split = System.Text.RegularExpressions.Regex.Split(lines[49], "\r\n");
                    char delimiter = ',';
                    split = lines[12].Split(delimiter);
                    string light = split[1];
                    double dblLight = Convert.ToDouble(light) / 3600000 / 96 * primEnElec;
                    split = lines[13].Split(delimiter);
                    string heat = split[1];
                    double dblHeat = Convert.ToDouble(heat) / 3600000 / 96 / EffHeat;
                    split = lines[14].Split(delimiter);
                    string cool = split[1];
                    double dblCool = Convert.ToDouble(cool) / 3600000 / 96 / EffCool;



                    result = (dblHeat + dblCool + dblLight);


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
                    //System.Threading.Thread.Sleep(1500);
                }




                //save sobResults as a text file
                File.WriteAllLines(path_out + "SobolResults.csv", sobResultsStr);
                


            }


        }




        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }


        public override Guid ComponentGuid
        {
            get { return new Guid("{ee34ba2a-bdf0-4e65-b579-9b52c7cf2071}"); }
        }
    }
}