using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyProgressBar : FancyView
  {
    protected MySprite textSprite;

    private float _value = 0;
    public float Value
    {
      get { return _value; }
      set { this._value = MathHelper.Clamp(value, MinValue, MaxValue); }
    }

    public float MinValue;
    public float MaxValue;
    public float BarsGap = 2;
    public FancyLabel Label;

    public FancyView ProgressBar;

    private bool _isVertical = false;
    public bool IsVertical
    {
      get { return _isVertical; }
      set
      {
        if (_isVertical != value)
        {
          Scale = new Vector2(Scale.Y, Scale.X);
          Pixels = new Vector2(Pixels.Y, Pixels.X);
          _isVertical = value;
        }
      }
    }

    public FancyProgressBar(float min, float max, bool vertical = false, float barsGap = 0)
    {
      MinValue = min;
      MaxValue = max;
      Value = max;
      BarsGap = barsGap;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      Direction = vertical ? ViewDirection.Column : ViewDirection.Row;
      Anchor = vertical ? ViewAlignment.End : ViewAlignment.Start;
      Alignment = ViewAlignment.Center;
      Padding = new Vector4(2);

      IsVertical = vertical;

      ProgressBar = new FancyView(Direction);
      ProgressBar.Absolute = true;
      AddChild(ProgressBar);

      Label = new FancyLabel("", 0.6f, TextAlignment.CENTER);
      Label.Margin = new Vector4(2, 0, 2, 0);
      AddChild(Label);
    }

    public override void Update()
    {
      Sprites.Clear();

      var size = GetSize();
      var ratio = (Value - MinValue) / (MaxValue - MinValue);
      if (ratio > MinValue)
      {
        if (IsVertical)
        {
          ProgressBar.Pixels = new Vector2(0, ratio * size.Y);
          ProgressBar.Scale = new Vector2(1, 0);
        }
        else
        {
          ProgressBar.Pixels = new Vector2(ratio * size.X, 0);
          ProgressBar.Scale = new Vector2(0, 1);
        }
      }
      else
      {
        ProgressBar.Pixels = Vector2.Zero;
        ProgressBar.Scale = new Vector2(IsVertical ? 1 : 0, !IsVertical ? 1 : 0);
      }

      var anchor = Vector2.Zero;
      if (Anchor == ViewAlignment.End)
      {
        if (IsVertical)
          anchor.Y = (1f - ratio) * size.Y;
        else
          anchor.X = (1f - ratio) * size.X;
      }
      ProgressBar.Position = Position + new Vector2(Padding.X, Padding.Y) + anchor;

      Label.TextColor = App.Theme.WhiteColor;
      ProgressBar.BgColor = App.Theme.MainColor_7;
      BgColor = App.Theme.MainColor_2;

      base.Update();

      if (ratio > MinValue)
      {
        if (BarsGap > 0)
        {
          var b = new MySprite()
          {
            Type = SpriteType.TEXTURE,
            Data = "SquareSimple",
            RotationOrScale = 0,
            Color = BgColor
          };
          if (IsVertical)
          {
            var count = (int)Math.Round(size.Y / (size.X / 2));
            var interval = size.Y / count;
            for (int i = 0; i < count - 1; i++)
            {
              b.Position = Position + new Vector2(0, interval + interval * i) + new Vector2(Padding.X, Padding.Y);
              b.Size = new Vector2(size.X, BarsGap * ThemeScale);
              ProgressBar.GetSprites().Add(b);
            }
          }
          else
          {
            var count = (int)Math.Round(size.X / (size.Y / 2));
            var interval = size.X / count;
            for (int i = 0; i < count - 1; i++)
            {
              b.Position = Position + new Vector2(interval + interval * i, size.Y / 2) + new Vector2(Padding.X, Padding.Y);
              b.Size = new Vector2(BarsGap * ThemeScale, size.Y);
              ProgressBar.GetSprites().Add(b);
            }
          }
        }
      }
    }

  }
}