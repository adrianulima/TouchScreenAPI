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

    private Color _whiteColor;
    public Color WhiteColor { get { UpdateColors(); return _whiteColor; } }
    private Color _bgColor;
    public Color BgColor { get { UpdateColors(); return _bgColor; } }
    private Color _mainColor;
    public Color MainColor { get { UpdateColors(); return _mainColor; } }
    private Color _main_1;
    public Color MainColor_1 { get { UpdateColors(); return _main_1; } }
    private Color _main_2;
    public Color MainColor_2 { get { UpdateColors(); return _main_2; } }
    private Color _main_3;
    public Color MainColor_3 { get { UpdateColors(); return _main_3; } }
    private Color _main_4;
    public Color MainColor_4 { get { UpdateColors(); return _main_4; } }
    private Color _main_5;
    public Color MainColor_5 { get { UpdateColors(); return _main_5; } }
    private Color _main_6;
    public Color MainColor_6 { get { UpdateColors(); return _main_6; } }
    private Color _main_7;
    public Color MainColor_7 { get { UpdateColors(); return _main_7; } }
    private Color _main_8;
    public Color MainColor_8 { get { UpdateColors(); return _main_8; } }
    private Color _main_9;
    public Color MainColor_9 { get { UpdateColors(); return _main_9; } }

    public float Scale = 1f;

    public string Font = MyFontEnum.White;
    private readonly StringBuilder _strBuilder = new StringBuilder();
    public Vector2 MeasureStringInPixels(String text, string font, float scale)
    {
      _strBuilder.Clear();
      _strBuilder.Append(text);
      return _surface.MeasureStringInPixels(_strBuilder, font, scale);
    }

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
        _whiteColor = new Color(r, g, b);
      }
      else
      {
        _whiteColor = new Color(_surface.ScriptForegroundColor * 0.025f, 1);
      }

      var bgLuma = 1 - (GetColorLuma(_surface.ScriptBackgroundColor) / 255) * 0.5f;
      var bgSum = _surface.ScriptBackgroundColor * 0.5f;

      _bgColor = new Color(_surface.ScriptBackgroundColor, 1);
      _mainColor = new Color(_surface.ScriptForegroundColor, 1);
      _main_9 = new Color((_surface.ScriptForegroundColor * (0.8f * bgLuma) + bgSum), 1);
      _main_8 = new Color((_surface.ScriptForegroundColor * (0.7f * bgLuma) + bgSum), 1);
      _main_7 = new Color((_surface.ScriptForegroundColor * (0.6f * bgLuma) + bgSum), 1);
      _main_6 = new Color((_surface.ScriptForegroundColor * (0.5f * bgLuma) + bgSum), 1);
      _main_5 = new Color((_surface.ScriptForegroundColor * (0.4f * bgLuma) + bgSum), 1);
      _main_4 = new Color((_surface.ScriptForegroundColor * (0.3f * bgLuma) + bgSum), 1);
      _main_3 = new Color((_surface.ScriptForegroundColor * (0.2f * bgLuma) + bgSum), 1);
      _main_2 = new Color((_surface.ScriptForegroundColor * (0.1f * bgLuma) + bgSum), 1);
      _main_1 = new Color((_surface.ScriptForegroundColor * (0.025f * bgLuma) + bgSum), 1);

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