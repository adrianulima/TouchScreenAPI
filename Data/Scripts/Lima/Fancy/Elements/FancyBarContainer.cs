using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyBarContainer : FancyView
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

    public FancyView Bar;

    public bool IsVertical;

    public FancyBarContainer(bool vertical = false)
    {
      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Direction = vertical ? ViewDirection.Column : ViewDirection.Row;
      Anchor = vertical ? ViewAlignment.End : ViewAlignment.Start;
      Alignment = ViewAlignment.Center;

      IsVertical = vertical;

      Bar = new FancyView(Direction);
      Bar.Absolute = true;
      AddChild(Bar);
    }

    public override void Update()
    {
      Sprites.Clear();

      var size = GetSize();
      if (Ratio > 0)
      {
        if (IsVertical)
        {
          Bar.Pixels = new Vector2(0, Ratio * size.Y);
          Bar.Scale = new Vector2(1, 0);
        }
        else
        {
          Bar.Pixels = new Vector2(Ratio * size.X, 0);
          Bar.Scale = new Vector2(0, 1);
        }
      }
      else
      {
        Bar.Pixels = Vector2.Zero;
        Bar.Scale = new Vector2(IsVertical ? 1 : 0, !IsVertical ? 1 : 0);
      }

      var anchor = Vector2.Zero;
      if (IsVertical)
        anchor.Y = (1f - Ratio) * size.Y * Offset;
      else
        anchor.X = (1f - Ratio) * size.X * Offset;

      Bar.Position = Position + new Vector2(Padding.X, Padding.Y) + anchor;

      Bar.BgColor = App.Theme.MainColor_7;
      BgColor = App.Theme.MainColor_2;

      base.Update();
    }

  }
}