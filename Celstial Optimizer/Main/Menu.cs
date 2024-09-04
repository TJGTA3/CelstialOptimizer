using System.Linq;
using CelestialOptimizer;
using Celstial.Utils;
using Photon.Pun;
using UnityEngine;
using Assets.Scripts.Network;
using Photon.Pun;
using Photon.Realtime;

namespace Celstial.Main
{
    public class Menu : MonoBehaviour
    {
        public static int selectedTab;
        public static bool open = true;
        public static float AmbientHue;
        public static Color baseColor;
        public static Vector2 scrollPos;
        public static Rect mainMenu = new Rect((float)(Screen.width / 2.64), 130f, 431f, 50f);
        public static string[] AmbientNames =
        {
            "Single",
            "Lerp",
            "RGB"
        };
        
        private void Awake()
        {
            Config.LoadConfig(Config.filepath);
            baseColor = GUI.backgroundColor;
            open = true;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                open = !open;
            }
        }
        
        
        public void OnGUI()
        {
            if (GUI.backgroundColor != Util.GetColorFromString((Config.Get("WindowBackgroundColor").ToString()))) GUI.backgroundColor = Util.GetColorFromString((Config.Get("WindowBackgroundColor").ToString()));

            if (open)
            {
                mainMenu = GUILayout.Window(1, mainMenu, new GUI.WindowFunction(NewMenu), $"<b><color=#{Config.Get("WindowStringColor")}>Celestial Optimizer</color></b>", new GUILayoutOption[0]);
            }
            if ((bool)Config.Get("Stats"))
            {
                GUILayout.Label($"<b><size=24><color=#cad3f5>FPS: {(int)(1f / Time.unscaledDeltaTime)}</color></size></b>");
                GUILayout.Label($"<b><size=24><color=#cad3f5>Ping: {PhotonNetwork.GetPing()}ms</color></size></b>");
            }
        }
        
        public static void TabButton(string tabName, int newTab, float width, float height)
        {
            if (selectedTab != newTab)
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("TabButtonColor").ToString());
                if (GUILayout.Button($"<b><color=#{Config.Get("TabStringColor")}>{tabName}</color></b>",
                        GUILayout.Width(width), GUILayout.Height(height)))
                    selectedTab = newTab;
                GUI.backgroundColor = baseColor;
            }

            if (selectedTab == newTab)
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("TabSelectedButtonColor").ToString());
                if (GUILayout.Button($"<b><color=#{Config.Get("TabStringColor")}>{tabName}</color></b>",
                        GUILayout.Width(width), GUILayout.Height(height)))
                    selectedTab = newTab;

                GUI.backgroundColor = baseColor;
            }
        }

        void NewMenu(int wID)
        {
            GUILayout.BeginHorizontal();
            TabButton("Cosmetic", 0, mainMenu.width * .45f, 35f);
            TabButton("Optimizations", 1, mainMenu.width * .45f, 35f);
            GUILayout.EndHorizontal();
            scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar,
                new GUIStyle(), GUILayout.Height(515f));

            switch (selectedTab)
            {
                case 0:
                    Cosmetic();
                    break;
                case 1:
                    Optimize();
                    break;
                
            }

            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
        
        public static void Optimize()
        {
            GuiLib.Begin();


            GuiLib.NewSection("Ambient");
            Config.Set("Ambient", GuiLib.NewToggle((bool)Config.Get("Ambient"), "Enabled"));

            GuiLib.NewButton("Mode: " + AmbientNames[int.Parse(Config.Get("AmbientMode").ToString())], () => Config.Set("AmbientMode", GuiLib.tabSystem(0, 2, int.Parse(Config.Get("AmbientMode").ToString()), 1)));
            switch (int.Parse(Config.Get("AmbientMode").ToString()))
            {
                case 0:
                    Config.Set("SkyColor1", GuiLib.newText("Single Color", Config.Get("SkyColor1").ToString()));
                    break;
                case 1:
                    Config.Set("LerpSpeed", GuiLib.NewSlider("Lerp Speed", (float)Config.Get("LerpSpeed"), .25f, 2f, "s", 2));
                    Config.Set("SkyColor2", GuiLib.newText("Lerp Color 1", Config.Get("SkyColor2").ToString()));
                    Config.Set("SkyColor3", GuiLib.newText("Lerp Color 2", Config.Get("SkyColor3").ToString()));
                    break;
                case 2:
                    Config.Set("AmbientRainbowSpeed", GuiLib.NewSlider("RGB Speed", (float)Config.Get("AmbientRainbowSpeed"), .01f, 1f, "%", 2));
                    break;

            }
            GuiLib.NewSection("HUD");
            Config.Set("Crosshair", GuiLib.NewToggle((bool)Config.Get("Crosshair"), "Crosshair"));
            Config.Set("Stats", GuiLib.NewToggle((bool)Config.Get("Stats"), "Stats"));
            
            Config.Set("DisableHud", GuiLib.NewToggle((bool)Config.Get("DisableHud"), "Disable Entire HUD (Unstable)"));
            Config.Set("DisableBuilds", GuiLib.NewToggle((bool)Config.Get("DisableBuilds"), "Disable Builds"));
            Config.Set("DisableSlots", GuiLib.NewToggle((bool)Config.Get("DisableSlots"), "Disable Weapon & Ability Slots"));
            Config.Set("DisableHealth", GuiLib.NewToggle((bool)Config.Get("DisableHealth"), "Disable Health & Shield Bars"));
            Config.Set("AestheticKeys", GuiLib.NewToggle((bool)Config.Get("AestheticKeys"), "Aesthetic Keys"));
            
            GuiLib.NewSection("Other Optimizations");
            Config.Set("DisableParticles", GuiLib.NewToggle((bool)Config.Get("DisableParticles"), "Disable Skin Particles"));
            Config.Set("DisableAllAudio", GuiLib.NewToggle((bool)Config.Get("DisableAllAudio"), "Disable All Audio"));
            
            GuiLib.NewSection("FPS Capper");
            Config.Set("FPSCapper", GuiLib.NewToggle((bool)Config.Get("FPSCapper"), "FPS Capper"));
            Config.Set("FPSCap", GuiLib.NewSlider("FPS Cap", (float)Config.Get("FPSCap"), 120, 1000, " FPS"));
            
            GuiLib.NewSection("Customization");
            Config.Set("SeeUsers", GuiLib.NewToggle((bool)Config.Get("SeeUsers"), "Show Yourself As Using CO"));
            
            GuiLib.NewSection("Custom Code Creator");
            Config.Set("CustomCode", GuiLib.newText("Code: ", Config.Get("CustomCode").ToString()));
            GuiLib.NewButton("Create Party", CreateCustomParty);
            
            
            
            GuiLib.End();
        }

        static void CreateCustomParty()
        {
            
            if (int.Parse(Config.Get("CustomCode").ToString()) >= 0 &&  int.Parse(Config.Get("CustomCode").ToString()) <= 33332 && RegionManager.Instance.ANOBPOLIFFK != "us")
            {
                RegionManager.Instance.ChangeRegion("us");
            }

            if (int.Parse(Config.Get("CustomCode").ToString()) >= 33333 && int.Parse(Config.Get("CustomCode").ToString()) <= 66665 && RegionManager.Instance.ANOBPOLIFFK != "eu")
            {
                RegionManager.Instance.ChangeRegion("eu");
            }
            
            if (int.Parse(Config.Get("CustomCode").ToString()) >= 66666 && int.Parse(Config.Get("CustomCode").ToString()) <= 99999 && RegionManager.Instance.ANOBPOLIFFK != "asia")
            {
                Debug.Log(RegionManager.Instance.ANOBPOLIFFK);
                RegionManager.Instance.ChangeRegion("asia");
            }
            

            Entity.prc.enabled = true;
            PhotonNetwork.CreateRoom(Config.Get("CustomCode").ToString(), new RoomOptions
            {
                MaxPlayers = (int)100,
                PublishUserId = true,
                IsVisible = false,
                EmptyRoomTtl = 15,
            }, TypedLobby.Default, null);
        }
        void AddSkin(string champ)
        {
            if (FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM != null)
            {
                var skins = FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM.Skins;
                if (!skins.CharacterSkins.Contains(champ)) skins.CharacterSkins.Add(champ);
            }
        }
        void UnlockAllChampSkins()
        {
            if (FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM != null)
            {
                foreach (var champ in GameVars.ChampionSkins)
                {
                    AddSkin(champ);
                }
            }
        }
        void AddWeaponSkin(string skin)
        {
            if (FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM == null) return;
            var skins = FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM.Skins;
            string weaponType = string.Join(".", skin.Split('.').Take(4));

            skins.EquippedWeaponSkins.RemoveAll(s => s.ToLower().StartsWith(weaponType.ToLower()) && s.ToLower() != skin.ToLower());
            skins.WeaponSkins.RemoveAll(s => s.ToLower().StartsWith(weaponType.ToLower()) && s.ToLower() != skin.ToLower());

            if (!skins.EquippedWeaponSkins.Contains(skin))
            {
                skins.WeaponSkins.Add(skin);
                skins.EquippedWeaponSkins.Add(skin);
            }
        }
        void UnlockAll()
        {
            var serverUser = FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM;
            if (serverUser != null)
            {
                var skins = serverUser.Skins;
                var text = "lol.1v1.playeremotes.pack.";
                var text2 = "lol.1v1.playerstickers.pack.";
                for (var i = 1; i <= 50; i++) skins.OwnedEmotes.Add(string.Format("{0}{1}", text, i));
                for (var j = 1; j <= 80; j++) skins.OwnedEmotes.Add(string.Format("{0}{1}", text2, j));
            }

            
            
        }

        void UnlockChamps()
        {
            var serverUser = FirebaseManager.OEPCIBFBPLE.MJPJKOMCLOM;
            string[] champs = new []
            {
                "lol.1v1.champions.violet",
                "lol.1v1.champions.magma",
                "lol.1v1.champions.caesar",
                "lol.1v1.champions.frosty",
                "lol.1v1.champions.shadow",
                "lol.1v1.champions.jade",
                "lol.1v1.champions.tron",
                "lol.1v1.champions.sentinel",
                "lol.1v1.champions.fang",
                "lol.1v1.champions.poseidon"
            };

            foreach (var champ in champs)
            {
                if (!serverUser.Champions.OwnedChampions.ContainsKey(champ))
                {
                    serverUser.Champions.OwnedChampions.Add(champ, new UserChampionData());
                }
            }
        }
        void Cosmetic()
        {
            GuiLib.Begin();
            GuiLib.NewSection("Champions");
            GuiLib.NewButton("Unlock All Champions", UnlockChamps);
            GuiLib.NewSection("Skins");
            GuiLib.NewButton("Unlock All Emotes", UnlockAll);
            GuiLib.NewButton("Unlock All Champion Skins", UnlockAllChampSkins);
            foreach (var weapon in GameVars.WeaponSkins)
            {
                if (GUILayout.Button("Equip: " + weapon.Value))
                {
                    AddWeaponSkin(weapon.Key);
                }
            }
        
            
            GuiLib.End();
        }
        
        
    }
}