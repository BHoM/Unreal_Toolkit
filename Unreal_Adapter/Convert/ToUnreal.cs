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
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SS = System.Net.Sockets;
using System.Threading;
using BH.oM.Base;
using BH.oM.VirtualReality;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Graphics.MaterialFragments;
using System.Drawing;

namespace BH.Engine.Unreal
{
    public static partial class Convert
    {
        public static string ToUnreal(UnrealProjectSettings unrealPrjSettings)
        {
            return Serialize(unrealPrjSettings);
        }

        public static string ToUnreal(IEnumerable<UnrealMesh> unrealMeshes)
        {
            return Serialize(unrealMeshes);
        }

        public static string ToUnreal(IEnumerable<UnrealSpline> unrealSplines)
        {
            return Serialize(unrealSplines);
        }

        public static string ToUnreal(Project project)
        {
            List<string> messages = new List<string>();

            if (project.Meshes.Count() > 0)
                messages.Add(Wrap(project.Meshes));

            if (project.Splines.Count() > 0)
                messages.Add(Wrap(project.Splines));

            if (project.Nodes.Count() > 0)
                messages.Add(Wrap(project.Nodes));

            return Wrap(messages, project);
        }


        /*************************************/

        private static string Wrap(List<UnrealMesh> UnrealMeshes)
        {

            //Add Message Type

            string json = "[[[BHoMMeshes]]]";


            //Add Mesh Message

            json += ",";
            for (int i = 0; i < UnrealMeshes.Count; i++)
            {
                List<Vector> Normals = new List<Vector>();
                json += "[[mesh]],[[";
                json += "{\"vertices\": [";

                foreach (BH.oM.Geometry.Point vertex in UnrealMeshes[i].Mesh.Vertices)
                {
                    json += "[" + (vertex.X * 100).ToString("0.0") + "," + (vertex.Y * -100).ToString("0.0") + "," + (vertex.Z * 100).ToString("0.0") + "],";
                    Normals.Add(new Vector());
                }
                json = json.Trim(',') + "], \"faces\": [";

                foreach (BH.oM.Geometry.Face face in UnrealMeshes[i].Mesh.Faces)
                {
                    //if (face.IsQuad)
                    //    json += face.A + "," + face.B + "," + face.C + "," + face.D + "," + face.B + "," + face.C + ",";
                    //else
                    json += "[" + face.C + "," + face.B + "," + face.A + "],";
                    Vector faceNormal = BH.Engine.Geometry.Create.Plane(UnrealMeshes[i].Mesh.Vertices[face.C], UnrealMeshes[i].Mesh.Vertices[face.B], UnrealMeshes[i].Mesh.Vertices[face.A]).Normal;
                    Normals[face.C] = Normals[face.C] + faceNormal;
                    Normals[face.B] = Normals[face.B] + faceNormal;
                    Normals[face.A] = Normals[face.A] + faceNormal;
                }
                json = json.Trim(',') + "], \"normals\": [";


                foreach (Vector normal in Normals)
                {
                    Vector unitnormal = normal / normal.Length() * -1;
                    json += "[" + unitnormal.X.ToString("0.0") + "," + unitnormal.Y.ToString("0.0") + "," + unitnormal.Z.ToString("0.0") + "],";
                }


                //Add Material Message

                json = json.Trim(',') + "], \"material\": [";
                RenderMaterial meshMaterial = UnrealMeshes[i].RenderMaterial;

                string materialString = "[";
                materialString += (meshMaterial.BaseColor.R / 255.0).ToString() + ", " + (meshMaterial.BaseColor.G / 255.0).ToString() + ", " + (meshMaterial.BaseColor.B / 255.0).ToString();
                materialString += ", " + meshMaterial.Opacity.ToString() + ", " + meshMaterial.Glossiness.ToString();
                materialString += ", " + (meshMaterial.EmissiveColor.R / 255.0).ToString() + ", " + (meshMaterial.EmissiveColor.G / 255.0).ToString() + ", " + (meshMaterial.EmissiveColor.B / 255.0).ToString() + ", " + meshMaterial.Emissivity.ToString();

                materialString += "]";

                json += materialString;

                json = json.Trim(',') + "]}";
                json += "]],";

            }
            json = json.Trim(',') + "]";

            return json;
        }

        /*************************************/

        private static string Wrap(List<UnrealSpline> Splines)
        {

            //Add Message Type

            string json = "[[[BHoMSplines]]]";

            //Create Polylinelist

            string splineString = ",[";
            string valueString = ",[";
            foreach (UnrealSpline spline in Splines)
            {
                splineString += "[";
                valueString += "[";

                foreach (UnrealNode node in spline.Nodes)
                {

                    splineString += "[" + Math.Round(node.Position.X * 100, 0) + "," + Math.Round(node.Position.Y * -100, 0) + "," + Math.Round(node.Position.Z * 100, 0) + "],";

                    valueString += "[";
                    foreach (double value in node.Values)
                    {
                        valueString += value + ",";

                    }
                    valueString = valueString.TrimEnd(',') + "]" + ",";

                }

                splineString = splineString.TrimEnd(',') + "],";
                valueString = valueString.TrimEnd(',') + "],";

            }

            splineString = splineString.TrimEnd(',') + "]";
            valueString = valueString.TrimEnd(',') + "]";

            json += splineString + valueString;

            return json;
        }

        /*************************************/

        private static string Wrap(List<UnrealNode> Nodes)
        {

            //Add Message Type
            string json = "[[[BHoMNodes]]]";


            //Create Node Message

            string nodeString = ",[[";
            string valueString = ",[[";

            foreach (UnrealNode node in Nodes)
            {

                nodeString += "[" + Math.Round(node.Position.X * 100, 0) + "," + Math.Round(node.Position.Y * -100, 0) + "," + Math.Round(node.Position.Z * 100, 0) + "],";

                valueString += "[";
                foreach (double value in node.Values)
                {
                    valueString += value + ",";
                }
                valueString = valueString.TrimEnd(',') + "]" + ",";

            }

            nodeString = nodeString.TrimEnd(',') + "]]";
            valueString = valueString.TrimEnd(',') + "]]";

            json += nodeString + valueString;

            return json;
        }

        /*************************************/

        private static string Wrap(List<string> messages, Project project)
        {

            string json = "[[[[[" + project.Name + "]]]],";

            json += "[[[[" + project.SaveIndex + "]]]],";

            json += "[[[[" + project.Scale + "]]]],";

            json += "[[[[" + project.Unit + "]]]],";

            json += "[[[[" + project.AcousticMode + "]]]],";

            json += "[[[[" + project.ResultMin + "," + project.ResultMax + "]]]],";

            for (int i = 0; i < messages.Count; i++)
            {
                json += "[" + messages[i] + "],";
            }
            json = json.Trim(',') + "]";
            return json;
        }


        private static string Serialize(IEnumerable<IBHoMObject> bhomObjects)
        {
            List<string> outString = new List<string>();

            foreach (var o in bhomObjects)
                outString.Add(Serialize(o));

            return outString.Aggregate((i, j) => i + Environment.NewLine + j);
        }

        private static string Serialize(IBHoMObject bhomObject)
        {
            var outStream = new StringWriter();

            // Never include CustomData, otherwise serialization error
            var attrOverride = new XmlAttributeOverrides();
            attrOverride.Add(typeof(BHoMObject), "CustomData", new XmlAttributes() { XmlIgnore = true });

            // Never include CustomData, otherwise serialization error
            attrOverride.Add(typeof(BHoMObject), "Fragments", new XmlAttributes() { XmlIgnore = true });

            var serializer = new XmlSerializer(bhomObject.GetType(), attrOverride);

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); ns.Add("", ""); // Removes namespace bloat

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true };
            XmlWriter xmlWriter = XmlWriter.Create(outStream, xmlWriterSettings);

            serializer.Serialize(xmlWriter, bhomObject, ns);

            return outStream.ToString();
        }

    }
}
