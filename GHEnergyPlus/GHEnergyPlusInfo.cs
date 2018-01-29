﻿using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GHEnergyPlus
{
    public class GHEnergyPlusInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GHEnergyPlus";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("6ffbe401-a1b2-4730-99ce-1c4a3ea23ba5");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Empa";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
