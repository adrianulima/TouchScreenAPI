using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyProgressBar : FancyElementBase
  {
    protected MySprite BgSprite;
    protected MySprite ProgressSprite;

    public Vector2 Range;
    public float Value = 0;
    public int Precision = 1;
    public bool Bars;

    public FancyProgressBar(float min, float max, bool bars = true)
    {
      Range = new Vector2(min, max);
      Value = MathHelper.Clamp(Value, min, max);
      Bars = bars;

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

      Sprites.Clear();

      var size = GetSize();

      BgSprite.Position = Position + new Vector2(0, size.Y / 2);
      BgSprite.Size = size;

      Sprites.Add(BgSprite);

      var ratio = ((Value - Range.X) / (Range.Y - Range.X));

      if (ratio <= Range.X)
        return;

      var gap = 2;
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

  }
}