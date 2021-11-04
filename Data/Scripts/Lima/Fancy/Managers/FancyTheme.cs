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
    private MyTSSCommon _tss;
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
      return _tss.Surface.MeasureStringInPixels(strBuilder, font, scale);
    }

    public float Scale = 1f;

    private bool IsBgDark
    {
      get
      {
        var hsv = _tss.Surface.ScriptBackgroundColor.ColorToHSV();
        return hsv.Z < 0.125 || (hsv.Y > 0.5 && hsv.Z < 0.25);
      }
    }

    private bool IsMainDark
    {
      get
      {
        var hsv = _tss.Surface.ScriptForegroundColor.ColorToHSV();
        return hsv.Z < 0.125 || (hsv.Y > 0.5 && hsv.Z < 0.25);
      }
    }

    public FancyTheme(MyTSSCommon tss)
    {
      _tss = tss;
    }

    public void UpdateColors()
    {
      if (_tss.Surface.ScriptForegroundColor == _lastFore && _tss.Surface.ScriptBackgroundColor == _lastBg)
        return;

      float r = (_tss.Surface.ScriptForegroundColor.R + ((1 - _tss.Surface.ScriptForegroundColor.R) * 0.6f));
      float g = (_tss.Surface.ScriptForegroundColor.G + ((1 - _tss.Surface.ScriptForegroundColor.G) * 0.6f));
      float b = (_tss.Surface.ScriptForegroundColor.B + ((1 - _tss.Surface.ScriptForegroundColor.B) * 0.6f));
      _White = new Color(r, g, b);
      _Bg = FancyUtils.Opac(_tss.Surface.ScriptBackgroundColor);
      _Main = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor);
      _Main_10 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.1f);
      _Main_20 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.2f);
      _Main_30 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.3f);
      _Main_40 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.4f);
      _Main_50 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.5f);
      _Main_60 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.6f);
      _Main_70 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.7f);
      _Main_80 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.8f);
      _Main_90 = FancyUtils.Opac(_tss.Surface.ScriptForegroundColor * 0.9f);

      _lastBg = _tss.Surface.ScriptBackgroundColor;
      _lastFore = _tss.Surface.ScriptForegroundColor;
    }

  }
}