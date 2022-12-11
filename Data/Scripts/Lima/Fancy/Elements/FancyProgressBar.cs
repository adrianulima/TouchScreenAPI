using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyProgressBar : FancyElementBase
  {
    protected MySprite bgSprite;
    protected MySprite progressSprite;

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
      Margin = new Vector4(8, 0, 8, 0);
      Pixels = new Vector2(0, 24);
    }

    public override void Update()
    {
      base.Update();

      bgSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_10
      };

      progressSprite = new MySprite()
      {
        Type = SpriteType.TEXTURE,
        Data = "SquareSimple",
        RotationOrScale = 0,
        Color = App.Theme.Main_60
      };

      sprites.Clear();

      bgSprite.Position = Position + new Vector2(0, Size.Y / 2);
      bgSprite.Size = Size;

      var gap = 2;
      var ratio = ((Value - Range.X) / (Range.Y - Range.X));

      progressSprite.Position = Position + new Vector2(gap, Size.Y / 2);
      progressSprite.Size = new Vector2(Size.X * ratio - gap * 2, Size.Y - gap * 2);

      sprites.Add(bgSprite);
      sprites.Add(progressSprite);

      if (Bars)
      {
        var c = (int)Math.Round((Size.X - gap * 2) / (Size.Y / 2));
        var interval = (Size.X - gap * 2) / c;
        var b = bgSprite;
        for (int i = 0; i < c - 1; i++)
        {
          b.Position = Position + new Vector2(interval + interval * i, Size.Y / 2);
          b.Size = new Vector2(gap, Size.Y);
          sprites.Add(b);
        }
      }
    }

  }
}