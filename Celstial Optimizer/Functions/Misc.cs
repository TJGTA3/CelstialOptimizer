using System.Reflection;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using Celstial.Main;
using Photon.Pun;
using Assets.Scripts.Network;
using Photon.Realtime;
using Celstial.Utils;
using ExitGames.Client.Photon.StructWrapping;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Celstial.Functions 
{
    public class Misc : MonoBehaviour
    {

        public static List<string> COUsers = new List<string>();
        public static float hue;
        public static float sat = .35f;
        public static float bri = 1f;

        public static List<string> Builds = new List<string>()
        {
            "Wall",
            "Floor",
            "Ramp",
            "Pyramid",
            "Roof"
        };
        

        private float hudTimer = 0f;
        private float photontimer = 0f; // Without this your lobby bugs out big time
        
        public void OnGUI()
        {
           
            if ((bool)Config.Get("Ambient") && GameVars.LocalPlayer() != null)
            {
                Ambient();
            }

            if ((bool)Config.Get("Crosshair"))
            {
                Crosshair();
            }
        }

        

        void Crosshair()
        {
            GUI.DrawTexture(new Rect(Render.CenterOfScreen().x - 15f, Render.CenterOfScreen().y, 30f, 2f), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(Render.CenterOfScreen().x, Render.CenterOfScreen().y - 15f, 2f, 30f), Texture2D.whiteTexture);
        }

       
        
        
        public void Update()
        {
            if ((bool)Config.Get("DisableAllAudio"))
            {
                foreach (var audio in Entity.audios)
                {
                    audio.enabled = false;
                    audio.gameObject.SetActive(false);
                }
            }
            if ((bool)Config.Get("DisableParticles"))
            {
                foreach (var ps in Entity.ParticleSystems)
                {
                    if (ps.isPlaying)
                    {
                        ps.Stop();
                    }
                    
                }
            }
            if (PhotonNetwork.InRoom && SceneManager.GetActiveScene().name == "MainMenu")
            {
                photontimer += Time.deltaTime;
                if (photontimer >= 1.75f)
                {
                    foreach (var p in PhotonNetwork.PlayerList)
                    {
                        if (p.CustomProperties.ToString().Contains("COUser") && !COUsers.Contains(p.NickName))
                        {
                            COUsers.Add(p.NickName);
                        }
                    }

                    foreach (var tmp in Entity.tmpu)
                    {
                        if (COUsers.Contains(tmp.text) && !tmp.text.Contains("[CO]"))
                        {
                            tmp.text = "[CO] " + tmp.text;
                        }
                    }

                    photontimer = 0f;
                }
            }
            // uwu
            if (SceneManager.GetActiveScene().name != "MainMenu" &&
                SceneManager.GetActiveScene().name != "MoveScene")
            {
                
                hudTimer += Time.deltaTime;
                if (hudTimer >= 1.75f)
                {
                    

                    foreach (var uie in Entity.uii)
                    {
                        if (uie != null)
                        {
                            if (uie.name != null)
                            {

                                // Disable Entire HUD
                                if ((bool)Config.Get("DisableHud"))
                                {
                                    if (uie.enabled)
                                    {
                                        uie.enabled = false;
                                    }

                                    uie.gameObject.SetActive(false);
                                }

                                if ((bool)Config.Get("AestheticKeys"))
                                {
                                    if (uie.name == "Bg")
                                    {
                                        if (uie.enabled)
                                        {
                                            uie.enabled = false;
                                        }

                                        uie.gameObject.SetActive(false);
                                    }
                                }


                                if (Builds.Contains(uie.name))
                                {
                                    // Disable Builds
                                    if ((bool)Config.Get("DisableBuilds"))
                                    {
                                        if (uie.enabled)
                                        {
                                            uie.enabled = false;
                                        }

                                        uie.gameObject.SetActive(false);
                                    }
                                }

                                if (uie.name.Contains("Slot") || uie.name == "Container")
                                {
                                    if ((bool)Config.Get("DisableSlots"))
                                    {
                                        if (uie.enabled)
                                        {
                                            uie.enabled = false;
                                        }

                                        uie.gameObject.SetActive(false);
                                    }
                                }

                                if (uie.name == "Health" || uie.name == "Armor" || uie.name == "DecreaseEffect" ||
                                    uie.name == "darkener" || uie.name == "Background" || uie.name == "HealthGlow")
                                {
                                    // Disable Builds
                                    if ((bool)Config.Get("DisableHealth"))
                                    {
                                        if (uie.enabled)
                                        {
                                            uie.enabled = false;
                                        }


                                        uie.gameObject.SetActive(false);
                                    }
                                }
                            }
                        }


                    }

                    hudTimer = 0f;
                }
            }
        }
            
            
            
            
        
    


       
        
        public static void Ambient()
        {
            if (SceneManager.GetActiveScene().name != "MenuMenu" && SceneManager.GetActiveScene().name != "MoveScene")
            {
                foreach (Camera camera in Entity.Cameras)
                {
                    switch ((int)Config.Get("AmbientMode"))
                    {
                        case 0:
                            if (camera.backgroundColor != Util.GetColorFromString(Config.Get("SkyColor1").ToString()))
                            {
                                camera.clearFlags = CameraClearFlags.Color;
                                camera.backgroundColor = Util.GetColorFromString(Config.Get("SkyColor1").ToString());
                            }

                            break;
                        case 1:
                            camera.clearFlags = CameraClearFlags.Color;
                            camera.backgroundColor = Color.Lerp(Util.GetColorFromString(Config.Get("SkyColor2").ToString()), Util.GetColorFromString(Config.Get("SkyColor3").ToString()), Mathf.PingPong(Time.time, (float)Config.Get("LerpSpeed")));
                            break;
                        case 2:
                            Menu.AmbientHue+= 0.0007f;
                            if (Menu.AmbientHue >= 1f)
                            {
                                Menu.AmbientHue = 0f;
                            }
                            camera.clearFlags = CameraClearFlags.Color;
                            Color color = Color.HSVToRGB(Menu.AmbientHue, sat, bri);
                            camera.backgroundColor = color;
                            break;
                            
                    }
                }
            }
        }
    }
}