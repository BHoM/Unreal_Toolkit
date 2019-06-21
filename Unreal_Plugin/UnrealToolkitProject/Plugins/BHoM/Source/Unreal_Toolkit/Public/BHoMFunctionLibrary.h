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

#include "pugixml.hpp"
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

	UFUNCTION(BlueprintCallable, BlueprintPure, Category = "BHoM.JSON")
		static void BHoMProjectSettings(FString json, FString & Name, FString & SaveIndex, FString & Scale, FString & Unit, FString & ResultMax, FString & ResultMin);

	UFUNCTION(BlueprintCallable, BlueprintPure, Category = "BHoM.JSON")
		static void BHoMMesh(FString json, TArray<FVector>& vertices, TArray<int>& triangles);


};
