using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Adapter.Unreal
{
    public partial class UnrealAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override int Delete(Type type, IEnumerable<object> ids)
        {
            //Insert code here to enable deletion of specific types of objects with specific ids
            throw new NotImplementedException();
        }

        /***************************************************/
    }
}
