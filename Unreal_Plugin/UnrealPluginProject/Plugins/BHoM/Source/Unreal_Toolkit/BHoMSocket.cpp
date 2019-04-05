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

#include "Unreal_Toolkit.h"
#include "BHoMSocket.h"



// Sets default values
ABHoMSocket::ABHoMSocket()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = false;

	Port = 8888;
}

// Called when the game starts or when spawned
void ABHoMSocket::BeginPlay()
{
	Super::BeginPlay();
	
	mySocket = CreateSocket(Port);
	socketThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)RunSocket, (LPVOID)(this), NULL, NULL);
}


// Called when the game ends or when destroyed
void ABHoMSocket::EndPlay(const EEndPlayReason::Type EndPlayReason)
{
	Super::EndPlay(EndPlayReason);

	CloseHandle(socketThread);
	closesocket(mySocket);
	WSACleanup();
}

SOCKET ABHoMSocket::CreateSocket(int port)
{
	//Initialise winsock
	WSADATA wsa;
	ScreenMsg(FString("\nInitialising Winsock..."));
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
	{
		ScreenMsg(FString("Failed. Error Code: "), WSAGetLastError());
		return 0;
	}
	ScreenMsg(FString("Initialised.\n"));

	//Create a socket
	SOCKET s;
	if ((s = socket(AF_INET, SOCK_DGRAM, 0)) == INVALID_SOCKET)
	{
		ScreenMsg(FString("Could not create socket: "), WSAGetLastError());
		return 0;
	}
	ScreenMsg(FString("Socket created.\n"));

	//Prepare the sockaddr_in structure
	struct sockaddr_in server;
	server.sin_family = AF_INET;
	server.sin_addr.s_addr = INADDR_ANY;
	server.sin_port = htons(port);

	//Bind
	if (bind(s, (struct sockaddr *)&server, sizeof(server)) == SOCKET_ERROR)
	{
		ScreenMsg(FString("Binding of address failed with error code: "), WSAGetLastError());
		return 0;
	}
	ScreenMsg(FString("Waiting for data..."));

	return s;
}

void ABHoMSocket::RunSocket(ABHoMSocket* socket)
{
	while (1)
	{
		std::string msg; //string to store our message we received
		if (GetString(socket->GetSocket(), msg))
		{
			socket->Message = FString(msg.c_str());
			socket->HasNewMessage = true;
			//socket->OnSocketMessage(FString(msg.c_str()));
		}

		Sleep(100);
	}

}


bool ABHoMSocket::GetInt(SOCKET &s, int& value)
{
	int32_t buff;
	if (!GetAll(s, (char*)&buff, sizeof(int32_t))) //Try to receive long (4 byte int)... If int fails to be recv'd
		return false;

	value = ntohl(buff); //Convert long from Network Byte Order to Host Byte Order
	return true;
}

bool ABHoMSocket::GetString(SOCKET &s, std::string & _string)
{
	int bufferlength; //Holds length of the message
	if (!GetInt(s, bufferlength)) //Get length of buffer and store it in variable: bufferlength
		return false;

	char * buffer = new char[bufferlength + 1];
	buffer[bufferlength] = '\0'; //Set last character of buffer to be a null terminator so we aren't printing memory that we shouldn't be looking at
	if (!GetAll(s, buffer, bufferlength)) //receive message and store the message in buffer array. If buffer fails to be received...
	{
		delete[] buffer;
		return false;
	}

	_string = buffer;
	delete[] buffer;
	return true;
}

bool ABHoMSocket::GetAll(SOCKET &s, char* data, int totalbytes)
{
	struct sockaddr_in si_other;
	int slen = sizeof(si_other);

	int bytesreceived = 0; //Holds the total bytes received
	while (bytesreceived < totalbytes)
	{
		int RetnCheck = recvfrom(s, data + bytesreceived, totalbytes - bytesreceived, 0, (struct sockaddr *) &si_other, &slen); //Try to recv remaining bytes
		if (RetnCheck == SOCKET_ERROR)
			return false;

		bytesreceived += RetnCheck;
	}
	return true;
}
