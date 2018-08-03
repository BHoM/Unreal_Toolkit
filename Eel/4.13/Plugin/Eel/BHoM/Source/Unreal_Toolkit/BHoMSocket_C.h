// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "GameFramework/Actor.h"
#include "AllowWindowsPlatformTypes.h"
#include <stdio.h>
#include <winsock2.h>
#include <Ws2tcpip.h>
#include "HideWindowsPlatformTypes.h"
#include <string>
#include "BHoMSocket_C.generated.h"

UCLASS()
class UNREAL_TOOLKIT_API ABHoMSocket_C : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	ABHoMSocket_C();

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
	void static RunSocket(ABHoMSocket_C* socket);

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
