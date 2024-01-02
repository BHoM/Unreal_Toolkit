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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BH.Adapter;
using BH.Engine.Unreal;

namespace BH.Adapter.Unreal
{
    public partial class UnrealAdapter : BHoMAdapter
    {

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        //Add any applicable constructors here, such as linking to a specific file or anything else as well as linking to that file through the (if existing) com link via the API
        public UnrealAdapter(string address = "127.0.0.1", int port = 8888)
        {
            AdapterId = BH.Engine.Unreal.Convert.AdapterId;   //Set the "AdapterId" to "SoftwareName_id". Generally stored as a constant string in the convert class in the SoftwareName_Engine

            Config.SeparateProperties = true;   //Set to true to push dependant properties of objects before the main objects are being pushed. Example: push nodes before pushing bars
            Config.MergeWithComparer = false;    //Set to true to use EqualityComparers to merge objects. Example: merge nodes in the same location
            Config.ProcessInMemory = true;     //Set to false to to update objects in the toolkit during the push
            Config.CloneBeforePush = false;      //Set to true to clone the objects before they are being pushed through the software. Required if any modifications at all, as adding a software ID is done to the objects
            Config.UseAdapterId = false;         //Tag objects with a software specific id in the CustomData. Requires the NextIndex method to be overridden and implemented

            m_address = address;
            m_port = port;

        }




        /***************************************************/
        /**** Private  Fields                           ****/
        /***************************************************/

        //Add any comlink object as a private field here, example named:

        //private SoftwareComLink m_softwareNameCom;
        private string m_address = "127.0.0.1";
        private int m_port = 8888;

        /***************************************************/


    }
}





