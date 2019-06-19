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



namespace BH.Adapter.Unreal
{
    public partial class UnrealAdapter
    {
        /***************************************************/
        /**** Adapter overload method                   ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;

            if (objects.Count() == 0)
                return false;

            if (objects.First() is Project)
            {
                BH.Engine.Reflection.Compute.RecordWarning("Project is being superseded. Please consider pushing UnrealProject instead.");

                if (objects.Count() > 1)
                {
                    BH.Engine.Reflection.Compute.RecordError("Please push only one Project at a time.\nAlso, do not push a Project together with other IBHoMObjects. IBHoM objects should be inserted directly into the Project.");
                    return false;
                }

                success = Create(objects.First() as Project);
            }

            if (objects.First() is UnrealProjectSettings)
            {
                if (objects.Count() > 1)
                {
                    BH.Engine.Reflection.Compute.RecordError("Please push only one Unreal Project Settings at a time.");
                    return false;
                }

                success = Create(objects.First() as UnrealProjectSettings);
            }

            if (objects.First() is UnrealNode)
            {
                BH.Engine.Reflection.Compute.RecordError($"Push to Unreal only supports Splines and Meshes. Not compatible with {objects.First().GetType().ToString()}");
                return false;
            }

            if (objects.First() is UnrealSpline)
            {
                success = CreateCollection(objects.Cast<UnrealSpline>());
            }

            if (objects.First() is UnrealMesh)
            {
                success = CreateCollection(objects.Cast<UnrealMesh>());
            }

            return success;
        }

        protected bool Create(Project project)
        {
            string data = BH.Engine.Unreal.Convert.ToUnreal(project);

            return SendData(m_address, m_port, data);

        }


        protected bool Create(UnrealProjectSettings settings)
        {
            string data = BH.Engine.Unreal.Convert.ToUnreal(settings);

            using (System.IO.TextWriter gna = new System.IO.StreamWriter("C:\\temp\\unrealProjectSettings.txt"))
                gna.Write(data);

            return SendData(m_address, m_port, data);
        }

        protected bool CreateCollection(IEnumerable<UnrealSpline> unrealSplines)
        {
            string data = BH.Engine.Unreal.Convert.ToUnreal(unrealSplines);

            return SendData(m_address, m_port, data);
        }

        protected bool CreateCollection(IEnumerable<UnrealMesh> unrealMeshes)
        {
            string data = BH.Engine.Unreal.Convert.ToUnreal(unrealMeshes);

            using (System.IO.TextWriter gna = new System.IO.StreamWriter("C:\\temp\\unrealMeshes.txt"))
                gna.Write(data);

            return SendData(m_address, m_port, data);
        }



        /***************************************************/
        /**** Private methods                           ****/
        /***************************************************/


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

        private static bool SendInt(SS.Socket mySocket, IPEndPoint ePoint, Int32 value)
        {
            value = IPAddress.HostToNetworkOrder(value); //Convert long from Host Byte Order to Network Byte Order
            if (!SendAll(mySocket, ePoint, BitConverter.GetBytes(value))) //Try to send int... If int fails to send
                return false; //Return false: int not successfully sent
            return true; //Return true: int successfully sent
        }


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

        /***************************************************/
    }
}
