// Fill out your copyright notice in the Description page of Project Settings.

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



