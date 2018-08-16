using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using SS = System.Net.Sockets;
using System.Threading;
using BH.oM.Base;
using BH.oM.CFD.Elements;
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

            if (project.Geometry.Count() > 0)
            {
                messages.Add(WrapMesh(project.Geometry));
            }

            if (project.Streamers.Count() > 0)
            {
                messages.Add(WrapStreamers(project.Streamers));
            }

            if (project.Receivers.Count() > 0)
            {
                messages.Add(WrapReceivers(project.Receivers));
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

        private static string WrapMesh(List<UnrealGeometry> UnrealGeometries)
        {

            //Add Message Type

            string json = "[[[BHoMMeshes]]]";


            //Add Color Message

            json += ",[";
            for (int i = 0; i < UnrealGeometries.Count; i++)
            {

                List<string> colorStrings = UnrealGeometries[i].color.Split(new Char[] { ',' }, StringSplitOptions.None).ToList();
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
            for (int i = 0; i < UnrealGeometries.Count; i++)
            {
                List<Vector> Normals = new List<Vector>();
                json += "[[";
                json += "{\"vertices\": [";

                foreach (Point vertex in UnrealGeometries[i].mesh.Vertices)
                {
                    json += "[" + (vertex.X * 100).ToString("0.0") + "," + (vertex.Y * -100).ToString("0.0") + "," + (vertex.Z * 100).ToString("0.0") + "],";
                    Normals.Add(new Vector());
                }
                json = json.Trim(',') + "], \"faces\": [";

                

                
                foreach (BH.oM.Geometry.Face face in UnrealGeometries[i].mesh.Faces)
                {
                    //if (face.IsQuad)
                    //    json += face.A + "," + face.B + "," + face.C + "," + face.D + "," + face.B + "," + face.C + ",";
                    //else
                    json += face.C + "," + face.B + "," + face.A + ",";
                    Vector faceNormal = Create.Plane(UnrealGeometries[i].mesh.Vertices[face.C], UnrealGeometries[i].mesh.Vertices[face.B], UnrealGeometries[i].mesh.Vertices[face.A]).Normal;
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

        private static string WrapStreamers(List<Streamer> Streamers)
        {

            //Add Message Type
            string json = "[[[BHoMSplines]]]";


            //Add Material Message
            json += ",[[[" + "null" + "]]]";


            //Create Polylinelist
            json += ",[";
            foreach (Streamer streamer in Streamers)
            {
                json += "[";
                foreach (Node node in streamer.Nodes)
                {
                    json += "[" + Math.Round(node.Position.X * 100, 0) + "," + Math.Round(node.Position.Y * -100, 0) + "," + Math.Round(node.Position.Z * 100, 0) + "],";
                }
                json = json.Trim(',') + "],";
            }
            json = json.Trim(',') + "]";


            //Add Result Message

            json += ",[";

            foreach (Streamer streamer in Streamers)
            {
                json += "[";
                foreach (Node node in streamer.Nodes)
                {
                    if (node.CustomData.Keys.Contains("Result"))
                    {
                        json += "[" + (double)node.CustomData["Result"] + "]" + ",";
                    }
                    else
                    {
                        json += "[],";
                    }
                }
                json = json.Trim(',') + "],";
            }
            json = json.Trim(',') + "]";

            return json;
        }

        /*************************************/

        private static string WrapReceivers(List<Receiver> Receivers)
        {

            //Add Message Type
            string json = "[[[BHoMReceivers]]]";


            //Create Receiver list
            json += ",[[";
            foreach (Receiver receiver in Receivers)
            {
                    json += "[" + Math.Round(receiver.Location.X * 100, 0) + "," + Math.Round(receiver.Location.Y * -100, 0) + "," + Math.Round(receiver.Location.Z * 100, 0) + "],";
            }
            json = json.Trim(',') + "]]";


            //Add Result Message

            json += ",[[";

            foreach (Receiver receiver in Receivers)
            {
                
                if (receiver.CustomData.Keys.Contains("Result"))
                {
                    json += "[";
                    foreach (double value in (List<double>)receiver.CustomData["Result"])
                    {
                            json += value + ",";
                    }
                    json.Trim(',');
                    json += "]" + ",";
                }
                else
                {
                    json += "[],";
                }
            }
            json = json.Trim(',') + "]]";

            return json;
        }

        /*************************************/

        private static string WrapVRProject(List<string> messages, Project project)
        {

            string json = "[[[[[" + project.Name + "]]]],";

            json += "[[[[" + project.saveIndex + "]]]],";

            json += "[[[[" + project.Scale + "]]]],";

            json += "[[[[" + project.Unit + "]]]],";

            for (int i = 0; i < messages.Count; i++)
            {
                json += "[" + messages[i] + "],";
            }
            json = json.Trim(',') + "]";
            return json;
        }
    }
}
