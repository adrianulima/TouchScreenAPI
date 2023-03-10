using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class BarContainer : View
  {
    private float _ratio = 1;
    public virtual float Ratio
    {
      get { return _ratio; }
      set { this._ratio = MathHelper.Clamp(value, 0, 1); }
    }
    private float _offset = 0;
    public float Offset
    {
      get { return _offset; }
      set { this._offset = MathHelper.Clamp(value, 0, 1); }
    }

    public View Bar;
    public bool IsVertical;

    public BarContainer(bool vertical = false)
    {
      Flex = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Direction = vertical ? ViewDirection.Column : ViewDirection.Row;
      Anchor = vertical ? ViewAnchor.End : ViewAnchor.Start;
      Alignment = ViewAlignment.Center;

      IsVertical = vertical;

      Bar = new View(Direction);
      Bar.Absolute = true;
      AddChild(Bar);
    }

    public override void Update()
    {
      var size = GetSize();
      if (Ratio > 0)
        Bar.Pixels = IsVertical ? new Vector2(0, Ratio * (size.Y / ThemeScale)) : new Vector2(Ratio * (size.X / ThemeScale), 0);
      else
        Bar.Pixels = Vector2.Zero;

      Bar.Flex = IsVertical ? Vector2.UnitX : Vector2.UnitY;

      var anchor = Vector2.Zero;
      if (IsVertical)
        anchor.Y = (1f - Ratio) * (size.Y) * Offset;
      else
        anchor.X = (1f - Ratio) * (size.X) * Offset;

      Bar.Position = Position + new Vector2(Padding.X, Padding.Y) * ThemeScale + anchor;

      if (UseThemeColors)
        ApplyThemeStyle();

      base.Update();
    }

    private void ApplyThemeStyle()
    {
      Bar.BgColor = App.Theme.MainColor_5;
      BgColor = App.Theme.MainColor_2;
    }

  }
}