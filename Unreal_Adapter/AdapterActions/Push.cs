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
using BH.oM.Adapter;
using BH.oM.Base;
using BH.oM.VirtualReality;

namespace BH.Adapter.Unreal
{
	public partial class UnrealAdapter : BHoMAdapter
	{
		public override List<object> Push(IEnumerable<object> objects, string tag = "", PushType pushType = PushType.AdapterDefault, ActionConfig actionConfig = null)
		{
			IEnumerable<IBHoMObject> objectsToPush = objects.OfType<IBHoMObject>();

			if (pushType != PushType.CreateOnly)
			{

			}


			if (objectsToPush.Count() != objects.Count())
				BH.Engine.Reflection.Compute.RecordWarning($"Only BHoMObjects can be pushed with {this.GetType().Name}");

			return Create(objectsToPush) ? objects.ToList() : new List<object>();
		}
	}
}
