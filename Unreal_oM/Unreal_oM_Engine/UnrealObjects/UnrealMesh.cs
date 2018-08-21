using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;
using BH.oM.Geometry;

namespace BH.oM.VirtualReality
{
    public class UnrealMesh : BHoMObject
    {
        /***************************************************/
        /**** Properties                                ****/
        /***************************************************/

        public string Color { get; set; } = "";

        public Mesh Mesh { get; set; } = new Mesh();
    
        /***************************************************/


    }
}