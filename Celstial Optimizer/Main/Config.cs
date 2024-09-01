using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Celstial.Main
{
    public class Config
    {
        public static string filepath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\celstialconfig.json";
        public static Dictionary<string, object> Configuration = new Dictionary<string, object>()
        {
            // Comp Settings Etc
            ["HideKeybinds"] = false,
            ["SkyColor"] = "000000",
            ["SkyColor1"] = "000000",
            ["SkyColor2"] = "8a2be2",
            ["SkyColor3"] = "87cefa",
            ["DisableHud"] = false,
            ["TabStringColor"] = "cad3f5",
            ["TabButtonColor"] = "24273a",
            ["TabSelectedButtonColor"] = "8087a2",
            ["GuiComponentColor"] = "8087a2",
            ["WindowBackgroundColor"] = "6e738d",
            ["SectionTextColor"] = "cad3f5",
            ["WindowStringColor"] = "b7bdf8",
            ["WatermarkStringColor"] = "cad3f5",
            ["Ambient"] = false,
            ["AmbientMode"] = 1,
            ["LerpSpeed"] = .75f,
            ["AmbientRainbowSpeed"] = 1f,
            ["Stats"] = false,
            ["Crosshair"] = false,
            ["DisableBuilds"] = true,
            ["DisableSlots"] = true,
            ["DisableHealth"] = true,
            ["AestheticKeys"] = true,
            ["FPSCap"] = 120f,
            ["FPSCapper"] = false,
            ["SeeUsers"] = true,
            ["DisableParticles"] = false,
            ["CustomCode"] = "00000",
            
        };
        public static void LoadConfig(string filePath)
        {
            
            if (File.Exists(filePath))
            {
                try
                {
                    string jsonContent = File.ReadAllText(filePath);
                    Dictionary<string, object> loadedConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonContent);

                    foreach (var key in loadedConfig.Keys)
                    {
                        if (Configuration.ContainsKey(key))
                        {
                            Configuration[key] = Convert.ChangeType(loadedConfig[key], Configuration[key].GetType());
                        }else { }
                    }

                }catch (Exception e) { }
            }else { }
            
        }
        public static void SaveConfig(string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Configuration);
                File.WriteAllText(filePath, json);
            }catch (IOException ex) { }
        }
        public static object Get(string cast)
        {
            return Configuration[cast];
        }
        public static void Set(string key, object value)
        {
            Configuration[key] = value;
        }
    }
}