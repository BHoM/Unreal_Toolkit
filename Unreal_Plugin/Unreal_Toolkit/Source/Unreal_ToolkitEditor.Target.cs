// Fill out your copyright notice in the Description page of Project Settings.

using UnrealBuildTool;
using System.Collections.Generic;

public class Unreal_ToolkitEditorTarget : TargetRules
{
	public Unreal_ToolkitEditorTarget(TargetInfo Target) : base (Target)
	{
		Type = TargetType.Editor;

        ExtraModuleNames.Add("Unreal_Toolkit");
	}

	//
	// TargetRules interface.
	//

}
