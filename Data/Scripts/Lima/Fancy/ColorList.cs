using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy
{
  internal static class ColorsList
  {
    public static List<Color> colors = new List<Color> { Color.MediumSeaGreen, Color.Magenta, Color.Maroon, Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid, Color.MediumPurple, Color.MediumSlateBlue, Color.MintCream, Color.MediumTurquoise, Color.MediumVioletRed, Color.MidnightBlue, Color.Linen, Color.MistyRose, Color.Moccasin, Color.NavajoWhite, Color.Navy, Color.MediumSpringGreen, Color.LimeGreen, Color.LightSkyBlue, Color.LightYellow, Color.Ivory, Color.Khaki, Color.Lavender, Color.LavenderBlush, Color.LawnGreen, Color.LemonChiffon, Color.LightBlue, Color.LightCoral, Color.Lime, Color.LightCyan, Color.LightGreen, Color.LightGray, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.OldLace, Color.LightSlateGray, Color.LightSteelBlue, Color.LightGoldenrodYellow, Color.Olive, Color.PaleGoldenrod, Color.Orange, Color.Silver, Color.SkyBlue, Color.SlateBlue, Color.SlateGray, Color.Snow, Color.SpringGreen, Color.SteelBlue, Color.Tan, Color.Sienna, Color.Teal, Color.Tomato, Color.Turquoise, Color.Violet, Color.Wheat, Color.White, Color.WhiteSmoke, Color.Yellow, Color.YellowGreen, Color.Thistle, Color.OliveDrab, Color.SeaShell, Color.SandyBrown, Color.OrangeRed, Color.Orchid, Color.Indigo, Color.PaleGreen, Color.PaleTurquoise, Color.PaleVioletRed, Color.PapayaWhip, Color.PeachPuff, Color.SeaGreen, Color.Peru, Color.Plum, Color.PowderBlue, Color.Red, Color.RosyBrown, Color.RoyalBlue, Color.SaddleBrown, Color.Salmon, Color.Pink, Color.IndianRed, Color.HotPink, Color.Honeydew, Color.DarkGoldenrod, Color.DarkCyan, Color.DarkBlue, Color.Cyan, Color.Crimson, Color.Cornsilk, Color.CornflowerBlue, Color.Coral, Color.Chocolate, Color.Chartreuse, Color.CadetBlue, Color.DarkGray, Color.BurlyWood, Color.BlueViolet, Color.Blue, Color.BlanchedAlmond, Color.Bisque, Color.Beige, Color.Azure, Color.Aquamarine, Color.Aqua, Color.AntiqueWhite, Color.AliceBlue, Color.Brown, Color.DarkGreen, Color.DarkMagenta, Color.DarkKhaki, Color.GreenYellow, Color.Green, Color.Gray, Color.Goldenrod, Color.Gold, Color.GhostWhite, Color.Gainsboro, Color.Fuchsia, Color.ForestGreen, Color.FloralWhite, Color.Firebrick, Color.DodgerBlue, Color.DeepSkyBlue, Color.DeepPink, Color.DarkViolet, Color.DarkTurquoise, Color.DarkSlateGray, Color.DarkSlateBlue, Color.DarkSeaGreen, Color.DarkSalmon, Color.DarkRed, Color.DarkOrchid, Color.DarkOrange, Color.DarkOliveGreen, Color.DimGray };
    public static List<String> names = new List<String> { "MediumSeaGreen", "Magenta", "Maroon", "MediumAquamarine", "MediumBlue", "MediumOrchid", "MediumPurple", "MediumSlateBlue", "MintCream", "MediumTurquoise", "MediumVioletRed", "MidnightBlue", "Linen", "MistyRose", "Moccasin", "NavajoWhite", "Navy", "MediumSpringGreen", "LimeGreen", "LightSkyBlue", "LightYellow", "Ivory", "Khaki", "Lavender", "LavenderBlush", "LawnGreen", "LemonChiffon", "LightBlue", "LightCoral", "Lime", "LightCyan", "LightGreen", "LightGray", "LightPink", "LightSalmon", "LightSeaGreen", "OldLace", "LightSlateGray", "LightSteelBlue", "LightGoldenrodYellow", "Olive", "PaleGoldenrod", "Orange", "Silver", "SkyBlue", "SlateBlue", "SlateGray", "Snow", "SpringGreen", "SteelBlue", "Tan", "Sienna", "Teal", "Tomato", "Turquoise", "Violet", "Wheat", "White", "WhiteSmoke", "Yellow", "YellowGreen", "Thistle", "OliveDrab", "SeaShell", "SandyBrown", "OrangeRed", "Orchid", "Indigo", "PaleGreen", "PaleTurquoise", "PaleVioletRed", "PapayaWhip", "PeachPuff", "SeaGreen", "Peru", "Plum", "PowderBlue", "Red", "RosyBrown", "RoyalBlue", "SaddleBrown", "Salmon", "Pink", "IndianRed", "HotPink", "Honeydew", "DarkGoldenrod", "DarkCyan", "DarkBlue", "Cyan", "Crimson", "Cornsilk", "CornflowerBlue", "Coral", "Chocolate", "Chartreuse", "CadetBlue", "DarkGray", "BurlyWood", "BlueViolet", "Blue", "BlanchedAlmond", "Bisque", "Beige", "Azure", "Aquamarine", "Aqua", "AntiqueWhite", "AliceBlue", "Brown", "DarkGreen", "DarkMagenta", "DarkKhaki", "GreenYellow", "Green", "Gray", "Goldenrod", "Gold", "GhostWhite", "Gainsboro", "Fuchsia", "ForestGreen", "FloralWhite", "Firebrick", "DodgerBlue", "DeepSkyBlue", "DeepPink", "DarkViolet", "DarkTurquoise", "DarkSlateGray", "DarkSlateBlue", "DarkSeaGreen", "DarkSalmon", "DarkRed", "DarkOrchid", "DarkOrange", "DarkOliveGreen", "DimGray" };
    public static List<String> fonts = new List<String> { "Debug", "BuildInfoHighlight", "BuildInfo", "LoadingScreen", "GameCredits", "ScreenCaption", "InfoMessageBoxText", "ErrorMessageBoxText", "InfoMessageBoxCaption", "UrlHighlight", "UrlNormal", "DarkBlue", "White", "Blue", "Green", "Red", "ErrorMessageBoxCaption" };
  }
}