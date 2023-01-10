using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Touch.UiKit.Elements
{
  public class TouchProgressBar : TouchBarContainer
  {
    protected MySprite textSprite;

    private float _value = 0;
    public float Value
    {
      get { return _value; }
      set { this._value = MathHelper.Clamp(value, MinValue, MaxValue); }
    }

    public override float Ratio
    {
      get { return base.Ratio; }
      set
      {
        base.Ratio = value;
        Value = value * (MaxValue - MinValue) + MinValue;
      }
    }

    public float MinValue;
    public float MaxValue;
    public float BarsGap = 2;
    public TouchLabel Label;

    public TouchProgressBar(float min, float max, bool vertical = false, float barsGap = 0) : base(vertical)
    {
      MinValue = min;
      MaxValue = max;
      Value = max;
      BarsGap = barsGap;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
      Padding = new Vector4(2);

      IsVertical = vertical;

      Label = new TouchLabel("", 0.6f, TextAlignment.CENTER);
      Label.Margin = new Vector4(2, 0, 2, 0);
      AddChild(Label);
    }

    public override void Update()
    {
      Sprites.Clear();

      Ratio = (Value - MinValue) / (MaxValue - MinValue);
      Offset = IsVertical ? 1 : 0;

      base.Update();

      if (UseThemeColors)
        ApplyThemeStyle();

      if (BarsGap > 0 && Ratio > MinValue)
      {
        var size = GetSize();
        var gapSprite = new MySprite()
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
            gapSprite.Position = Position + new Vector2(0, interval + interval * i) + new Vector2(Padding.X, Padding.Y);
            gapSprite.Size = new Vector2(size.X, BarsGap * ThemeScale);
            Bar.GetSprites().Add(gapSprite);
          }
        }
        else
        {
          var count = (int)Math.Round(size.X / (size.Y / 2));
          var interval = size.X / count;
          for (int i = 0; i < count - 1; i++)
          {
            gapSprite.Position = Position + new Vector2(interval + interval * i, size.Y / 2) + new Vector2(Padding.X, Padding.Y);
            gapSprite.Size = new Vector2(BarsGap * ThemeScale, size.Y);
            Bar.GetSprites().Add(gapSprite);
          }
        }
      }
    }

    private void ApplyThemeStyle()
    {
      Label.TextColor = App.Theme.WhiteColor;
      Bar.BgColor = App.Theme.MainColor_7;
      BgColor = App.Theme.MainColor_2;
    }

  }
}