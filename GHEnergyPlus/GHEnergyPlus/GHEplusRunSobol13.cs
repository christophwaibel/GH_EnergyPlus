// Decompiled with JetBrains decompiler
// Type: GHEnergyPlus.GHEplusRunSobol13
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
  public class GHEplusRunSobol13 : GH_Component
  {
    protected virtual Bitmap Icon
    {
      get
      {
        return Resources.sobol_13;
      }
    }

    public virtual Guid ComponentGuid
    {
      get
      {
        return new Guid("{3573cde7-99dc-48a1-b460-850d52099851}");
      }
    }

    public GHEplusRunSobol13()
    {
      this.\u002Ector("Sobol13Djuric", "Sobol13", "Sobol 13 Djuric (now Nord) 2007, school building.", "EnergyHubs", "BuildingSimulation");
    }

    protected virtual void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    {
      pManager.AddTextParameter("idf file", "idf file", "idf file. has to be in C:\\eplus\\EPOpti17\\Input\\", (GH_ParamAccess) 0);
      pManager.AddTextParameter("weather file", "weather file", "weather file. has to be in WeatherData folder of your Energyplus folder", (GH_ParamAccess) 0);
      pManager.AddTextParameter("Path LPt sequence", "Path LPt sequence", "Path to Sobol sequence csv file. That should be a matrix with samples per row and parameters in columns. Create sequence e.g. in matlab.", (GH_ParamAccess) 0);
      pManager.AddBooleanParameter("run", "run", "run EnergyPlus", (GH_ParamAccess) 0);
      pManager.AddIntegerParameter("folder", "folder", "folder number, like 1,2,3, for parallel runs", (GH_ParamAccess) 0);
      pManager.get_Param(4).set_Optional(true);
    }

    protected virtual void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    {
    }

    protected virtual void SolveInstance(IGH_DataAccess DA)
    {
      int num1 = 0;
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<int>(4, (M0&) @num1))
        num1 = 0;
      string str1 = "c:\\eplus\\EPOpti17\\Input" + (object) num1 + "\\";
      string path1 = "c:\\eplus\\EPOpti17\\Output" + (object) num1 + "\\";
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
      string path2 = "blabla";
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<string>(2, (M0&) @path2))
        return;
      bool flag = false;
      // ISSUE: explicit reference operation
      // ISSUE: cast to a reference type
      if (!DA.GetData<bool>(3, (M0&) @flag))
        return;
      int length = 10;
      if (!flag)
        return;
      List<double[]> numArrayList = new List<double[]>();
      List<string> stringList1 = new List<string>();
      FileStream fileStream1 = new FileStream(path2, FileMode.Open, FileAccess.Read);
      using (StreamReader streamReader = new StreamReader((Stream) fileStream1))
      {
        string str4;
        while ((str4 = streamReader.ReadLine()) != null)
          stringList1.Add(str4);
      }
      string[] array1 = stringList1.ToArray();
      fileStream1.Close();
      for (int index1 = 0; index1 < array1.Length; ++index1)
      {
        double[] numArray = new double[length];
        char ch = ';';
        string[] strArray = array1[index1].Split(ch);
        for (int index2 = 0; index2 < strArray.Length; ++index2)
          numArray[index2] = Convert.ToDouble(strArray[index2]);
        numArrayList.Add(numArray);
      }
      int count = numArrayList.Count;
      double[] numArray1 = new double[count];
      string[] contents = new string[count];
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
      double[] numArray2 = new double[length];
      double[] numArray3 = new double[length];
      numArray2[0] = 0.05;
      numArray3[0] = 0.3;
      numArray2[1] = 400.0;
      numArray3[1] = 800.0;
      numArray2[2] = 60.0;
      numArray3[2] = 120.0;
      numArray2[3] = 50.0;
      numArray3[3] = 110.0;
      numArray2[4] = 80.0;
      numArray3[4] = 160.0;
      numArray2[5] = 20.0;
      numArray3[5] = 50.0;
      numArray2[6] = 80.0;
      numArray3[6] = 180.0;
      numArray2[7] = 140.0;
      numArray3[7] = 280.0;
      numArray2[8] = 380.0;
      numArray3[8] = 760.0;
      numArray2[9] = 350.0;
      numArray3[9] = 700.0;
      for (int index1 = 0; index1 < numArrayList.Count; ++index1)
      {
        string str4 = str2 + "_modi_" + (object) index1;
        string[] strArray2 = new string[0];
        List<string> stringList2 = new List<string>();
        FileStream fileStream2 = new FileStream(str1 + str2 + ".idf", FileMode.Open, FileAccess.Read);
        using (StreamReader streamReader = new StreamReader((Stream) fileStream2))
        {
          string str5;
          while ((str5 = streamReader.ReadLine()) != null)
            stringList2.Add(str5);
        }
        string[] array2 = stringList2.ToArray();
        fileStream2.Close();
        double[] numArray4 = new double[strArray1.Length];
        for (int index2 = 0; index2 < length; ++index2)
          numArray4[index2] = numArrayList[index1][index2] * (numArray3[index2] - numArray2[index2]) + numArray2[index2];
        string[] strArray3 = new string[strArray1.Length];
        for (int index2 = 0; index2 < length; ++index2)
          strArray3[index2] = numArray4[index2].ToString();
        for (int index2 = 0; index2 < array2.Length; ++index2)
        {
          for (int index3 = 0; index3 < strArray1.Length; ++index3)
            array2[index2] = array2[index2].Replace(strArray1[index3], strArray3[index3]);
        }
        File.WriteAllLines(str1 + str4 + ".idf", array2);
        string str6 = str1 + str4 + ".idf";
        string command = " -w " + (str1 + "ep\\WeatherData\\" + str3 + ".epw") + " -x -d " + path1 + " -i " + str1 + "ep\\Energy+.idd " + str6;
        string directory1 = path1;
        Misc.RunEplus(FileName, command, directory1);
        string str7 = "eplusout.eso";
        while (!File.Exists(path1 + str7))
          Console.WriteLine("waiting");
        strArray2 = new string[0];
        List<string> stringList3 = new List<string>();
        FileStream fileStream3 = new FileStream(path1 + str7, FileMode.Open, FileAccess.Read);
        using (StreamReader streamReader = new StreamReader((Stream) fileStream3))
        {
          string str5;
          while ((str5 = streamReader.ReadLine()) != null)
            stringList3.Add(str5);
        }
        string[] array3 = stringList3.ToArray();
        fileStream3.Close();
        char ch = ',';
        double num2 = Convert.ToDouble(array3[55].Split(ch)[1]) / 3600000.0 * (2520.0 * 0.6 * 0.75 / (19.0 - -11.5)) * 0.034 + Convert.ToDouble(array3[56].Split(ch)[1]) / 1000.0 * 12.42;
        double maxValue = (double) byte.MaxValue;
        double num3 = 0.19 * (numArray4[0] * 100.0) - 0.15;
        double num4 = 10.0;
        double num5 = 1.4 * (maxValue * num3 / num4);
        double num6 = 10.5;
        double num7 = 0.3;
        double num8 = 8.0;
        double num9 = 0.0;
        for (int index2 = 0; index2 < 9; ++index2)
          num9 += numArray4[index2 + 1];
        double num10 = num6 / num7 * (num9 / num8) * (1.0 / num4);
        double num11 = num2 + num10 + num5;
        numArray1[index1] = num11;
        contents[index1] = num11.ToString();
        Thread.Sleep(1500);
        File.Delete(str1 + str4 + ".idf");
        DirectoryInfo directoryInfo = new DirectoryInfo(path1);
        foreach (FileSystemInfo file in directoryInfo.GetFiles())
          file.Delete();
        foreach (DirectoryInfo directory2 in directoryInfo.GetDirectories())
          directory2.Delete(true);
      }
      File.WriteAllLines(path1 + "SobolResults.csv", contents);
    }
  }
}
