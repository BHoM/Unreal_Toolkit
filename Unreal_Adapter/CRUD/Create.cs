/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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


