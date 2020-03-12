/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2019, the respective contributors. All rights reserved.
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
using BH.oM.Base;
using BH.oM.VirtualReality;

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
            AdapterIdName = "Unreal_id";

            m_AdapterSettings.HandleDependencies = false;
            m_AdapterSettings.UseAdapterId = false;         //Tag objects with a software specific id in the CustomData. Requires the NextIndex method to be overridden and implemented
            m_AdapterSettings.DefaultPushType = oM.Adapter.PushType.CreateOnly;
              
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
