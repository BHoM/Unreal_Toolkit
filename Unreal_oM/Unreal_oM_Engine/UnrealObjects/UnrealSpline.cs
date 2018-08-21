using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;

namespace BH.oM.VirtualReality
{
    public class UnrealSpline : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public List<UnrealNode> Nodes { get; set; } = new List<UnrealNode>();
    
        /***************************************************/


    }
}