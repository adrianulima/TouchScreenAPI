using System;
using System.Text;
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
    private readonly StringBuilder _strBuilder = new StringBuilder();
    public Vector2 MeasureStringInPixels(String text, string font, float scale)
    {
      _strBuilder.Clear();
      _strBuilder.Append(text);
      return _surface.MeasureStringInPixels(_strBuilder, font, scale);
    }

    public float Scale = 1f;

    public bool IsBgDark { get { return IsColorDark(_surface.ScriptBackgroundColor); } }

    public bool IsMainDark { get { return IsColorDark(_surface.ScriptForegroundColor); } }

    public FancyTheme(Sandbox.ModAPI.Ingame.IMyTextSurface surface)
    {
      _surface = surface;
    }

    public void UpdateColors()
    {
      if (_surface.ScriptForegroundColor == _lastFore && _surface.ScriptBackgroundColor == _lastBg)
        return;

      if (IsBgDark)
      {
        float r = _surface.ScriptForegroundColor.R + ((1 - _surface.ScriptForegroundColor.R) * 0.6f);
        float g = _surface.ScriptForegroundColor.G + ((1 - _surface.ScriptForegroundColor.G) * 0.6f);
        float b = _surface.ScriptForegroundColor.B + ((1 - _surface.ScriptForegroundColor.B) * 0.6f);
        _White = new Color(r, g, b);
      }
      else
      {
        _White = new Color(_surface.ScriptForegroundColor * 0.025f, 1);
      }

      var bgLuma = 1 - (GetColorLuma(_surface.ScriptBackgroundColor) / 255) * 0.5f;
      var bgSum = _surface.ScriptBackgroundColor * 0.5f;

      _Bg = new Color(_surface.ScriptBackgroundColor, 1);
      _Main = new Color(_surface.ScriptForegroundColor, 1);
      _Main_90 = new Color((_surface.ScriptForegroundColor * (0.8f * bgLuma) + bgSum), 1);
      _Main_80 = new Color((_surface.ScriptForegroundColor * (0.7f * bgLuma) + bgSum), 1);
      _Main_70 = new Color((_surface.ScriptForegroundColor * (0.6f * bgLuma) + bgSum), 1);
      _Main_60 = new Color((_surface.ScriptForegroundColor * (0.5f * bgLuma) + bgSum), 1);
      _Main_50 = new Color((_surface.ScriptForegroundColor * (0.4f * bgLuma) + bgSum), 1);
      _Main_40 = new Color((_surface.ScriptForegroundColor * (0.3f * bgLuma) + bgSum), 1);
      _Main_30 = new Color((_surface.ScriptForegroundColor * (0.2f * bgLuma) + bgSum), 1);
      _Main_20 = new Color((_surface.ScriptForegroundColor * (0.1f * bgLuma) + bgSum), 1);
      _Main_10 = new Color((_surface.ScriptForegroundColor * (0.025f * bgLuma) + bgSum), 1);

      _lastBg = _surface.ScriptBackgroundColor;
      _lastFore = _surface.ScriptForegroundColor;
    }

    private bool IsColorDark(Color color)
    {
      // var hsv = color.ColorToHSV();
      // return hsv.Z < 0.325 || (hsv.Y > 0.5 && hsv.Z < 0.25);
      return GetColorLuma(color) < 160;
    }

    private static float GetColorLuma(Color color)
    {
      var luma = 0.2126f * color.R + 0.7152f * color.G + 0.0722f * color.B;
      return luma;// 0 ... 255
    }

  }
}