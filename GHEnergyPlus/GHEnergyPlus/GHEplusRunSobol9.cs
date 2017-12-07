using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEplusRunSobol9 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHEplusRunSobol9 class.
        /// </summary>
        public GHEplusRunSobol9()
            : base("Sobol9Kämpf", "Sobol9",
                "Sobol 09 small office building, Kämpf, Wetter & Robinson 2010, ",
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
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
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

            int dvar = 13;


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
                    double[] numbers = new double[dvar];
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


                //to be replaced
                string[] replacethis = new string[dvar + 1];
                for (int i = 0; i < replacethis.Length; i++)
                    replacethis[i] = @"%" + i.ToString() + @"%";


                double[] mins = new double[dvar];
                double[] maxs = new double[dvar];
                mins[0] = 0.8;
                maxs[0] = 1.25;
                mins[1] = 1.35;
                maxs[1] = 2.2;
                mins[2] = 0.8;
                maxs[2] = 1.25;
                mins[3] = 1.35;
                maxs[3] = 2.2;
                mins[4] = 0.8;
                maxs[4] = 1.25;
                mins[5] = 1.35;
                maxs[5] = 2.2;
                mins[6] = 0.8;
                maxs[6] = 1.25;
                mins[7] = 1.35;
                maxs[7] = 2.2;
                mins[8] = 12;
                maxs[8] = 18;
                mins[9] = 13;
                maxs[9] = 21;
                mins[10] = 13;
                maxs[10] = 21;
                mins[11] = 24;
                maxs[11] = 36;
                mins[12] = 24;
                maxs[12] = 36;


                //for each sample
                for (int j = 0; j < sobparameterset.Count; j++)
                {
                    //***********************************************************************************
                    //***********************************************************************************
                    //***********************************************************************************
                    //modify idf file with parameters and save as new idf file
                    string idfmodified = idffile + "_modi_" + j;

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



                    double[] x = new double[replacethis.Length];
                    for (int i = 0; i < dvar; i++)
                        x[i] = (sobparameterset[j][i] * (maxs[i] - mins[i]) + mins[i]);
                    x[13] = x[8] * 0.0006875 - 0.00025; 

                    //replacers
                    string[] replacers = new string[replacethis.Length];
                    for (int i = 0; i < replacethis.Length; i++)
                        replacers[i] = x[i].ToString();        //translate from normalized sobol samples to domain


                    //constraints
                    bool[] gx = new bool[4];        //true means constraint violation
                    if (x[0] - x[1] + 0.5488 >= 0) gx[0] = true;
                    if (x[2] - x[3] + 0.5488 >= 0) gx[1] = true;
                    if (x[4] - x[5] + 0.5488 >= 0) gx[2] = true;
                    if (x[6] - x[7] + 0.5488 >= 0) gx[3] = true;
                   



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
                    string outputfile = "eplustbl.csv";
                    while (!File.Exists(path_out + outputfile))
                    {
                        Console.WriteLine("waiting");
                    }


                    //output result 
                    double result = double.NaN;
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

                    split = lines[16].Split(delimiter);
                    string MJm2 = split[4];

                    Fx = (Convert.ToDouble(gx[0]) * 0.5 +
                        Convert.ToDouble(gx[1]) * 0.5 +
                        Convert.ToDouble(gx[2]) * 0.5 +
                        Convert.ToDouble(gx[3]) * 0.5 +
                        1) * Convert.ToDouble(MJm2);


                    result = Fx;




                    //DA.SetData(0, result);
                    //save result (MJ/m2) in an array
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
                }




                //save sobResults as a text file
                File.WriteAllLines(path_out + "SobolResults.csv", sobResultsStr);



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
                return GHEnergyPlus.Properties.Resources.sobol_9;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{1b8e6627-5e5b-4e95-81fb-f0cdc6babe10}"); }
        }
    }
}