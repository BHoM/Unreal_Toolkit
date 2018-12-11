## Example Scripts

A collection of unreal projects utilising the socket reciever plugin in various ways. In order to recieve data through the sockets, you need to compile the BHoM_Engine and the Unreal_Toolkit. Check out the Wiki page for more information on how to set up the sockets in unreal.


### 1. MeshFromSocket

This Unreal project recieves socket messages containing mesh data for construction of an unreal Custom Mesh.
The project consists of the following 3 blueprints:
1. SocketReciever - Recieves socket messages
2. SocketMessageToMeshData - used to convert the socket message into an array of mesh vertices and array of point indicies.
3. Custom_Mesh - Construct the unreal Custom Mesh from an array of points and an array of indecies (TriMesh)

Attached to the script folder is a Grasshopper script with a mesh example that has been parsed into a string and can be pushed through the sockets and recieved in Unreal.
