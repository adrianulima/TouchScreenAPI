using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.GameSystems.TextSurfaceScripts;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyTheme
  {
    public static List<Color> colors = new List<Color> { Color.MediumSeaGreen, Color.Magenta, Color.Maroon, Color.MediumAquamarine, Color.MediumBlue, Color.MediumOrchid, Color.MediumPurple, Color.MediumSlateBlue, Color.MintCream, Color.MediumTurquoise, Color.MediumVioletRed, Color.MidnightBlue, Color.Linen, Color.MistyRose, Color.Moccasin, Color.NavajoWhite, Color.Navy, Color.MediumSpringGreen, Color.LimeGreen, Color.LightSkyBlue, Color.LightYellow, Color.Ivory, Color.Khaki, Color.Lavender, Color.LavenderBlush, Color.LawnGreen, Color.LemonChiffon, Color.LightBlue, Color.LightCoral, Color.Lime, Color.LightCyan, Color.LightGreen, Color.LightGray, Color.LightPink, Color.LightSalmon, Color.LightSeaGreen, Color.OldLace, Color.LightSlateGray, Color.LightSteelBlue, Color.LightGoldenrodYellow, Color.Olive, Color.PaleGoldenrod, Color.Orange, Color.Silver, Color.SkyBlue, Color.SlateBlue, Color.SlateGray, Color.Snow, Color.SpringGreen, Color.SteelBlue, Color.Tan, Color.Sienna, Color.Teal, Color.Tomato, Color.Turquoise, Color.Violet, Color.Wheat, Color.White, Color.WhiteSmoke, Color.Yellow, Color.YellowGreen, Color.Thistle, Color.OliveDrab, Color.SeaShell, Color.SandyBrown, Color.OrangeRed, Color.Orchid, Color.Indigo, Color.PaleGreen, Color.PaleTurquoise, Color.PaleVioletRed, Color.PapayaWhip, Color.PeachPuff, Color.SeaGreen, Color.Peru, Color.Plum, Color.PowderBlue, Color.Red, Color.RosyBrown, Color.RoyalBlue, Color.SaddleBrown, Color.Salmon, Color.Pink, Color.IndianRed, Color.HotPink, Color.Honeydew, Color.DarkGoldenrod, Color.DarkCyan, Color.DarkBlue, Color.Cyan, Color.Crimson, Color.Cornsilk, Color.CornflowerBlue, Color.Coral, Color.Chocolate, Color.Chartreuse, Color.CadetBlue, Color.DarkGray, Color.BurlyWood, Color.BlueViolet, Color.Blue, Color.BlanchedAlmond, Color.Bisque, Color.Beige, Color.Azure, Color.Aquamarine, Color.Aqua, Color.AntiqueWhite, Color.AliceBlue, Color.Brown, Color.DarkGreen, Color.DarkMagenta, Color.DarkKhaki, Color.GreenYellow, Color.Green, Color.Gray, Color.Goldenrod, Color.Gold, Color.GhostWhite, Color.Gainsboro, Color.Fuchsia, Color.ForestGreen, Color.FloralWhite, Color.Firebrick, Color.DodgerBlue, Color.DeepSkyBlue, Color.DeepPink, Color.DarkViolet, Color.DarkTurquoise, Color.DarkSlateGray, Color.DarkSlateBlue, Color.DarkSeaGreen, Color.DarkSalmon, Color.DarkRed, Color.DarkOrchid, Color.DarkOrange, Color.DarkOliveGreen, Color.DimGray };
    public static List<String> colorNames = new List<String> { "MediumSeaGreen", "Magenta", "Maroon", "MediumAquamarine", "MediumBlue", "MediumOrchid", "MediumPurple", "MediumSlateBlue", "MintCream", "MediumTurquoise", "MediumVioletRed", "MidnightBlue", "Linen", "MistyRose", "Moccasin", "NavajoWhite", "Navy", "MediumSpringGreen", "LimeGreen", "LightSkyBlue", "LightYellow", "Ivory", "Khaki", "Lavender", "LavenderBlush", "LawnGreen", "LemonChiffon", "LightBlue", "LightCoral", "Lime", "LightCyan", "LightGreen", "LightGray", "LightPink", "LightSalmon", "LightSeaGreen", "OldLace", "LightSlateGray", "LightSteelBlue", "LightGoldenrodYellow", "Olive", "PaleGoldenrod", "Orange", "Silver", "SkyBlue", "SlateBlue", "SlateGray", "Snow", "SpringGreen", "SteelBlue", "Tan", "Sienna", "Teal", "Tomato", "Turquoise", "Violet", "Wheat", "White", "WhiteSmoke", "Yellow", "YellowGreen", "Thistle", "OliveDrab", "SeaShell", "SandyBrown", "OrangeRed", "Orchid", "Indigo", "PaleGreen", "PaleTurquoise", "PaleVioletRed", "PapayaWhip", "PeachPuff", "SeaGreen", "Peru", "Plum", "PowderBlue", "Red", "RosyBrown", "RoyalBlue", "SaddleBrown", "Salmon", "Pink", "IndianRed", "HotPink", "Honeydew", "DarkGoldenrod", "DarkCyan", "DarkBlue", "Cyan", "Crimson", "Cornsilk", "CornflowerBlue", "Coral", "Chocolate", "Chartreuse", "CadetBlue", "DarkGray", "BurlyWood", "BlueViolet", "Blue", "BlanchedAlmond", "Bisque", "Beige", "Azure", "Aquamarine", "Aqua", "AntiqueWhite", "AliceBlue", "Brown", "DarkGreen", "DarkMagenta", "DarkKhaki", "GreenYellow", "Green", "Gray", "Goldenrod", "Gold", "GhostWhite", "Gainsboro", "Fuchsia", "ForestGreen", "FloralWhite", "Firebrick", "DodgerBlue", "DeepSkyBlue", "DeepPink", "DarkViolet", "DarkTurquoise", "DarkSlateGray", "DarkSlateBlue", "DarkSeaGreen", "DarkSalmon", "DarkRed", "DarkOrchid", "DarkOrange", "DarkOliveGreen", "DimGray" };

    private Sandbox.ModAPI.Ingame.IMyTextSurface _surface;
    private Color _lastBg;
    private Color _lastFore;

    private Color _White;
    public Color White { get { UpdateColors(); return _White; } }
    private Color _Bg;
    public Color Bg { get { UpdateColors(); return _Bg; } }
    private Color _Main;
    public Color Main { get { UpdateColors(); return _Main; } }
    private Color _Main_10;
    public Color Main_10 { get { UpdateColors(); return _Main_10; } }
    private Color _Main_20;
    public Color Main_20 { get { UpdateColors(); return _Main_20; } }
    private Color _Main_30;
    public Color Main_30 { get { UpdateColors(); return _Main_30; } }
    private Color _Main_40;
    public Color Main_40 { get { UpdateColors(); return _Main_40; } }
    private Color _Main_50;
    public Color Main_50 { get { UpdateColors(); return _Main_50; } }
    private Color _Main_60;
    public Color Main_60 { get { UpdateColors(); return _Main_60; } }
    private Color _Main_70;
    public Color Main_70 { get { UpdateColors(); return _Main_70; } }
    private Color _Main_80;
    public Color Main_80 { get { UpdateColors(); return _Main_80; } }
    private Color _Main_90;
    public Color Main_90 { get { UpdateColors(); return _Main_90; } }

    public string Font = MyFontEnum.White;
    public readonly StringBuilder strBuilder = new StringBuilder();
    public Vector2 MeasureStringInPixels(String text, string font, float scale)
    {
      strBuilder.Clear();
      strBuilder.Append(text);
      return _surface.MeasureStringInPixels(strBuilder, font, scale);
    }

    public float Scale = 1f;

    private bool IsBgDark
    {
      get
      {
        var hsv = _surface.ScriptBackgroundColor.ColorToHSV();
        return hsv.Z < 0.125 || (hsv.Y > 0.5 && hsv.Z < 0.25);
      }
    }

    private bool IsMainDark
    {
      get
      {
        var hsv = _surface.ScriptForegroundColor.ColorToHSV();
        return hsv.Z < 0.125 || (hsv.Y > 0.5 && hsv.Z < 0.25);
      }
    }

    public FancyTheme(Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      _surface = surface;
    }

    public void UpdateColors()
    {
      if (_surface.ScriptForegroundColor == _lastFore && _surface.ScriptBackgroundColor == _lastBg)
        return;

      float r = (_surface.ScriptForegroundColor.R + ((1 - _surface.ScriptForegroundColor.R) * 0.6f));
      float g = (_surface.ScriptForegroundColor.G + ((1 - _surface.ScriptForegroundColor.G) * 0.6f));
      float b = (_surface.ScriptForegroundColor.B + ((1 - _surface.ScriptForegroundColor.B) * 0.6f));
      _White = new Color(r, g, b);
      _Bg = FancyUtils.Opac(_surface.ScriptBackgroundColor);
      _Main = FancyUtils.Opac(_surface.ScriptForegroundColor);
      _Main_10 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.1f);
      _Main_20 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.2f);
      _Main_30 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.3f);
      _Main_40 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.4f);
      _Main_50 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.5f);
      _Main_60 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.6f);
      _Main_70 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.7f);
      _Main_80 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.8f);
      _Main_90 = FancyUtils.Opac(_surface.ScriptForegroundColor * 0.9f);

      _lastBg = _surface.ScriptBackgroundColor;
      _lastFore = _surface.ScriptForegroundColor;
    }

  }
}