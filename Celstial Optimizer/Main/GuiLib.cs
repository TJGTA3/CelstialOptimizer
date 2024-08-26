﻿using System;
using System.Linq;
using Celstial.Main;
using Celstial.Utils;
using UnityEngine;
namespace CelestialOptimizer
{

    public class GuiLib : MonoBehaviour
    {
        public static bool isSectionOpen;

        public static void Begin()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        }

        public static void End()
        {
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            isSectionOpen = false;
        }

        public static void Separate()
        {
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            isSectionOpen = false;
        }

        public static void NewSection(string text)
        {
            if (isSectionOpen)
            {
                GUILayout.EndVertical();
                isSectionOpen = false;
            }

            isSectionOpen = true;

            GUILayout.BeginVertical(new GUIContent(), "Box", GUILayout.Width(Menu.mainMenu.width / 2.15f - 12));
            GUILayout.BeginHorizontal();
            var labelStyle = new GUIStyle("Label") { fontSize = 12, alignment = TextAnchor.MiddleCenter };
            if (labelStyle.normal.textColor != Util.GetColorFromString(Config.Get("SectionTextColor").ToString()))
            {
                labelStyle.normal.textColor = Util.GetColorFromString(Config.Get("SectionTextColor").ToString());
            }

            var textSize = labelStyle.CalcSize(new GUIContent(text)).x;
            var dashSize = labelStyle.CalcSize(new GUIContent(" ")).x;
            var sectionSize = Menu.mainMenu.width / 2 - 12;

            var spaceMulti = (int)((
                        sectionSize - textSize - dashSize * 4
                    ) / dashSize / 2
                );

            var seps = string.Concat(Enumerable.Repeat(" ", spaceMulti));

            GUILayout.Label($"<b>{seps} {text} {seps}</b>", labelStyle);
            GUILayout.EndHorizontal();
        }

        public static void SectionLabel(string text, string separator = " ")
        {
            var labelStyle = new GUIStyle("box") { fontSize = 12, alignment = TextAnchor.MiddleCenter };
            if (labelStyle.normal.textColor != Util.GetColorFromString(Config.Get("SectionTextColor").ToString()))
            {
                labelStyle.normal.textColor = Util.GetColorFromString(Config.Get("SectionTextColor").ToString());
            }


            var textSize = labelStyle.CalcSize(new GUIContent(text)).x;
            var dashSize = labelStyle.CalcSize(new GUIContent(separator)).x;
            var sectionSize = (Menu.mainMenu.width / 2) - 12;

            var spaceMulti = (int)((
                        (sectionSize - textSize) - (dashSize * 4)
                    ) / dashSize / 2
                );

            var seps = string.Concat(Enumerable.Repeat(separator, spaceMulti));

            GUILayout.Label($"<b>{seps} {text} {seps}</b>", labelStyle);
        }


        public static void NewButton(string text, Action callback)
        {

            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            // make button and execute callback if pressed
            if (GUILayout.Button(text)) callback();
            GUI.backgroundColor = Menu.baseColor;
        }

        public static int tabSystem(int min, int max, int value, int direction)
        {
            if (value < min) return max;
            if (value > max) return min;

            var newValue = value + direction;

            if (newValue < min) return max;
            if (newValue > max) return min;
            return newValue;
        }

        public static int IntSystem(string text, int num, int min, int max, int dir)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            var newNum = 0;
            if (GUILayout.Button(text, new GUILayoutOption[0])) newNum = tabSystem(min, max, num, dir);
            GUI.backgroundColor = Menu.baseColor;
            return newNum;
        }

        public static void IntSystem(string text, ref int num, int min, int max, int dir)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            if (GUILayout.Button(text, new GUILayoutOption[0])) num = tabSystem(min, max, num, dir);
            GUI.backgroundColor = Menu.baseColor;
        }

        public static void DoubIntSystem(string text, ref int num, ref int num2, int min, int max, int dir,
            int min1,
            int max1, int dir1)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            var toggleStyle = new GUIStyle("Box") { fontSize = 12 };
            if (GUILayout.Button(text, new GUILayoutOption[0]))
            {
                num = tabSystem(min, max, num, dir);
                num2 = tabSystem(min1, max1, num2, dir1);
            }

            GUI.backgroundColor = Menu.baseColor;
        }

        public static bool NewToggle(bool value, string text)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            // set toggle style
            var toggleStyle = new GUIStyle("Box") { fontSize = 12 };
            if (value)
                toggleStyle.normal.textColor = Util.GetColorFromString("a6da95");
            else
                toggleStyle.normal.textColor = Util.GetColorFromString("ed8796");

            // make toggle and return new value
            return GUILayout.Toggle(value, text, toggleStyle);
            GUI.color = Color.white;
        }


        public static string newText(string text, string value)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            var labelStyle = new GUIStyle("Box") { fontSize = 12 };
            float width = (Menu.mainMenu.width / 2) / 2 - 16f;
            GUILayout.BeginHorizontal();
            GUILayout.Label(text, labelStyle, new GUILayoutOption[] { GUILayout.Width(width) });
            string txt = GUILayout.TextField(value, labelStyle, new GUILayoutOption[] { GUILayout.Width(width) });
            GUILayout.EndHorizontal();
            // create slider and return new value

            return txt;
        }

        public static float NewSlider(string text, float value, float minimum, float maximum, string suffix)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            // slider title and title style
            var labelStyle = new GUIStyle("Box") { fontSize = 12 };
            GUILayout.Label($"{text}: {value}{suffix}", labelStyle);

            // create slider and return new value
            return GUILayout.HorizontalSlider(value, minimum, maximum);
        }

        public static float NewSlider(string text, float value, float minimum, float maximum, string suffix,
            int digits)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            // slider title and title style
            var labelStyle = new GUIStyle("Box") { fontSize = 12 };
            GUILayout.Label($"{text}: {Math.Round(value, digits)}{suffix}", labelStyle);

            // create slider and return new value
            return GUILayout.HorizontalSlider(value, minimum, maximum);
        }

        public static float NewSlider(string text, float value, float minimum, float maximum, string suffix,
            int digits,
            int multiply)
        {
            if (GUI.backgroundColor != Util.GetColorFromString(Config.Get("GuiComponentColor").ToString()))
            {
                GUI.backgroundColor = Util.GetColorFromString(Config.Get("GuiComponentColor").ToString());
            }

            // slider title and title style
            var labelStyle = new GUIStyle("Box") { fontSize = 12 };
            GUILayout.Label($"{text}: {Math.Round(value, digits) * multiply}{suffix}", labelStyle);

            // create slider and return new value
            return GUILayout.HorizontalSlider(value, minimum, maximum);
        }

    }
}
