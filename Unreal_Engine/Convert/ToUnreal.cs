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
using System.Net;
using SS = System.Net.Sockets;
using System.Threading;
using BH.oM.Base;
using BH.oM.VirtualReality;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.Acoustic;

namespace BH.Engine.Unreal
{
    public static partial class Convert
    {
        public static bool ToUnreal(string address, int port, Project project)
        {
            List<string> messages = new List<string>();

            if (project.Meshes.Count() > 0)
            {
                messages.Add(WrapMesh(project.Meshes));
            }

            if (project.Splines.Count() > 0)
            {
                messages.Add(WrapSplines(project.Splines));
            }

            if (project.Nodes.Count() > 0)
            {
                messages.Add(WrapNodes(project.Nodes));
            }

            string final = WrapVRProject(messages, project);
            return SendData(address, port, final);
        }


        private static bool SendData(string host, int destPort, string data)
        {
            IPAddress dest = Dns.GetHostAddresses(host)[0]; //Get the destination IP Address
            IPEndPoint ePoint = new IPEndPoint(dest, destPort);
            SS.Socket mySocket = new SS.Socket(SS.AddressFamily.InterNetwork, SS.SocketType.Dgram, SS.ProtocolType.Udp); //Create a socket using the same protocols as in the Javascript file (Dgram and Udp)

            //byte[] outBuffer = Encoding.ASCII.GetBytes(data); //Convert the data to a byte array
            //int nbBytes = mySocket.SendTo(outBuffer, ePoint); //Send the data to the socket

            bool done = SendString(mySocket, ePoint, data);

            mySocket.Close(); //Socket use over, time to close it

            return done;
        }


        /*************************************/
        /****  Private methods            ****/
        /*************************************/

        private static bool SendInt(SS.Socket mySocket, IPEndPoint ePoint, Int32 value)
        {
            value = IPAddress.HostToNetworkOrder(value); //Convert long from Host Byte Order to Network Byte Order
            if (!SendAll(mySocket, ePoint, BitConverter.GetBytes(value))) //Try to send int... If int fails to send
                return false; //Return false: int not successfully sent
            return true; //Return true: int successfully sent
        }

        /*************************************/

        private static bool SendString(SS.Socket mySocket, IPEndPoint ePoint, string message)
        {
            Int32 bufferlength = message.Count(); //Find string buffer length
            if (!SendInt(mySocket, ePoint, bufferlength)) //Send length of string buffer, If sending buffer length fails...
                return false; //Return false: Failed to send string buffer length
            return SendAll(mySocket, ePoint, Encoding.ASCII.GetBytes(message)); //Try to send string buffer
        }

        /*************************************/

        private static bool SendAll(SS.Socket mySocket, IPEndPoint ePoint, byte[] data)
        {
            int totalbytes = data.Length;

            int bytessent = 0; //Holds the total bytes sent
            while (bytessent < totalbytes) //While we still have more bytes to send
            {
                try
                {
                    int nbBytes = Math.Min(5000, totalbytes - bytessent);
                    int RetnCheck = mySocket.SendTo(new ArraySegment<byte>(data, bytessent, nbBytes).ToArray(), ePoint); //Try to send remaining bytes
                    bytessent += RetnCheck; //Add to total bytes sent
                    Thread.Sleep(50);
                }
                catch
                {
                    return false;
                }
            }

            return true; //Success!
        }

        /*************************************/

        private static string WrapMesh(List<UnrealMesh> UnrealMeshes)
        {

            //Add Message Type

            string json = "[[[BHoMMeshes]]]";


            //Add Color Message

            json += ",[";
            for (int i = 0; i < UnrealMeshes.Count; i++)
            {

                List<string> colorStrings = UnrealMeshes[i].Color.Split(new Char[] { ',' }, StringSplitOptions.None).ToList();
                double colorScale = 1.00 / 255.00;
                string ColorName = "[";
                for (int j = 0; j < colorStrings.Count; j++)
                {
                    double colorValue = Math.Round(colorScale * double.Parse(colorStrings[j]), 3);
                    ColorName += colorValue + ",";
                }
                ColorName = ColorName.Trim(',') + "]";

                json += "[" + ColorName + "],";
            }
            json = json.Trim(',') + "]";


            //Add Mesh Message

            json += ",[";
            for (int i = 0; i < UnrealMeshes.Count; i++)
            {
                List<Vector> Normals = new List<Vector>();
                json += "[[";
                json += "{\"vertices\": [";

                foreach (Point vertex in UnrealMeshes[i].Mesh.Vertices)
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
                    json += face.C + "," + face.B + "," + face.A + ",";
                    Vector faceNormal = Create.Plane(UnrealMeshes[i].Mesh.Vertices[face.C], UnrealMeshes[i].Mesh.Vertices[face.B], UnrealMeshes[i].Mesh.Vertices[face.A]).Normal;
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

                json = json.Trim(',') + "]}";
                json += "]],";
            }
            json = json.Trim(',') + "]";

            return json;
        }

        /*************************************/

        private static string WrapSplines(List<UnrealSpline> Splines)
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

            json += splineString + valueString ;

            return json;
        }

        /*************************************/

        private static string WrapNodes(List<UnrealNode> Nodes)
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

        private static string WrapVRProject(List<string> messages, Project project)
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
    }
}
