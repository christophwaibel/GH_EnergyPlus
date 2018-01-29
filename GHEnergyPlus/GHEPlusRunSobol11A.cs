using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunSobol11A : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHEPlusRunSobol11A2 class.
        /// </summary>
        public GHEPlusRunSobol11A()
            : base("Sobol11NguyenA_NV", "Sobol11A_NV",
                "Sobol 11 A Nat.Vent., adaptive comfort with Natural Ventilation (objective function I, eqt. 4, 2nd line), Nguyen & Reiter 2014, ",
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

            int dvar = 18;


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
                string[] replacethis = new string[dvar + 3];
                replacethis[0] = @"%azimuth%";
                replacethis[1] = @"%width%";
                replacethis[2] = @"%osize1%";
                replacethis[3] = @"%osize2%";
                replacethis[4] = @"%osize3%";
                replacethis[5] = @"%osize4%";
                replacethis[6] = @"%wwidth1%";
                replacethis[7] = @"%wwidth2%";
                replacethis[8] = @"%wwidth3%";
                replacethis[9] = @"%wwidth4%";
                replacethis[10] = @"%absor%";
                replacethis[11] = @"%infil%";
                replacethis[12] = @"%tmass%";
                replacethis[13] = @"%floortype%";
                replacethis[14] = @"%ventilation%";
                replacethis[15] = @"%rooftype%";
                replacethis[16] = @"%wintype%";
                replacethis[17] = @"%exwall%";
                replacethis[18] = @"%ratio%";
                replacethis[19] = @"%windazimuth%";
                replacethis[20] = @"%length%";

                double[] mins = new double[dvar];
                double[] maxs = new double[dvar];
                mins[0] = -90;
                maxs[0] = 90;
                mins[1] = 4;
                maxs[1] = 10;
                mins[2] = 0.2;
                maxs[2] = 0.8;
                mins[3] = 0.2;
                maxs[3] = 0.8;
                mins[4] = 0.2;
                maxs[4] = 0.8;
                mins[5] = 0.2;
                maxs[5] = 0.8;
                mins[6] = 5;
                maxs[6] = 8;
                mins[7] = 5;
                maxs[7] = 8;
                mins[8] = 0.5;
                maxs[8] = 2.5;
                mins[9] = 0.5;
                maxs[9] = 2.5;
                mins[10] = 0.3;
                maxs[10] = 0.9;
                mins[11] = 0.002;
                maxs[11] = 0.006;
                mins[12] = 600;
                maxs[12] = 602;
                mins[13] = 500;
                maxs[13] = 502;
                mins[14] = 404;
                maxs[14] = 409;
                mins[15] = 300;
                maxs[15] = 302;
                mins[16] = 200;
                maxs[16] = 202;
                mins[17] = 100;
                maxs[17] = 103;

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







                    //replacers
                    string[] replacers = new string[dvar + 3];
                    for (int i = 0; i < dvar - 6; i++)
                        replacers[i] = (sobparameterset[j][i] * (maxs[i] - mins[i]) + mins[i]).ToString();        //translate from normalized sobol samples to domain
                    for (int i = dvar - 6; i < dvar; i++)
                        replacers[i] = (sobparameterset[j][i] + mins[i]).ToString();

                    double xlength = 100 / (sobparameterset[j][1] * (maxs[1] - mins[1]) + mins[1]);               //translate
                    double xratio = (sobparameterset[j][1] * (maxs[1] - mins[1]) + mins[1]) / xlength;            //translate
                    double xwindazimuth = (sobparameterset[j][0] * (maxs[0] - mins[0]) + mins[0] )+ 90;           //translate

                    replacers[18] = xratio.ToString();
                    replacers[19] = xwindazimuth.ToString();
                    replacers[20] = xlength.ToString();





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
                    double ConstCost;
                    double TotHourDisCom; //heating set point not met (HSPNM) + cooling set point not met (CSPNM)

                    string[] split;
                    char delimiter = ',';

                    split = lines[20].Split(delimiter);
                    string strConstCost = split[3];

                    split = lines[79].Split(delimiter);
                    string strHSPNM = split[2];
                    string strCSPNM = split[5];

                    ConstCost = Convert.ToDouble(strConstCost);
                    TotHourDisCom = Convert.ToDouble(strHSPNM) + Convert.ToDouble(strCSPNM);
                    Fx = ConstCost * (TotHourDisCom / 8760);



                    result = Fx;




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
                return GHEnergyPlus.Properties.Resources.sobol_11A2;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{33a1de8f-5a9c-43da-a7cb-b57ed07a50a3}"); }
        }
    }
}