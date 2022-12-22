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

    public Vector2 Range;
    public float Value = 0;
    public int Precision = 1;
    public bool Bars;
    public string Label;
    public float LabelScale = 0.6f;
    public TextAlignment LabelAlignment = TextAlignment.CENTER;

    public FancyProgressBar(float min, float max, bool bars = true, string label = "")
    {
      Range = new Vector2(min, max);
      Value = MathHelper.Clamp(Value, min, max);
      Bars = bars;
      Label = label;

      Scale = new Vector2(1, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      base.Update();

      BgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      ProgressSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_60
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

      var ratio = ((Value - Range.X) / (Range.Y - Range.X));
      var gap = 2 * ThemeScale;
      if (ratio > Range.X)
      {
        ProgressSprite.Position = Position + new Vector2(gap, size.Y / 2);
        ProgressSprite.Size = new Vector2(Math.Max(size.X * ratio - gap * 2, 0), size.Y - gap * 2);
        Sprites.Add(ProgressSprite);

        if (Bars)
        {
          var c = (int)Math.Round((size.X - gap * 2) / (size.Y / 2));
          var interval = (size.X - gap * 2) / c;
          var b = BgSprite;
          for (int i = 0; i < c - 1; i++)
          {
            b.Position = Position + new Vector2(interval + interval * i, size.Y / 2);
            b.Size = new Vector2(gap, size.Y);
            Sprites.Add(b);
          }
        }
      }

      if (Label != "")
      {
        var py = size.Y * 0.5f - (TextSprite.RotationOrScale * 16.6f);
        if (LabelAlignment == TextAlignment.LEFT)
          TextSprite.Position = Position;
        else if (LabelAlignment == TextAlignment.RIGHT)
          TextSprite.Position = Position + new Vector2(GetSize().X - gap * 2, py);
        else
          TextSprite.Position = Position + new Vector2(GetSize().X / 2 + gap * 2, py);
        Sprites.Add(TextSprite);
      }
    }

  }
}