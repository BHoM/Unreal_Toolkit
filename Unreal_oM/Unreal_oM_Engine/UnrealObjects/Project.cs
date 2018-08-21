using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.Acoustic;

namespace BH.oM.VirtualReality
{
    public class Project : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public string SaveIndex { get; set; } = "";

        public List<UnrealSpline> Splines { get; set; } = new List<UnrealSpline>();

        public List<UnrealMesh> Meshes { get; set; } = new List<UnrealMesh>();

        public List<UnrealNode> Nodes { get; set; } = new List<UnrealNode>();

        public string Scale { get; set; } = "";

        public string Unit { get; set; } = "";

        public bool AcousticMode { get; set; } = false;

        public double ResultMax { get; set; } = 0;

        public double ResultMin { get; set; } = 0;

        /***************************************************/
    }
}