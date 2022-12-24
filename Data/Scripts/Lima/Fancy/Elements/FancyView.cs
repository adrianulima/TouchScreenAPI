using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace Lima.Fancy.Elements
{
  public class FancyView : FancyContainerBase
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
    protected Vector2 ChildrenScales = Vector2.One;

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
      return (base.GetFlexSize() - ChildrenPixels) * (1 / ChildrenScales);
    }

    public override Vector2 GetBoundaries()
    {
      return base.GetBoundaries() + GetExtraBounds();
    }

    protected override bool ValidateChild(FancyElementBase child)
    {
      if (child.Absolute)
        return base.ValidateChild(child);

      if (Direction != ViewDirection.None && !child.Absolute)
      {
        var size = GetSize();
        var before = new Vector2(Border.X + Padding.X, Border.Y + Padding.Y);
        var childSize = child.GetSize();
        if (Direction == ViewDirection.Row)
        {
          if (child.Position.X + childSize.X > Position.X + size.X + before.X + 1)
            return false;
          if (child.Position.X < Position.X + before.Y - 1)
            return false;
        }
        else if (Direction == ViewDirection.Column)
        {
          if (child.Position.Y + childSize.Y > Position.Y + size.Y + before.Y + 1)
            return false;
          if (child.Position.Y < Position.Y + before.Y - 1)
            return false;
        }
      }

      return base.ValidateChild(child);
    }

    private void UpdateChildrenPixelsAndScales()
    {
      int childrenCount = Children.Count;
      if (Direction != ViewDirection.None && childrenCount > 0)
      {
        ChildrenPixels = Vector2.Zero;
        ChildrenScales = Vector2.Zero;

        foreach (var child in Children)
        {
          if (child.Absolute)
            continue;

          var extra = child is FancyView ? Vector2.Zero : new Vector2(child.Margin.X + child.Margin.Z, child.Margin.Y + child.Margin.W);

          if (Direction == ViewDirection.Row)
          {
            ChildrenPixels.X += child.Pixels.X * ThemeScale + extra.X;
            ChildrenScales.X += child.Scale.X;
          }
          else if (Direction == ViewDirection.Column)
          {
            ChildrenPixels.Y += child.Pixels.Y * ThemeScale + extra.Y;
            ChildrenScales.Y += child.Scale.Y;
          }
        }

        if (Direction == ViewDirection.Row)
          ChildrenPixels.X += Gap * (childrenCount - 1);
        else if (Direction == ViewDirection.Column)
          ChildrenPixels.Y += Gap * (childrenCount - 1);

        if (ChildrenScales.X == 0)
          ChildrenScales.X = 1;
        if (ChildrenScales.Y == 0)
          ChildrenScales.Y = 1;
      }
    }

    protected virtual void UpdateChildrenPositions()
    {
      FancyElementBase prevChild = null;
      if (Direction != ViewDirection.None)
      {
        var before = new Vector2(Border.X + Padding.X, Border.Y + Padding.Y);
        foreach (var child in Children)
        {
          if (!child.Enabled || child.Absolute) continue;

          var originPos = before + Position + new Vector2(child.Margin.X, child.Margin.Y);

          if (prevChild == null)
          {
            child.Position = originPos;
            prevChild = child;
            continue;
          }

          var prevChildBounds = prevChild.GetBoundaries();
          var oX = Direction == ViewDirection.Row ? Gap + prevChild.Position.X + prevChildBounds.X + prevChild.Margin.Z - Position.X - Border.X - Padding.X : 0;
          var oY = Direction == ViewDirection.Column ? Gap + prevChild.Position.Y + prevChildBounds.Y + prevChild.Margin.W - Position.Y - Border.Y - Padding.Y : 0;
          child.Position = originPos + new Vector2(oX, oY);
          prevChild = child;
        }
      }
    }

    public override void Update()
    {
      UpdateChildrenPixelsAndScales();
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
      var extraBounds = GetExtraBounds();

      if (BgColor != null)
      {
        BgSprite.Position = Position + new Vector2(0, (size.Y + extraBounds.Y) / 2);
        BgSprite.Size = size + extraBounds;

        Sprites.Add(BgSprite);
      }

      if (Border.X > 0)
      {
        BorderSprites[0].Position = Position + new Vector2(0, (size.Y + extraBounds.Y) / 2);
        BorderSprites[0].Size = new Vector2(Border.X, size.Y + extraBounds.Y);

        Sprites.Add(BorderSprites[0]);
      }
      if (Border.Y > 0)
      {
        BorderSprites[1].Position = Position + new Vector2(0, Border.Y / 2);
        BorderSprites[1].Size = new Vector2(size.X + extraBounds.X, Border.Y);

        Sprites.Add(BorderSprites[1]);
      }
      if (Border.Z > 0)
      {
        BorderSprites[2].Position = Position + new Vector2(size.X + Border.X + Padding.X + Padding.Z, (size.Y + extraBounds.Y) / 2);
        BorderSprites[2].Size = new Vector2(Border.Z, (size.Y + extraBounds.Y));

        Sprites.Add(BorderSprites[2]);
      }
      if (Border.W > 0)
      {
        BorderSprites[3].Position = Position + new Vector2(0, size.Y + Border.Y + Padding.Y + Padding.W + Border.W / 2);
        BorderSprites[3].Size = new Vector2(size.X + extraBounds.X, Border.W);

        Sprites.Add(BorderSprites[3]);
      }
    }

    protected virtual Vector2 GetExtraBounds()
    {
      var borderSize = new Vector2(Border.X + Border.Z, Border.Y + Border.W);
      var paddingSize = new Vector2(Padding.X + Padding.Z, Padding.Y + Padding.W);
      return borderSize + paddingSize;
    }
  }
}