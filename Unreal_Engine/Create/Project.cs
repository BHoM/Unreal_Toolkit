using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.VirtualReality;
using BH.oM.CFD.Elements;
using BH.oM.Geometry;

namespace BH.Engine.VirtualReality.Unreal
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Project Project( string name, int saveindex, int scale, string unit = "", List<Streamer> streamers = null, List<UnrealGeometry> geometry = null)
        {
            return new Project
            {
                Geometry = geometry,
                Streamers = streamers,
                saveIndex = saveindex.ToString(),
                Name = name,
                Scale = scale.ToString(),
                Unit = unit,
            };
        }

        /***************************************************/
    }
}
