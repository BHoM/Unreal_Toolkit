using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Engine.Unreal;
using BH.oM.VirtualReality;

namespace BH.Adapter.Unreal
{
    public partial class UnrealAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool active = false)
        {
            if (active)
            {
                bool success = true;        //boolean returning if the creation was successfull or not
                List < Project > projects = objects.Cast<Project>().ToList();
                Project project = projects[0];
                success = BH.Engine.Unreal.Convert.ToUnreal(m_address, m_port, project);

                //UpdateViews()             //If there exists a command for updating the views is the software call it now:

                return success;             //Finally return if the creation was successful or not
            }
            return false;
        }

        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/


        private bool CreateCollection(IEnumerable<object> objects)
        {
            //Code for creating a collection of nodes in the software

            throw new NotImplementedException();
        }


        /***************************************************/
    }
}
