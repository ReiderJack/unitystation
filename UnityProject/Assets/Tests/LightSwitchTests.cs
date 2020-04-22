﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lighting;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tests
{
    public class LightSwitchTests
    {
	    List<APCPoweredDevice> GetAllPoweredDevicesInTheScene()
	    {
		    List<APCPoweredDevice> objectsInScene = new List<APCPoweredDevice>();

		    foreach (APCPoweredDevice go in Resources.FindObjectsOfTypeAll(typeof(APCPoweredDevice)))
		    {
			    if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
				    objectsInScene.Add(go);
		    }

		    return objectsInScene;
	    }

	    [Test]
	    [Ignore("Need to assign everything")]
	    public void FindAll_PoweredDevices_WithoutRelatedAPC()
	    {
		    int count = 0;
		    List<string> devicesWithoutAPC = new List<string>();
		   var listOfDevices =  GetAllPoweredDevicesInTheScene();
		   var report = new StringBuilder();
		   Logger.Log("Powered Devices without APC", Category.Tests);
		   foreach (var objectDevice in listOfDevices)
		   {
			   var device = objectDevice as APCPoweredDevice;
			   if(device.IsSelfPowered) continue;
			   if (device.RelatedAPC == null)
			   {
				   count++;
				   var obStr = objectDevice.name;
				   devicesWithoutAPC.Add(obStr);
				   Logger.Log(obStr, Category.Tests);
				   report.AppendLine(obStr);
			   }
		   }
		   Assert.That(count, Is.EqualTo(0),$"APCPoweredDevice count in the scene: {listOfDevices.Count}");
	    }

		private List<LightSource> GetAllLightSourcesInTheScene()
		{
		  List<LightSource> objectsInScene = new List<LightSource>();

		  foreach (LightSource go in Resources.FindObjectsOfTypeAll(typeof(LightSource)))
		  {
			  if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
				  objectsInScene.Add(go);
		  }

		  return objectsInScene;
		}

	    [Test]
	    [Ignore("Need to assign everything")]
	    public void FindAll_LightSources_WithoutRelatedSwitch()
	    {
		    int count = 0;
		    List<string> devicesWithoutAPC = new List<string>();
		    var listOfDevices =  GetAllLightSourcesInTheScene();
		    var report = new StringBuilder();
		    Logger.Log("LightSource without Switches", Category.Tests);
		    foreach (var objectDevice in listOfDevices)
		    {

			    var device = objectDevice as LightSource;
			    if(device.IsWithoutSwitch) continue;
			    if (device.relatedLightSwitch == null)
			    {
				    count++;
				    var obStr = objectDevice.name;
				    devicesWithoutAPC.Add(obStr);
				    Logger.Log(obStr, Category.Tests);
				    report.AppendLine(obStr);
			    }
		    }
		    Assert.That(count, Is.EqualTo(0),$"LightSource count in the scene: {listOfDevices.Count}");
	    }

		List<LightSwitchV2> GetAllLightSwitchesInTheScene()
		{
			List<LightSwitchV2> objectsInScene = new List<LightSwitchV2>();

			foreach (LightSwitchV2 go in Resources.FindObjectsOfTypeAll(typeof(LightSwitchV2)))
			{
				if (!EditorUtility.IsPersistent(go.transform.root.gameObject) &&
				    !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
					objectsInScene.Add(go);
			}

			return objectsInScene;
		}

	    [Test]
	    [Ignore("Need to assign everything")]
	    public void FindAll_Switches_WithoutLightSources()
	    {
		    int count = 0;
		    List<string> devicesWithoutAPC = new List<string>();
		    var listOfDevices =  GetAllLightSwitchesInTheScene();
		    var report = new StringBuilder();
		    Logger.Log("Light switches without Lights", Category.Tests);
		    foreach (var objectDevice in listOfDevices)
		    {
			    var device = objectDevice;
			    if (device.listOfLights.Count == 0)
			    {
				    count++;
				    var obStr = objectDevice.name;
				    devicesWithoutAPC.Add(obStr);
				    Logger.Log(obStr, Category.Tests);
				    report.AppendLine(obStr);
			    }
		    }
		    Assert.That(count, Is.EqualTo(0),$"Switches count in the scene: {listOfDevices.Count}");
	    }

	    [Test]
	    [Ignore("Need to assign everything")]
	    public void CheckAll_SwitchesFor_LightSourcesInTheList()
	    {
		    // Checks if light source from switch list
		    // is assigned to the switch
		    int count = 0;
		    List<string> devicesWithoutAPC = new List<string>();
		    var listOfDevices =  Resources.FindObjectsOfTypeAll(typeof(LightSwitchV2));
		    var report = new StringBuilder();
		    Logger.Log("LightSources without properly defined switch", Category.Tests);
		    foreach (var objectDevice in listOfDevices)
		    {
			    var device = objectDevice as LightSwitchV2;
			    if (device.listOfLights.Count == 0)
			    {
				    continue;
			    }
			    foreach (var light in device.listOfLights)
			    {
				    if (light.relatedLightSwitch != device)
				    {
					    string obStr = light.name;
					    devicesWithoutAPC.Add(obStr);
					    string lightSwitch = light.relatedLightSwitch == null ? "null" : light.relatedLightSwitch.name;
					    Logger.Log($"\"{obStr}\" relatedSwitch is \"{lightSwitch}\", supposed to be \"{device.name}\"", Category.Tests);
					    report.AppendLine(obStr);
					    count++;
				    }
			    }
		    }
		    Assert.That(count, Is.EqualTo(0),$"APCs count in the scene: {listOfDevices.Length}");
	    }

	    [Test]
	    [Ignore("Need to assign everything")]
	    public void CheckAll_APCsFor_ConnectedDevicesInTheList()
	    {
		    // Checks if powered device from APC list
		    // is assigned to the APC
		    int count = 0;
		    List<string> devicesAPC = new List<string>();
		    var listOfAPCs =  Resources.FindObjectsOfTypeAll(typeof(APC));
		    var report = new StringBuilder();
		    Logger.Log("Devices without properly defined APC", Category.Tests);
		    foreach (var apc in listOfAPCs)
		    {
			    var device = apc as APC;
			    if (device.ConnectedDevices.Count == 0)
			    {
				    continue;
			    }
			    foreach (var connectedDevice in device.ConnectedDevices)
			    {
				    if (connectedDevice == null)
				    {
					    devicesAPC.Add(device.name);
					    Logger.Log($"ConnectedDevice is null in \"{device.name}\"", Category.Tests);
					    report.AppendLine(device.name);
					    count++;
				    }
				    else
				    if (connectedDevice.RelatedAPC != device)
				    {
					    string obStr = connectedDevice.name;
					    devicesAPC.Add(obStr);
					    string apcStr = connectedDevice.RelatedAPC == null ? "null" : connectedDevice.RelatedAPC.name;
					    Logger.Log($"\"{obStr}\" RelatedAPC is \"{apcStr}\", supposed to be \"{device.name}\"", Category.Tests);
					    report.AppendLine(obStr);
					    count++;
				    }
			    }
		    }

		    Assert.That(count, Is.EqualTo(0), $"APCs in the scene: {listOfAPCs.Length}");
	    }

	    /// <summary>
	    /// Checks only scenes selected for build
	    /// </summary>
	    [Test]
	    public void CheckAllScenes_ForAPCPoweredDevices_WhichMissRelatedAPCs()
	    {
		    var buildScenes = EditorBuildSettings.scenes.Where(s => s.enabled);
		    var missingAPCinDeviceReport = new List<(string, string)>();
		    int countMissingAPC = 0;
		    int countSelfPowered = 0;
		    int countAll = 0;

		    foreach (var scene in buildScenes)
		    {
			    var currentScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
			    var currentSceneName = currentScene.name;

			    var listOfDevices =  GetAllPoweredDevicesInTheScene();
			    foreach (var objectDevice in listOfDevices)
			    {
				    countAll++;
				    var device = objectDevice;
				    if (device.IsSelfPowered)
				    {
					    countSelfPowered++;
					    continue;
				    }
				    if (device.RelatedAPC == null)
				    {
					    countMissingAPC++;
					    missingAPCinDeviceReport.Add((currentSceneName,objectDevice.name));
				    }
			    }
		    }

		    // Form report about missing components
		    var report = new StringBuilder();
		    foreach (var s in missingAPCinDeviceReport)
		    {
			    var missingComponentMsg = $"{s.Item1}: \"{s.Item2}\" miss APC reference.";
			    report.AppendLine(missingComponentMsg);
		    }

		    Logger.Log($"All devices count: {countAll}", Category.Tests);
		    Logger.Log($"Self powered Devices count: {countSelfPowered}", Category.Tests);
		    Logger.Log($"Devices count which miss APCs: {countMissingAPC}", Category.Tests);
		    Assert.IsEmpty(missingAPCinDeviceReport, report.ToString());
	    }

	    /// <summary>
	    /// Checks only scenes selected for build
	    /// </summary>
	    [Test]
	    public void CheckAllScenes_ForLightSources_WhichMissRelatedSwitches()
	    {
		    var buildScenes = EditorBuildSettings.scenes.Where(s => s.enabled);
		    var missingAPCinDeviceReport = new List<(string, string)>();
		    int countMissingSwitch = 0;
		    int countWithoutSwitches = 0;
		    int countAll = 0;

		    foreach (var scene in buildScenes)
		    {
			    var currentScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
			    var currentSceneName = currentScene.name;

			    var listOfDevices = GetAllLightSourcesInTheScene();
			    foreach (var objectDevice in listOfDevices)
			    {
				    countAll++;
				    var device = objectDevice;
				    if (device.IsWithoutSwitch)
				    {
					    countWithoutSwitches++;
					    continue;
				    }
				    if (device.relatedLightSwitch == null)
				    {
					    countMissingSwitch++;
					    missingAPCinDeviceReport.Add((currentSceneName,objectDevice.name));
				    }
			    }
		    }

		    // Form report about missing components
		    var report = new StringBuilder();
		    foreach (var s in missingAPCinDeviceReport)
		    {
			    var missingComponentMsg = $"{s.Item1}: \"{s.Item2}\" miss switch reference.";
			    report.AppendLine(missingComponentMsg);
		    }

		    Logger.Log($"All Light Sources count: {countAll}", Category.Tests);
		    Logger.Log($"Without switches Light Sources count: {countWithoutSwitches}", Category.Tests);
		    Logger.Log($"With missing switch reference: {countMissingSwitch}", Category.Tests);
		    Assert.IsEmpty(missingAPCinDeviceReport, report.ToString());
	    }

	    /// <summary>
	    /// Checks only scenes selected for build
	    /// </summary>
	    [Test]
	    public void CheckAllScenes_ForLightSwitchesLists_WhichMissLightSources()
	    {
		    var buildScenes = EditorBuildSettings.scenes.Where(s => s.enabled);
		    var missingAPCinDeviceReport = new List<(string, string)>();
		    int countSwitchesWithoutLights = 0;
		    int countAll = 0;

		    foreach (var scene in buildScenes)
		    {
			    var currentScene = EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
			    var currentSceneName = currentScene.name;

			    var listOfDevices = GetAllLightSwitchesInTheScene();
			    foreach (var objectDevice in listOfDevices)
			    {
				    countAll++;
				    var device = objectDevice;
				    if (device.listOfLights.Count == 0)
				    {
					    countSwitchesWithoutLights++;
					    missingAPCinDeviceReport.Add((currentSceneName,objectDevice.name));
				    }
			    }
		    }

		    // Form report about missing components
		    var report = new StringBuilder();
		    foreach (var s in missingAPCinDeviceReport)
		    {
			    var missingComponentMsg = $"{s.Item1}: \"{s.Item2}\" miss switch reference.";
			    report.AppendLine(missingComponentMsg);
		    }

		    Logger.Log($"All Light Switches count: {countAll}", Category.Tests);
		    Logger.Log($"Switches with empty lists of lights: {countSwitchesWithoutLights}", Category.Tests);
		    Assert.IsEmpty(missingAPCinDeviceReport, report.ToString());
	    }
    }
}
