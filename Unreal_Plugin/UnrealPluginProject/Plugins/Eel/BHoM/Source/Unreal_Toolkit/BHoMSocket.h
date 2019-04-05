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

#pragma once

#include "GameFramework/Actor.h"
#include "AllowWindowsPlatformTypes.h"
#include <stdio.h>
#include <winsock2.h>
#include <Ws2tcpip.h>
#include "HideWindowsPlatformTypes.h"
#include <string>
#include "BHoMSocket.generated.h"

UCLASS()
class UNREAL_TOOLKIT_API ABHoMSocket : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ABHoMSocket();

	// Called when the game starts or when spawned
	virtual void BeginPlay() override;
	
	// Called when the game ends or when destroyed
	virtual void EndPlay(const EEndPlayReason::Type EndPlayReason) override;

	UPROPERTY(EditAnywhere, BlueprintReadWrite, Meta=(ExposeOnSpawn = true), Category = Socket)
	int32 Port;

	UPROPERTY(BlueprintReadOnly)
	FString Message;

	UPROPERTY(BlueprintReadWrite)
	bool HasNewMessage;

	UFUNCTION(BlueprintImplementableEvent, meta = (DisplayName = "Socket Message"))
	void OnSocketMessage(const FString& msg);

	SOCKET& GetSocket()
	{
		return mySocket;
	}

private:

	// Socket handling methods
	SOCKET static CreateSocket(int port);
	void static RunSocket(ABHoMSocket* socket);

	// Data receiving methods
	bool static GetInt(SOCKET &s, int& value);
	bool static GetString(SOCKET &s, std::string & _string);
	bool static GetAll(SOCKET &s, char* data, int totalbytes);

	// Screen message methods
	FORCEINLINE void static ScreenMsg(const FString& Msg)
	{
		GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Red, *Msg);
	}
	FORCEINLINE void static ScreenMsg(const FString& Msg, const float Value)
	{
		GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Red, FString::Printf(TEXT("%s %f"), *Msg, Value));
	}
	FORCEINLINE void static ScreenMsg(const FString& Msg, const FString& Msg2)
	{
		GEngine->AddOnScreenDebugMessage(-1, 5.f, FColor::Red, FString::Printf(TEXT("%s %s"), *Msg, *Msg2));
	}

	// Private variables
	SOCKET mySocket;
	HANDLE socketThread;
	
};
