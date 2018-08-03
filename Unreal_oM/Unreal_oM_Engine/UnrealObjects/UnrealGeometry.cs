﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;

namespace BH.oM.VirtualReality
{
    public class UnrealGeometry : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public string color { get; set; } = "";

        public Mesh mesh { get; set; } = new Mesh();
    
        /***************************************************/


    }
}