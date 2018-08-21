using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.VirtualReality;
using BH.oM.Acoustic;

namespace BH.Engine.VirtualReality.Unreal
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static Project Project(  string name, 
                                        int saveindex, 
                                        int scale, 
                                        string unit = "", 
                                        bool acousticMode = false,
                                        double resultMin = 0,
                                        double resultMax = 0,
                                        List<UnrealSpline> splines = null, 
                                        List<UnrealMesh> meshes = null, 
                                        List<UnrealNode> nodes = null)

        {
            return new Project
            {
                Meshes = meshes,
                Splines = splines,
                SaveIndex = saveindex.ToString(),
                Name = name,
                Scale = scale.ToString(),
                Unit = unit,
                Nodes = nodes,
                AcousticMode = acousticMode,
                ResultMax = resultMax,
                ResultMin = resultMin,

            };
        }

        /***************************************************/
    }
}
