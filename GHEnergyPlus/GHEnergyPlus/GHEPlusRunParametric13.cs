// Decompiled with JetBrains decompiler
// Type: GHEnergyPlus.GHEPlusRunParametric13
// Assembly: GHEnergyPlus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 809D60A7-5F24-4F14-BF88-2A9A0D82958A
// Assembly location: C:\Users\wach\Desktop\prob13\GHEnergyPlus.dll

using GHEnergyPlus.Properties;
using Grasshopper.Kernel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace GHEnergyPlus
{
  public class GHEPlusRunParametric13 : GH_Component
  {
    protected virtual Bitmap Icon
    {
      get
      {
        return Resources.opti_13;
      }
    }

    public virtual Guid ComponentGuid
    {
      get
      {
        return new Guid("{16f5c4ba-cf77-49cb-92e7-cb0dfb0317dd}");
      }
    }

    public GHEPlusRunParametric13()
    {
      this.\u002Ector("Prob13Djuric", "Prob13Djuric", "Problem 13 Djuric (now Nord) 2007, school building.", "EnergyHubs", "BuildingSimulation");
    }

    protected virtual void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddTextParameter("idf", "idf", "idf file name. has to be in C:\\eplus\\EPOpti17\\Input\\", (GH_ParamAccess) 0);
      pManager.AddTextParameter("weather", "weather", "weather file name. has to be in \\WeatherData of your Energyplus folder", (GH_ParamAccess) 0);
      pManager.AddBooleanParameter("run", "run", "Run the simulation", (GH_ParamAccess) 0);
      pManager.AddIntegerParameter("sleep", "sleep", "sleep. default is 1500", (GH_ParamAccess) 0);
      pManager.get_Param(3).set_Optional(true);
      pManager.AddIntegerParameter("folder", "folder", "folder number, like 1,2,3, for parallel runs", (GH_ParamAccess) 0);
      pManager.get_Param(4).set_Optional(true);
      pManager.AddNumberParameter("------------", "------------", "------------", (GH_ParamAccess) 0);
      pManager.get_Param(5).set_Optional(true);
      pManager.AddNumberParameter("delta", "x[0]", "Insulation thickness in [m] ∈ [0.05, 0.3].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA1", "x[1]", "Radiator1 in Class Zone2, U-factor times area value [W/K] ∈ [400, 800].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA2", "x[2]", "Radiator2 in Work Zone, U-factor times area value [W/K] ∈ [60, 120].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA3", "x[3]", "Radiator3 in Kitchen Zone, U-factor times area value [W/K] ∈ [50, 110].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA4", "x[4]", "Radiator4 in Music Zone, U-factor times area value [W/K] ∈ [80, 160].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA5", "x[5]", "Radiator5 in Service Zone, U-factor times area value [W/K] ∈ [20, 50].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA6", "x[6]", "Radiator6 in Washing Zone, U-factor times area value [W/K] ∈ [80, 180].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA7", "x[7]", "Radiator7 in Big Zone, U-factor times area value [W/K] ∈ [140, 280].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA8", "x[8]", "Radiator8 in Class Zone1, U-factor times area value [W/K] ∈ [380, 760].", (GH_ParamAccess) 0);
      pManager.AddNumberParameter("UA9", "x[9]", "Radiator9 in Office Zone, U-factor times area value [W/K] ∈ [350, 700].", (GH_ParamAccess) 0);
    }

    protected virtual void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
      pManager.AddNumberParameter("€", "€", "Total Cost for Energy, Radiators and Insulation in [€].", (GH_ParamAccess) 0);
    }

    protected virtual void SolveInstance(IGH_DataAccess DA)
    {
      int millisecondsTimeout = 100;
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<int>(3, (M0&) @millisecondsTimeout))
        millisecondsTimeout = 100;
      int num1 = 0;
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<int>(4, (M0&) @num1))
        num1 = 0;
      string str1 = "c:\\eplus\\EPOpti17\\Input" + (object) num1 + "\\";
      string path = "c:\\eplus\\EPOpti17\\Output" + (object) num1 + "\\";
      string FileName = "c:\\eplus\\EPOpti17\\Input" + (object) num1 + "\\ep\\energyplus.exe";
      string str2 = "blabla";
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<string>(0, (M0&) @str2))
        return;
      string str3 = "blabla";
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<string>(1, (M0&) @str3))
        return;
      bool flag = false;
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<bool>(2, (M0&) @flag))
        return;
      int length = 10;
      double[] numArray = new double[length];
      for (int index = 0; index < numArray.Length; ++index)
      {
        // ISSUE: explicit reference operation
        // ISSUE: cast to a reference type
        if (!DA.GetData<double>(index + 6, (M0&) @numArray[index]))
          return;
      }
      if (!flag)
        return;
      string str4 = str2 + "_modi";
      List<string> stringList1 = new List<string>();
      FileStream fileStream1 = new FileStream(str1 + str2 + ".idf", FileMode.Open, FileAccess.Read);
      using (StreamReader streamReader = new StreamReader((Stream) fileStream1))
      {
        string str5;
        while ((str5 = streamReader.ReadLine()) != null)
          stringList1.Add(str5);
      }
      string[] array1 = stringList1.ToArray();
      fileStream1.Close();
      string[] strArray1 = new string[length];
      strArray1[0] = "%delta%";
      strArray1[1] = "%UA1%";
      strArray1[2] = "%UA2%";
      strArray1[3] = "%UA3%";
      strArray1[4] = "%UA4%";
      strArray1[5] = "%UA5%";
      strArray1[6] = "%UA6%";
      strArray1[7] = "%UA7%";
      strArray1[8] = "%UA8%";
      strArray1[9] = "%UA9%";
      string[] strArray2 = new string[strArray1.Length];
      for (int index = 0; index < length; ++index)
        strArray2[index] = numArray[index].ToString();
      for (int index1 = 0; index1 < array1.Length; ++index1)
      {
        for (int index2 = 0; index2 < strArray1.Length; ++index2)
          array1[index1] = array1[index1].Replace(strArray1[index2], strArray2[index2]);
      }
      File.WriteAllLines(str1 + str4 + ".idf", array1);
      string str6 = str1 + str4 + ".idf";
      string command = " -w " + (str1 + "ep\\WeatherData\\" + str3 + ".epw") + " -x -d " + path + " -i " + str1 + "ep\\Energy+.idd " + str6;
      string directory1 = path;
      Misc.RunEplus(FileName, command, directory1);
      string str7 = "eplusout.eso";
      while (!File.Exists(path + str7))
        Console.WriteLine("waiting");
      string[] strArray3 = new string[0];
      List<string> stringList2 = new List<string>();
      FileStream fileStream2 = new FileStream(path + str7, FileMode.Open, FileAccess.Read);
      using (StreamReader streamReader = new StreamReader((Stream) fileStream2))
      {
        string str5;
        while ((str5 = streamReader.ReadLine()) != null)
          stringList2.Add(str5);
      }
      string[] array2 = stringList2.ToArray();
      fileStream2.Close();
      char ch = ',';
      double num2 = Convert.ToDouble(array2[55].Split(ch)[1]) / 3600000.0 * (2520.0 * 0.6 * 0.75 / (19.0 - -11.5)) * 0.034 + Convert.ToDouble(array2[56].Split(ch)[1]) / 1000.0 * 12.42;
      double maxValue = (double) byte.MaxValue;
      double num3 = 0.19 * (numArray[0] * 100.0) - 0.15;
      double num4 = 10.0;
      double num5 = 1.4 * (maxValue * num3 / num4);
      double num6 = 10.5;
      double num7 = 0.3;
      double num8 = 8.0;
      double num9 = 0.0;
      for (int index = 0; index < 9; ++index)
        num9 += numArray[index + 1];
      double num10 = num6 / num7 * (num9 / num8) * (1.0 / num4);
      double num11 = num2 + num10 + num5;
      Thread.Sleep(millisecondsTimeout);
      File.Delete(str1 + str4 + ".idf");
      DirectoryInfo directoryInfo = new DirectoryInfo(path);
      foreach (FileSystemInfo file in directoryInfo.GetFiles())
        file.Delete();
      foreach (DirectoryInfo directory2 in directoryInfo.GetDirectories())
        directory2.Delete(true);
      DA.SetData(0, (object) num11);
    }
  }
}
