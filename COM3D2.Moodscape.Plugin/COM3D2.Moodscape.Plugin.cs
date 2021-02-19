using ExIni;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace COM3D2.Moodscape.Plugin
{
    [PluginFilter("COM3D2x64"), PluginFilter("COM3D2VRx64"), PluginFilter("COM3D2OHx64"), PluginFilter("COM3D2OHVRx64"), PluginName("Moodscape"), PluginVersion("1.0.0.0")]
    public class Moodscape : PluginBase
    {
        public void Start()
        {
            try
            {
                IniKey enabled = Preferences["Config"]["Enabled"];
                IniKey global = Preferences["Config"]["GlobalEffect"];

                if (enabled.Value == "true")
                {
                    if (global.Value == "true")
                        StartCoroutine(LoopGlobal());
                    else
                        StartCoroutine(Loop());

                    Console.WriteLine("[Moodscape] Loaded correctly!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Moodscape] There was an error during the configuration loading, disabling the plugin. Error details: " + e.ToString());
            }
        }

        public IEnumerator Loop()
        {
            String prevbg = "";
            while (true)
            {
                String newbg = GetBackGroundString();
                if (prevbg != newbg && newbg != null)
                {
                    prevbg = newbg;
                    Console.WriteLine("[Moodscape] Background name: " + prevbg);
                    Console.WriteLine("[Moodscape] Loading background config...");

                    try
                    {
                        IniKey scenesettings = Preferences[prevbg]["Preset"];

                        AudioReverbPreset preset = GetPresetFromId(int.Parse(scenesettings.Value));

                        Console.WriteLine("[Moodscape] Loaded correctly! Applying audio effects...");

                        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "AudioVoice(Clone)").ToArray();

                        for (int i = 0; i < objects.Length; i++)
                        {
                            if (objects[i].GetComponent<AudioReverbFilter>() == null)
                            {
                                objects[i].AddComponent<AudioReverbFilter>();
                            }

                            objects[i].GetComponent<AudioReverbFilter>().reverbPreset = preset;
                        }

                        Console.WriteLine("[Moodscape] Loaded preset " + preset + ", Done!");
                    }
                    catch
                    {
                        GameObject[] objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "AudioVoice(Clone)").ToArray();

                        for (int i = 0; i < objects.Length; i++)
                        {
                            if (objects[i].GetComponent<AudioReverbFilter>() == null)
                            {
                                objects[i].AddComponent<AudioReverbFilter>();
                            }

                            objects[i].GetComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Off;
                        }

                        Console.WriteLine("[Moodscape] No configuration found for this scene");
                    }
                }

                yield return null;
            }
        }

        public IEnumerator LoopGlobal()
        {
            String prevbg = "";
            while (true)
            {
                String newbg = GetBackGroundString();
                if (prevbg != newbg || (prevbg != newbg && newbg == null))
                {
                    prevbg = newbg;
                    Console.WriteLine("[Moodscape] Background name: " + prevbg);
                    Console.WriteLine("[Moodscape] Loading background config...");

                    try
                    {
                        IniKey scenesettings = Preferences[prevbg]["Preset"];

                        AudioReverbPreset preset = GetPresetFromId(int.Parse(scenesettings.Value));

                        Console.WriteLine("[Moodscape] Loaded correctly! Applying audio effects...");

                        if (Camera.main.gameObject.GetComponent<AudioReverbFilter>() == null)
                            Camera.main.gameObject.AddComponent<AudioReverbFilter>();
                        Camera.main.gameObject.GetComponent<AudioReverbFilter>().reverbPreset = preset;

                        Console.WriteLine("[Moodscape] Loaded preset " + preset + ", Done!");
                    }
                    catch
                    {
                        if (Camera.main.gameObject.GetComponent<AudioReverbFilter>() == null)
                            Camera.main.gameObject.AddComponent<AudioReverbFilter>();
                        Camera.main.gameObject.GetComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Off;

                        Console.WriteLine("[Moodscape] No configuration found for this scene");
                    }
                }

                yield return null;
            }
        }

        String GetBackGroundString()
        {
            GameObject BGRoot = GameObject.Find("__GameMain__").transform.Find("BG").gameObject;

            if (BGRoot.transform.childCount == 0)
            {
                return null;
            }
            else
            {
                GameObject BG = null;
                for (int i = 0; i < BGRoot.transform.childCount; i++)
                {
                    if(!BGRoot.transform.GetChild(i).gameObject.name.Contains("FloorPlane"))
                    {
                        BG = BGRoot.transform.GetChild(i).gameObject;
                    }
                }
                return BG.name.Replace("(Clone)", "");
            } 
        }

        AudioReverbPreset GetPresetFromId(int i)
        {
            switch(i)
            {
                case 0:
                    return AudioReverbPreset.Off;
                case 1:
                    return AudioReverbPreset.Alley;
                case 2:
                    return AudioReverbPreset.Arena;
                case 3:
                    return AudioReverbPreset.Auditorium;
                case 4:
                    return AudioReverbPreset.Bathroom;
                case 5:
                    return AudioReverbPreset.CarpetedHallway;
                case 6:
                    return AudioReverbPreset.Cave;
                case 7:
                    return AudioReverbPreset.City;
                case 8:
                    return AudioReverbPreset.Concerthall;
                case 9:
                    return AudioReverbPreset.Dizzy;
                case 10:
                    return AudioReverbPreset.Drugged;
                case 11:
                    return AudioReverbPreset.Forest;
                case 12:
                    return AudioReverbPreset.Generic;
                case 13:
                    return AudioReverbPreset.Hallway;
                case 14:
                    return AudioReverbPreset.Hangar;
                case 15:
                    return AudioReverbPreset.Livingroom;
                case 16:
                    return AudioReverbPreset.Mountains;
                case 17:
                    return AudioReverbPreset.PaddedCell;
                case 18:
                    return AudioReverbPreset.ParkingLot;
                case 19:
                    return AudioReverbPreset.Plain;
                case 20:
                    return AudioReverbPreset.Psychotic;
                case 21:
                    return AudioReverbPreset.Quarry;
                case 22:
                    return AudioReverbPreset.Room;
                case 23:
                    return AudioReverbPreset.SewerPipe;
                case 24:
                    return AudioReverbPreset.StoneCorridor;
                case 25:
                    return AudioReverbPreset.Stoneroom;
                case 26:
                    return AudioReverbPreset.Underwater;
                default:
                    return AudioReverbPreset.Off;
            }
        }
    }
}
