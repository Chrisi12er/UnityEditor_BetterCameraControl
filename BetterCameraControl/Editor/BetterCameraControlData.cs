using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[FilePath("UserSettings/BetterCameraControlSettings.asset", FilePathAttribute.Location.ProjectFolder)]
public class BetterCameraControlData : ScriptableSingleton<BetterCameraControlData>
{
	public bool useAdaptiveSpeedChange = true;
	public float adaptiveSpeedChangeRatio = 0.08f;

	public void Save()
	{ 
		Save(true);
	}
}
