using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyProgressBar : FancyElementBase
  {
    protected MySprite BgSprite;
    protected MySprite ProgressSprite;
    protected MySprite TextSprite;

    private float _value = 0;
    public float Value
    {
      get { return _value; }
      set { this._value = MathHelper.Clamp(value, MinValue, MaxValue); }
    }

    public float MinValue;
    public float MaxValue;
    public int Precision = 1;
    public bool Bars;
    public string Label;
    public float LabelScale = 0.6f;
    public TextAlignment LabelAlignment = TextAlignment.CENTER;

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

    public FancyProgressBar(float min, float max, bool bars = true, bool vertical = false)
    {
      MinValue = min;
      MaxValue = max;
      Value = max;
      Bars = bars;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);

      IsVertical = vertical;
    }

    public override void Update()
    {
      base.Update();

      BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_20
      };

      ProgressSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_70
      };

      if (Label != "")
      {
        TextSprite = new MySprite()
        {
          Type = SpriteType.TEXT,
          Data = Label,
          RotationOrScale = LabelScale * ThemeScale,
          Color = App.Theme.White,//Theme.Main,
          Alignment = LabelAlignment,
          FontId = App.Theme.Font
        };
      }

      Sprites.Clear();

      var size = GetSize();

      BgSprite.Position = Position + new Vector2(0, size.Y / 2);
      BgSprite.Size = size;

      Sprites.Add(BgSprite);

      var ratio = (Value - MinValue) / (MaxValue - MinValue);
      var gap = 2 * ThemeScale;
      if (ratio > MinValue)
      {
        if (IsVertical)
        {
          var sizeProgY = Math.Max(size.Y * ratio - gap * 2, 0);
          ProgressSprite.Size = new Vector2(size.X - gap * 2, sizeProgY);
          ProgressSprite.Position = Position + new Vector2(gap, size.Y - sizeProgY / 2 - gap);
        }
        else
        {
          ProgressSprite.Size = new Vector2(Math.Max(size.X * ratio - gap * 2, 0), size.Y - gap * 2);
          ProgressSprite.Position = Position + new Vector2(gap, size.Y / 2);
        }
        Sprites.Add(ProgressSprite);

        if (Bars)
        {
          if (IsVertical)
          {
            var innerSize = (size.Y - gap * 2);
            var count = (int)Math.Round(innerSize / (size.X / 2));
            var interval = innerSize / count;
            var b = BgSprite;
            for (int i = 0; i < count - 1; i++)
            {
              b.Position = Position + new Vector2(gap, gap + interval + interval * i);
              b.Size = new Vector2(size.X, gap);
              Sprites.Add(b);
            }
          }
          else
          {
            var innerSize = (size.X - gap * 2);
            var count = (int)Math.Round(innerSize / (size.Y / 2));
            var interval = innerSize / count;
            var b = BgSprite;
            for (int i = 0; i < count - 1; i++)
            {
              b.Position = Position + new Vector2(gap + interval + interval * i, size.Y / 2);
              b.Size = new Vector2(gap, size.Y);
              Sprites.Add(b);
            }
          }
        }
      }

      if (Label != "")
      {
        var py = -(TextSprite.RotationOrScale * 16.6f);
        if (IsVertical)
          py += py + size.Y - gap;
        else
          py += size.Y * 0.5f;

        if (LabelAlignment == TextAlignment.LEFT)
          TextSprite.Position = Position;
        else if (LabelAlignment == TextAlignment.RIGHT)
          TextSprite.Position = Position + new Vector2(size.X - gap * 2, py);
        else
          TextSprite.Position = Position + new Vector2(size.X / 2, py);
        Sprites.Add(TextSprite);
      }
    }

  }
}