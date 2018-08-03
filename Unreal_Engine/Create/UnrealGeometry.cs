using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.VirtualReality;
using BH.oM.Geometry;

namespace BH.Engine.VirtualReality
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static UnrealGeometry UnrealGeometry(Mesh Mesh, string Color)
        {
            return new UnrealGeometry
            {
                mesh = Mesh,
                color = Color,
            };
        }

        /***************************************************/
    }
}
