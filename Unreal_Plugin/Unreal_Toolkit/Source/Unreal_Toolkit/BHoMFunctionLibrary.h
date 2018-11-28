// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "Kismet/BlueprintFunctionLibrary.h"
#include "BHoMFunctionLibrary.generated.h"

/**
 * 
 */
UCLASS()
class UNREAL_TOOLKIT_API UBHoMFunctionLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_BODY()
	
public:
	UFUNCTION(BlueprintCallable, BlueprintPure, Category = "BHoM.JSON")
		static void JsonToLines(FString json, TArray<FVector>& startPoints, TArray<FVector>& endPoints);

	UFUNCTION(BlueprintCallable, BlueprintPure, Category = "BHoM.JSON")
		static void JsonToMesh(FString json, TArray<FVector>& vertices, TArray<int>& triangles, TArray<FVector>& normals);

	UFUNCTION(BlueprintCallable, BlueprintPure, Category = "BHoM.JSON")
		static void JsonToArray(FString json, TArray<FString>& items);


	UFUNCTION(BlueprintCallable, BlueprintPure, Category = "BHoM.JSON")
		static void JsonToFloats(FString json, TArray<float>& items);

};
