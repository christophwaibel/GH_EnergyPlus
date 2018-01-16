using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;

namespace GHEnergyPlus
{
    public class GHEPlusRunSobol14 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHEPlusRunSobol14 class.
        /// </summary>
        public GHEPlusRunSobol14()
            : base("Sobol14Gonzalez", "Sobol14",
                "Sobol 14 Gonzalez & Coley 2014, office room.",
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

            int dvar = 20;


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
                string[] replacethis = new string[dvar + 71];

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


                double[] mins = new double[dvar];
                double[] maxs = new double[dvar];
                mins[0] = 0.021;
                maxs[0] = 0.6;
                mins[1] = 0.3;
                maxs[1] = 3.0;
                mins[2] = 1.70;
                maxs[2] = 5.20;
                mins[3] = 0.12;
                maxs[3] = 0.8;
                mins[4] = 0.12;
                maxs[4] = 0.8;
                mins[5] = 0.12;
                maxs[5] = 0.8;
                mins[6] = 0.12;
                maxs[6] = 0.8;
                mins[7] = 1;
                maxs[7] = 4;
                mins[8] = 100;
                maxs[8] = 500;
                mins[9] = 0.2;
                maxs[9] = 2.4;
                mins[10] = 200;
                maxs[10] = 3000;
                mins[11] = 0.01;
                maxs[11] = 2.0;
                mins[12] = 0.01;
                maxs[12] = 5.0;
                mins[13] = 0.01;
                maxs[13] = 5.0;
                mins[14] = 0.01;
                maxs[14] = 2.0;
                mins[15] = 0.01;
                maxs[15] = 5.0;
                mins[16] = 0.01;
                maxs[16] = 5.0; 
                mins[17] = 0.01;
                maxs[17] = 2.0;
                mins[18] = 0.01;
                maxs[18] = 5.0;
                mins[19] = 0.01;
                maxs[19] = 5.0;

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

                    split = lines[11].Split(delimiter);
                    string heat = split[1];
                    split = lines[12].Split(delimiter);
                    string cool = split[1];
                    double dblheat = 0.001 * Convert.ToDouble(heat) / 3600;
                    double dblcool = 0.001 * Convert.ToDouble(cool) / 3600 * 3;

                    Fx = dblheat + dblcool;


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
                return GHEnergyPlus.Properties.Resources.sobol_14;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{e3ad1a08-5c5b-4aa0-9ebb-03396f31941c}"); }
        }
    }
}