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

#include "BHoMFunctionLibrary.h"
#include "Unreal_Toolkit.h"
#include <list>


void UBHoMFunctionLibrary::JsonToLines(FString json, TArray<FVector>& startPoints, TArray<FVector>& endPoints)
{
	json.RemoveFromEnd(FString("]]]"));
	json.RemoveFromStart(FString("[[["));

	FString lineSeparator = "]],[[";
	FString pointSeparator = "],[";

	int jsonStart = 0;
	int jsonEnd = json.Len();

	TArray<FString> coordArray;

	while (jsonStart < jsonEnd)
	{
		//Get the substring for this polyline
		int lineEnd = json.Find(lineSeparator, ESearchCase::IgnoreCase, ESearchDir::FromStart, jsonStart);
		if (lineEnd < 0) lineEnd = jsonEnd;

		// Get the points from this line
		TArray<FVector> points;
		int pointStart = jsonStart;
		while (pointStart < lineEnd)
		{
			int pointEnd = json.Find(pointSeparator, ESearchCase::IgnoreCase, ESearchDir::FromStart, pointStart);
			if (pointEnd < 0) pointEnd = lineEnd;

			FString pointString = json.Mid(pointStart, pointEnd);
			pointString.ParseIntoArray(coordArray, TEXT(","), true);
			if (coordArray.Num() < 3) continue;

			points.Add(FVector(FCString::Atof(*coordArray[0]), FCString::Atof(*coordArray[1]), FCString::Atof(*coordArray[2])));
			pointStart = pointEnd + 3;
		}

		// Create list of start and end points
		for (int i = 1; i < points.Num(); i++)
		{
			endPoints.Add(points[i]);
			startPoints.Add(points[i - 1]);

		}

		// Get ready to process the next part
		jsonStart = lineEnd + 5;
	}
}


void UBHoMFunctionLibrary::JsonToMesh(FString json, TArray<FVector>& vertices, TArray<int>& triangles, TArray<FVector>& normals)
{
	json.RemoveFromEnd(FString("]}"));
	json.RemoveFromStart(FString("{\"vertices\": [["));

	FString faceSeparator = "], \"faces\": [";
	FString normalSeparator = "], \"normals\": [";
	FString pointSeparator = "],[";

	int jsonStart = 0;
	int jsonEnd = json.Len();
	int faceStart = json.Find(faceSeparator, ESearchCase::IgnoreCase, ESearchDir::FromStart, jsonStart);
	int normalStart = json.Find(normalSeparator, ESearchCase::IgnoreCase, ESearchDir::FromStart, jsonStart);

	TArray<FString> coordArray;

	// Get the vertices
	int pointStart = jsonStart;
	while (pointStart < faceStart)
	{
		int pointEnd = json.Find(pointSeparator, ESearchCase::IgnoreCase, ESearchDir::FromStart, pointStart);
		if (pointEnd < 0) pointEnd = faceStart;

		FString pointString = json.Mid(pointStart, pointEnd);
		pointString.ParseIntoArray(coordArray, TEXT(","), true);
		if (coordArray.Num() < 3) continue;

		vertices.Add(FVector(FCString::Atof(*coordArray[0]), FCString::Atof(*coordArray[1]), FCString::Atof(*coordArray[2])));
		pointStart = pointEnd + 3;
	}

	// Get the faces
	pointStart = faceStart + 13;
	coordArray.Empty();
	FString intString = json.Mid(pointStart, normalStart);
	intString.ParseIntoArray(coordArray, TEXT(","), true);
	for (int i = 0; i < coordArray.Num(); i++)
		triangles.Add(FCString::Atof(*coordArray[i]));

	// Get the normals
	if (normalStart >= 0)
	{
		pointStart = normalStart + 15;
		while (pointStart < jsonEnd)
		{
			int pointEnd = json.Find(pointSeparator, ESearchCase::IgnoreCase, ESearchDir::FromStart, pointStart);
			if (pointEnd < 0) pointEnd = jsonEnd;

			FString pointString = json.Mid(pointStart, pointEnd);
			pointString.ParseIntoArray(coordArray, TEXT(","), true);
			if (coordArray.Num() < 3) continue;

			normals.Add(FVector(FCString::Atof(*coordArray[0]), FCString::Atof(*coordArray[1]), FCString::Atof(*coordArray[2])));
			pointStart = pointEnd + 3;
		}
	}
}


void UBHoMFunctionLibrary::JsonToArray(FString json, TArray<FString>& items)
{
	json.RemoveFromEnd(FString("]"));
	json.RemoveFromStart(FString("["));

	int startPos = 0;
	int jsonEnd = json.Len();
	int level = 0;

	for (int i = 0; i < jsonEnd; i++)
	{
		const char c = json[i];
		if (c == '[' || c == '{')
			level++;
		else if (c == ']' || c == '}')
			level--;

		if (i == jsonEnd-1 || (c == ',' && level == 0))
		{
			items.Add(json.Mid(startPos, i));
			startPos = i + 1;
		}		
	}
}


void UBHoMFunctionLibrary::JsonToFloats(FString json, TArray<float>& items)
{
	json.RemoveFromEnd(FString("]"));
	json.RemoveFromStart(FString("["));

	TArray<FString> stringArray;
	json.ParseIntoArray(stringArray, TEXT(","), true);
	for (int i = 0; i < stringArray.Num(); i++)
		items.Add(FCString::Atof(*stringArray[i]));
}

void UBHoMFunctionLibrary::BHoMProjectSettings(FString json, FString& Name, FString& SaveIndex, FString& Scale, FString& Unit, FString& ResultMax, FString& ResultMin)
{
	pugi::xml_document doc;

	pugi::xml_parse_result result = doc.load_string(TCHAR_TO_ANSI(*json));

	auto text = doc.child("UnrealProjectSettings").child("Name").text().as_string();
	Name = ANSI_TO_TCHAR(text);

	text = doc.child("UnrealProjectSettings").child("SaveIndex").text().as_string();
	SaveIndex = ANSI_TO_TCHAR(text);

	text = doc.child("UnrealProjectSettings").child("Scale").text().as_string();
	Scale = ANSI_TO_TCHAR(text);

	text = doc.child("UnrealProjectSettings").child("Unit").text().as_string();
	Unit = ANSI_TO_TCHAR(text);

	text = doc.child("UnrealProjectSettings").child("ResultMax").text().as_string();
	ResultMax = ANSI_TO_TCHAR(text);

	text = doc.child("UnrealProjectSettings").child("ResultMin").text().as_string();
	ResultMin = ANSI_TO_TCHAR(text);
}

void UBHoMFunctionLibrary::BHoMMesh(FString json, TArray<FVector>& vertices, TArray<int>& triangles)
{
	pugi::xml_document doc;

	pugi::xml_parse_result result = doc.load_string(TCHAR_TO_ANSI(*json));

	auto nodes = doc.select_nodes("UnrealMesh/Mesh/Vertices/Point");

	for (int i = 0; i < nodes.size(); i++)
	{
		auto x = FCString::Atof(ANSI_TO_TCHAR(nodes[i].node().select_node("X").node().text().as_string()));
		auto y = FCString::Atof(ANSI_TO_TCHAR(nodes[i].node().select_node("Y").node().text().as_string()));
		auto z = FCString::Atof(ANSI_TO_TCHAR(nodes[i].node().select_node("Z").node().text().as_string()));

		vertices.Add(FVector(x, y, z));
	}

	nodes = doc.select_nodes("UnrealMesh/Mesh/Faces/Face");

	for (int i = 0; i < nodes.size(); i++)
	{
		auto f = FCString::Atof(ANSI_TO_TCHAR(nodes[i].node().select_node("A").node().text().as_string()));

		triangles.Add(f);
	}

}

//void UBHoMFunctionLibrary::BHoMProjectSettings1_works(FString json, TArray<FString>& SaveIndex)
//{
//	pugi::xml_document doc;
//
//	pugi::xml_parse_result result = doc.load_string(TCHAR_TO_ANSI(*json));
//
//	auto text = doc.child("UnrealProjectSettings").child("SaveIndex").text().as_string();
//
//	saveIndex.Add(ANSI_TO_TCHAR(text));
//}

