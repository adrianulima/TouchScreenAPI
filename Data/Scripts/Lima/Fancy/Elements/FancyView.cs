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
    protected Vector2 ChildrenPixels = Vector2.Zero;

    public Color? BgColor;
    public Color? BorderColor;
    public Vector4 Border;
    public Vector4 Padding;
    public int Gap = 0;

    public FancyView(ViewDirection direction = ViewDirection.Column, Color? bgColor = null)
    {
      Direction = direction;
      BgColor = bgColor;
    }

    public override Vector2 GetSize()
    {
      return base.GetSize() - GetExtraBounds();
    }

    public override Vector2 GetFlexSize()
    {
      return base.GetFlexSize() - ChildrenPixels;
    }

    public override Vector2 GetBoundaries()
    {
      return base.GetBoundaries() + GetExtraBounds();
    }

    private void UpdateChildrenPixels()
    {
      ChildrenPixels = Vector2.Zero;
      int childrenCount = Children.Count;
      if (Direction != ViewDirection.None && childrenCount > 0)
      {
        foreach (var child in Children)
        {
          if (Direction == ViewDirection.Row)
            ChildrenPixels.X += child.Pixels.X * ThemeScale;
          else if (Direction == ViewDirection.Column)
            ChildrenPixels.Y += child.Pixels.Y * ThemeScale;
        }
      }
    }

    private void UpdateChildrenPositions()
    {
      FancyElementBase prevChild = null;
      int childrenCount = Children.Count;
      if (Direction != ViewDirection.None && childrenCount > 0)
      {
        var before = new Vector2(Border.X + Padding.X, Border.Y + Padding.Y);
        for (int i = 0; i < childrenCount; i++)
        {
          if (!Children[i].Enabled) continue;

          var childMargin = new Vector2(Children[i].Margin.X, Children[i].Margin.Y);
          if (prevChild == null)
          {
            Children[i].Position = before + childMargin + Position;
            prevChild = Children[i];
            continue;
          }

          var prevChildBounds = prevChild.GetBoundaries();
          var oX = Direction == ViewDirection.Row ? Gap + prevChild.Position.X + prevChildBounds.X + prevChild.Margin.Z - Position.X - Border.X - Padding.X : 0;
          var oY = Direction == ViewDirection.Column ? Gap + prevChild.Position.Y + prevChildBounds.Y + prevChild.Margin.W - Position.Y - Border.Y - Padding.Y : 0;
          Children[i].Position = before + childMargin + Position + new Vector2(oX, oY);
          prevChild = Children[i];
        }
      }
    }

    public override void Update()
    {
      UpdateChildrenPixels();
      UpdateChildrenPositions();

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
        if (Border[s] > 0)
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

      if (Border.X > 0)
      {
        BorderSprites[0].Position = Position + new Vector2(0, (size.Y + GetExtraBounds().Y) / 2);
        BorderSprites[0].Size = new Vector2(Border.X, size.Y + GetExtraBounds().Y);

        Sprites.Add(BorderSprites[0]);
      }
      if (Border.Y > 0)
      {
        BorderSprites[1].Position = Position + new Vector2(0, Border.Y / 2);
        BorderSprites[1].Size = new Vector2(size.X + GetExtraBounds().X, Border.Y);

        Sprites.Add(BorderSprites[1]);
      }
      if (Border.Z > 0)
      {
        BorderSprites[2].Position = Position + new Vector2(size.X + Border.X + Padding.X + Padding.Z, (size.Y + GetExtraBounds().Y) / 2);
        BorderSprites[2].Size = new Vector2(Border.Z, (size.Y + GetExtraBounds().Y));

        Sprites.Add(BorderSprites[2]);
      }
      if (Border.W > 0)
      {
        BorderSprites[3].Position = Position + new Vector2(0, size.Y + Border.Y + Padding.Y + Padding.W + Border.W / 2);
        BorderSprites[3].Size = new Vector2(size.X + GetExtraBounds().X, Border.W);

        Sprites.Add(BorderSprites[3]);
      }
    }

    private Vector2 GetExtraBounds()
    {
      var borderSize = new Vector2(Border.X + Border.Z, Border.Y + Border.W);
      var paddingSize = new Vector2(Padding.X + Padding.Z, Padding.Y + Padding.W);
      return borderSize + paddingSize;
    }
  }
}