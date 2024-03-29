/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.oM.Base;

namespace BH.Adapter.Unreal
{
    public partial class UnrealAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/
        protected override IEnumerable<IBHoMObject> Read(Type type, IList ids)
        {
            //Choose what to pull out depending on the type. Also see example methods below for pulling out bars and dependencies
            //if (type == typeof(Node))
            //    return ReadNodes(ids as dynamic);
            //else if (type == typeof(Bar))
            //    return ReadBars(ids as dynamic);
            //else if (type == typeof(ISectionProperty) || type.GetInterfaces().Contains(typeof(ISectionProperty)))
            //    return ReadSectionProperties(ids as dynamic);
            //else if (type == typeof(Material))
            //    return ReadMaterials(ids as dynamic);

            return new List<IBHoMObject>();
        }

        /***************************************************/
        /**** Private specific read methods             ****/
        /***************************************************/

        //The List<string> in the methods below can be changed to a list of any type of identification more suitable for the toolkit

        //private List<Bar> ReadBars(List<string> ids = null)
        //{
        //    //Implement code for reading bars
        //    throw new NotImplementedException();
        //}

        ///***************************************/

        //private List<Node> ReadNodes(List<string> ids = null)
        //{
        //    //Implement code for reading nodes
        //    throw new NotImplementedException();
        //}

        ///***************************************/

        //private List<ISectionProperty> ReadSectionProperties(List<string> ids = null)
        //{
        //    //Implement code for reading section properties
        //    throw new NotImplementedException();
        //}

        ///***************************************/

        //private List<Material> ReadMaterials(List<string> ids = null)
        //{
        //    //Implement code for reading materials
        //    throw new NotImplementedException();
        //}

        /***************************************************/


    }
}





