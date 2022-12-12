using System;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyView : FancyElementContainerBase
  {
    public enum ViewDirection : int
    {
      None = 0,
      Row = 1,
      Column = 2
    }

    public ViewDirection Direction = ViewDirection.Column;

    protected MySprite BgSprite;
    protected MySprite[] BorderSprites = new MySprite[4];

    public int Gap = 0;

    // private Vector4 Padding = Vector4.One * 4;

    public Color? BgColor;
    public Color? BorderColor;
    public Vector4 BorderWidth;

    public FancyView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null)
    {
      Direction = direction;
      BgColor = bgColor;
    }

    public override Vector2 GetSize()
    {
      return base.GetSize() - new Vector2(BorderWidth.X + BorderWidth.Z, BorderWidth.Y + BorderWidth.W);
    }

    public override Vector2 GetBoundaries()
    {
      return base.GetBoundaries() + new Vector2(BorderWidth.X + BorderWidth.Z, BorderWidth.Y + BorderWidth.W);
    }

    public override void Update()
    {
      base.Update();

      if (BgColor != null)
      {
        BgSprite = new MySprite()
        {
          Type = SpriteType.TEXTURE,
          Data = "SquareSimple",
          RotationOrScale = 0,
          Color = BgColor
        };
      }

      for (int s = 0; s < BorderSprites.Length; s++)
      {
        if (BorderWidth[s] > 0)
        {
          BorderSprites[s] = new MySprite()
          {
            Type = SpriteType.TEXTURE,
            Data = "SquareSimple",
            RotationOrScale = 0,
            Color = BorderColor ?? App.Theme.Main_60
          };
        }
      }

      Sprites.Clear();

      var size = GetSize();

      if (BgColor != null)
      {
        BgSprite.Position = Position + new Vector2(0, size.Y / 2);
        BgSprite.Size = size;

        Sprites.Add(BgSprite);
      }

      if (BorderWidth.X > 0)
      {
        BorderSprites[0].Position = Position + new Vector2(0, (size.Y + BorderWidth.Y + BorderWidth.W) / 2);
        BorderSprites[0].Size = new Vector2(BorderWidth.X, (size.Y + BorderWidth.Y + BorderWidth.W));

        Sprites.Add(BorderSprites[0]);
      }
      if (BorderWidth.Y > 0)
      {
        BorderSprites[1].Position = Position + new Vector2(0, BorderWidth.Y / 2);
        BorderSprites[1].Size = new Vector2(size.X + BorderWidth.X + BorderWidth.Z, BorderWidth.Y);

        Sprites.Add(BorderSprites[1]);
      }
      if (BorderWidth.Z > 0)
      {
        BorderSprites[2].Position = Position + new Vector2(size.X + BorderWidth.X, (size.Y + BorderWidth.Y + BorderWidth.W) / 2);
        BorderSprites[2].Size = new Vector2(BorderWidth.Z, (size.Y + BorderWidth.Y + BorderWidth.W));

        Sprites.Add(BorderSprites[2]);
      }
      if (BorderWidth.W > 0)
      {
        BorderSprites[3].Position = Position + new Vector2(0, BorderWidth.Y + size.Y + BorderWidth.W / 2);
        BorderSprites[3].Size = new Vector2(size.X + BorderWidth.X + BorderWidth.Z, BorderWidth.W);

        Sprites.Add(BorderSprites[3]);
      }

      if (Direction != ViewDirection.None)
      {
        var before = new Vector2(BorderWidth.X, BorderWidth.Y);
        // var after = new Vector2(BorderWidth.Z, BorderWidth.W);
        for (int i = 0; i < children.Count; i++)
        {
          var childMargin = new Vector2(children[i].Margin.X, children[i].Margin.Y);
          if (i == 0)
          {
            children[0].Position = before + childMargin + Position;
            continue;
          }
          var prevChild = children[i - 1];
          var prevChildBounds = prevChild.GetBoundaries();
          var oX = Direction == ViewDirection.Row ? Gap - Position.X + prevChildBounds.X - prevChild.Margin.X : 0;
          var oY = Direction == ViewDirection.Column ? Gap - Position.Y + prevChildBounds.Y - prevChild.Margin.Y : 0;
          children[i].Position = before + childMargin + Position + new Vector2(oX, oY);
        }
      }
    }
  }
}