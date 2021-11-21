using System;
using System.Text;
using Lima.Utils;
using VRage.Game;
using VRageMath;

namespace Lima.Fancy
{
  public class FancyTheme
  {
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
      _Bg = ColorUtils.Opac(_surface.ScriptBackgroundColor);
      _Main = ColorUtils.Opac(_surface.ScriptForegroundColor);
      _Main_10 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.1f);
      _Main_20 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.2f);
      _Main_30 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.3f);
      _Main_40 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.4f);
      _Main_50 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.5f);
      _Main_60 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.6f);
      _Main_70 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.7f);
      _Main_80 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.8f);
      _Main_90 = ColorUtils.Opac(_surface.ScriptForegroundColor * 0.9f);

      _lastBg = _surface.ScriptBackgroundColor;
      _lastFore = _surface.ScriptForegroundColor;
    }

  }
}