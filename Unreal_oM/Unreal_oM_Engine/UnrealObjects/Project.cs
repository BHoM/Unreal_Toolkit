using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.CFD.Elements;

namespace BH.oM.VirtualReality
{
    public class Project : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public string saveIndex { get; set; } = "";

        public List<Streamer> Streamers { get; set; } = new List<Streamer>();

        public List<UnrealGeometry> Geometry { get; set; } = new List<UnrealGeometry>();

        public string Scale { get; set; } = "";

        public string Unit { get; set; } = "";

        /***************************************************/
    }
}