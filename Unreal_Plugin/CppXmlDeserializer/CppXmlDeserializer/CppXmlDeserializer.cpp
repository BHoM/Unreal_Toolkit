// CppXmlDeserializer.cpp : Defines the entry point for the console application.
//

#include "pugixml.hpp"
#include "stdafx.h"
#include <iostream>

// http://www.gerald-fahrnholz.eu/sw/DocGenerated/HowToUse/html/group___grp_pugi_xml.html


bool readProjectSettings() 
{
	pugi::xml_document doc;

	/// ProjectSettings
	//std::string json = "<UnrealProjectSettings><SaveIndex>1</SaveIndex></UnrealProjectSettings>";
	//const char *cstr = json.c_str();
	//pugi::xml_parse_result result = doc.load_string(cstr);
	//std::cout << "Load result: " << result.description() << ", settings: " << doc.child("UnrealProjectSettings").child("SaveIndex").text().as_string() << std::endl;

	std::string pathtext = "C:\\temp\\unrealProjectSettings.txt";
	const char *path = pathtext.c_str();
	pugi::xml_parse_result result = doc.load_file(path);

	if (!result)
	{
		std::cout << "Parse error: " << result.description()
			<< ", character pos= " << result.offset;
	}

	std::cout << "\n SaveIndex " << doc.child("UnrealProjectSettings").child("SaveIndex").text().as_string();
	//std::cout << "\n SaveIndex " << doc.select_node("UnrealProjectSettings/SaveIndex").node().text().as_string(); //alternative way, useless though as you have to insert the full path anyway
	std::cout << "\n Scale " << doc.child("UnrealProjectSettings").child("Scale").text().as_string();
	std::cout << "\n Unit " << doc.child("UnrealProjectSettings").child("Unit").text().as_string();
	std::cout << "\n ResultMax " << doc.child("UnrealProjectSettings").child("ResultMax").text().as_string();
	std::cout << "\n ResultMin " << doc.child("UnrealProjectSettings").child("ResultMin").text().as_string();

	return true;
}

bool readMeshes() 
{
	///// Meshes

	pugi::xml_document doc;

	std::string pathtext = "C:\\temp\\unrealMeshes.txt";
	const char *path = pathtext.c_str();
	pugi::xml_parse_result result = doc.load_file(path);


	if (!result)
	{
		std::cout << "Parse error: " << result.description()
			<< ", character pos= " << result.offset;
	}

	// TODO: Recreate the behaviour of the function JsonToMesh in the C++ UnrealToolkitProject plugin solution

	//std::cout << "\n Meshes " << doc.child("UnrealMesh").text().as_string();
	auto nodes = doc.select_nodes("UnrealMesh/Mesh/Vertices/Point");
	std::cout << nodes.size();
	for (int i = 0; i < nodes.size(); i++)
	{
		std::cout << "\n X: " << nodes[i].node().select_node("X").node().text().as_string();

	}

	return true;
}





///----------------------
/// MAIN
///----------------------
int main()
{
	//readProjectSettings();

	readMeshes();
}
