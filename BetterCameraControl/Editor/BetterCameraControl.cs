using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;





[InitializeOnLoad]
public class BetterCameraControl
{
	private static int viewToolID = GUIUtility.GetControlID(FocusType.Keyboard);
	private static bool rightMBDown = false;
	private static BetterCameraControlData data => BetterCameraControlData.instance;

	static BetterCameraControl()
	{
		SceneView.duringSceneGui += DuringSceneGui;
		SceneView.beforeSceneGui += BeforeSceneGUI;
		SceneViewCameraWindow.additionalSettingsGui += AdditionalSettingsGui;
	}



	private static void BeforeSceneGUI(SceneView sceneView)
	{
		
	}

	private static void DuringSceneGui(SceneView sceneView)
	{
		Event ev = Event.current;
		EventType evType = ev.GetTypeForControl(viewToolID);
		switch(evType)
		{
			case EventType.MouseDown:
				if(ev.button == 1) rightMBDown = true;
				break;
			case EventType.MouseUp:
				if(ev.button == 1) rightMBDown = false;
				break;
			case EventType.ScrollWheel:
				if(data.useAdaptiveSpeedChange && rightMBDown)
				{
					float newSpeed = sceneView.cameraSettings.speed * (1 - ev.delta.y * data.adaptiveSpeedChangeRatio);
					sceneView.cameraSettings.speedMin = newSpeed;
					sceneView.cameraSettings.speedMax = newSpeed;
					sceneView.cameraSettings.speed = newSpeed;
					ev.Use();
				}
				break;
		}
	}
	
	private static void AdditionalSettingsGui(SceneView sceneView)
	{
		var settings = sceneView.cameraSettings;
		{
			float value = settings.easingDuration;
			EditorGUI.BeginChangeCheck();
			value = EditorGUILayout.Slider("Camera Easing Duration", value, 0f, 1f);
			if(EditorGUI.EndChangeCheck())
				settings.easingDuration = value;
		}
		{
			bool value = data.useAdaptiveSpeedChange;
			EditorGUI.BeginChangeCheck();
			value = EditorGUILayout.Toggle("Use Speed Change Ratio", value);
			if(EditorGUI.EndChangeCheck())
			{
				if(!value)
				{
					settings.speedMin = settings.speedMax * 0.5f;
				}
				data.useAdaptiveSpeedChange = value;
				data.Save();
			}
		}
		{
			float value = data.adaptiveSpeedChangeRatio;
			EditorGUI.BeginChangeCheck();
			value = EditorGUILayout.Slider("Speed Change Ratio", value, 0f, 1f);
			if(EditorGUI.EndChangeCheck())
			{
				data.adaptiveSpeedChangeRatio = value;
				data.Save();
			}
		}

	}
}
