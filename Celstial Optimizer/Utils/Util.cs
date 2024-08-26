using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Celstial.Utils
{
    public class Util
    {
        public static int HexToDec(string Hex)
        {
            return Convert.ToInt32(Hex, 16);
        }
        
        public static float HexToFloatNormalized(string Hex)
        {
            return HexToDec(Hex) / 255f;
        }

        public static Color GetColorFromString(string HexCode)
        {
            if (HexCode.Contains("#")) HexCode = HexCode.Replace("#", "");

            if (HexCode.Length == 3 || HexCode.Length == 4) HexCode = ExpandShortHexCode(HexCode);
            else if (HexCode.Length > 8) HexCode = HexCode.Substring(0, 8);
            else if (HexCode.Length > 6 && HexCode.Length < 8) HexCode = HexCode.Substring(0, 6);

            if (!IsValidHexColor(HexCode))
            {
                if (HexCode.Length == 7)
                    HexCode += "FF"; // Extend into transparency
                else
                    HexCode = ExpandShortHexCode(HexCode);
            }

            if (HexCode.Length == 6)
            {
                var num = HexToFloatNormalized(HexCode.Substring(0, 2));
                var num2 = HexToFloatNormalized(HexCode.Substring(2, 2));
                var num3 = HexToFloatNormalized(HexCode.Substring(4, 2));
                return new Color(num, num2, num3);
            }

            if (HexCode.Length == 8)
            {
                var num = HexToFloatNormalized(HexCode.Substring(0, 2));
                var num2 = HexToFloatNormalized(HexCode.Substring(2, 2));
                var num3 = HexToFloatNormalized(HexCode.Substring(4, 2));
                var num4 = HexToFloatNormalized(HexCode.Substring(6, 2));
                return new Color(num, num2, num3, num4);
            }

            return new Color();
        }

        private static bool IsValidHexColor(string hexCode)
        {
            return Regex.IsMatch(hexCode, "^[0-9a-fA-F]{3}$|^[0-9a-fA-F]{4}$|^[0-9a-fA-F]{6}$|^[0-9a-fA-F]{8}$");
            //return true;
        }
        
        private static string ExpandShortHexCode(string shortHexCode)
        {
            if (shortHexCode.Length == 3)
                return string.Format("{0}{0}{1}{1}{2}{2}", shortHexCode[0], shortHexCode[1], shortHexCode[2]);
            if (shortHexCode.Length == 4)
                return string.Format("{0}{1}{2}{3}", shortHexCode[0], shortHexCode[1], shortHexCode[2], shortHexCode[3]);

            return shortHexCode;
        }
    }
}