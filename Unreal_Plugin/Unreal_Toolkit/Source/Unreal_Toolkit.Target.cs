// Fill out your copyright notice in the Description page of Project Settings.

using UnrealBuildTool;
using System.Collections.Generic;

public class Unreal_ToolkitTarget : TargetRules
{
	public Unreal_ToolkitTarget(TargetInfo Target) : base (Target)
	{
		Type = TargetType.Game;

        ExtraModuleNames.Add("Unreal_Toolkit");
	}

	//
	// TargetRules interface.
	//

}
