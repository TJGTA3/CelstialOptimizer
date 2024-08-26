using UnityEngine;

namespace Celstial.Utils
{
    public class Render
    {
        private static GUIStyle style = new GUIStyle();
        private static GUIStyle style1 = new GUIStyle();
        
        private static Texture2D drawingTex;
        private static Color lastTexColour;
        public static Vector2 CenterOfScreen() => new Vector2(Screen.width / 2f, Screen.height / 2f);
        public static void DrawText(Vector2 ScreenPos, string text, Color outLineColor = new Color(), bool center = true, int fontSize = 12, FontStyle fontStyle = FontStyle.Bold, int type = 0)
        {
            style.fontSize = fontSize;
            style.richText = true;
            style.fontStyle = fontStyle;
            style1.fontSize = fontSize;
            style1.richText = true;
            style1.normal.textColor = outLineColor;
            style1.fontStyle = fontStyle;
            GUIContent guicontent = new GUIContent(text);
            GUIContent guicontent2 = new GUIContent(text);
            if (center)
            {
                ScreenPos.x -= style.CalcSize(guicontent).x / 2f;
            }
            switch (type)
            {
                case 0:
                    GUI.Label(new Rect(ScreenPos.x, ScreenPos.y, 300f, 25f), guicontent, style);
                    return;
                case 1:
                    GUI.Label(new Rect(ScreenPos.x + 1f, ScreenPos.y + 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x, ScreenPos.y, 300f, 25f), guicontent, style);
                    return;
                case 2:
                    GUI.Label(new Rect(ScreenPos.x + 1f, ScreenPos.y + 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x - 1f, ScreenPos.y - 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x, ScreenPos.y, 300f, 25f), guicontent, style);
                    return;
                case 3:
                    GUI.Label(new Rect(ScreenPos.x + 1f, ScreenPos.y + 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x - 1f, ScreenPos.y - 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x, ScreenPos.y - 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x, ScreenPos.y + 1f, 300f, 25f), guicontent2, style1);
                    GUI.Label(new Rect(ScreenPos.x, ScreenPos.y, 300f, 25f), guicontent, style);
                    return;
                default:
                    return;
            }
        }
        
    }
}