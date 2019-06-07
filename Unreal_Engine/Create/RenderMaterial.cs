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
using BH.oM.VirtualReality;
using BH.oM.Geometry;
using BH.oM.Graphics.MaterialFragments;

namespace BH.Engine.Graphics
{
    public static partial class Create
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public static RenderMaterial RenderMaterial(Color baseColor = null, double opacity = 1, double glossiness = 0, Color emissiveColor = null, double emissivity = 0)
        {
            baseColor = (baseColor == null) ? new Color() : baseColor;
            emissiveColor = (emissiveColor == null) ? new Color() : emissiveColor;


            return new RenderMaterial
            {
                BaseColor = baseColor,
                Opacity = opacity,
                Glossiness = glossiness,
                EmissiveColor = emissiveColor,
                Emissivity = emissivity
            };
        }

        /***************************************************/

        public static Color Color(double r = 1, double g = 1, double b = 1)
        {
            return new Color
            {
                R = r,
                G = g,
                B = b
            };
        }

        /***************************************************/
    }
}
